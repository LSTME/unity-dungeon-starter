using System;
using System.Collections.Generic;
using System.Threading;
using Scripts.AI;
using UnityEngine;
using Scripts.Interfaces;

namespace Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerController : MonoBehaviour
    {
        private Animator animator
        {
            get { return GetComponent<Animator>(); }
        }

        [SerializeField] private bool m_IsMoving;
        [SerializeField] private bool m_IsRotating;
        [SerializeField] private int m_RotationSpeed = 5;
        [SerializeField] private float m_MovementSpeed = 0.3f;
        [SerializeField] private bool m_TogglePressables;
        [SerializeField] private bool m_WalkActionFired = false;

        private Vector2 m_CurrentLocation = new Vector2(0, 0);
        private Vector2 m_TargetLocation = new Vector2(0, 0);
        private Vector2 m_LastLocation = new Vector2(0, 0);
        [SerializeField] private Direction m_CurrentDirection = Direction.North;
        [SerializeField] private Direction m_TargetDirection = Direction.North;
        private Vector2 m_Input;

        public bool enableKeyboardInteraction = false;

        private string IssuedAction = "";
        private float IssuedActionValue = 0.0f;

        private System.Collections.Generic.HashSet<string> knownActions =
            new System.Collections.Generic.HashSet<string>();

        private GameObject PickedUpObject = null;

        public static PlayerController getInstance()
        {
            return GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        private static readonly ManualResetEvent _interpreterLock = new ManualResetEvent(true);

        public static ManualResetEvent InterpreterLock
        {
            get { return _interpreterLock; }
        }

        private static readonly Queue<Action> _actionQueue = new Queue<Action>();

        public static Queue<Action> ActionQueue
        {
            get { return _actionQueue; }
        }

        public bool IsObjectPickedUp()
        {
            return PickedUpObject != null;
        }

        public bool PickUpObject(GameObject GO)
        {
            if (IsObjectPickedUp()) return false;

            PickedUpObject = GO;

            return true;
        }

        public GameObject PutDownObject()
        {
            if (!IsObjectPickedUp()) return null;

            var result = PickedUpObject;

            PickedUpObject = null;

            return result;
        }

        private MapGenerator Map
        {
            get { return GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>(); }
        }

        private void Start()
        {
            knownActions.Add("Strafe");
            knownActions.Add("Vertical");
            knownActions.Add("Horizontal");
            knownActions.Add("Action");
            knownActions.Add("");

            RotateTo(m_CurrentDirection, false);

            new Thread(o =>
            {
                new Player().Run();
            }).Start();
        }

        private void Update()
        {
            MiniMapController.getInstance().PlayerLocation = m_CurrentLocation;
            visitCurrentLocationOnMinimap();

            var action = GetAction();
            if (action != null)
            {
                action();
            }
        }

        public Vector2 CurrentLocation
        {
            get { return m_CurrentLocation; }
        }

        public Direction CurrentDirection
        {
            get { return m_CurrentDirection; }
        }

        Action GetAction()
        {
            if (Input.GetButtonDown("InputToggle"))
            {
                enableKeyboardInteraction = !enableKeyboardInteraction;
            }

            Action action;

            if ((action = PerformWalkOnAction()) != null)
            {
                m_WalkActionFired = true;
                return action;
            }

            if (m_IsRotating) return Rotate;
            if (m_IsMoving) return Move;

            var vertical = GetFloatValueAction("Vertical");
            if (vertical > 0) return MoveForward;
            if (vertical < 0) return MoveBackward;

            var strafe = GetFloatValueAction("Strafe");
            if (strafe > 0) return StrafeRight;
            if (strafe < 0) return StrafeLeft;

            var horizontal = GetFloatValueAction("Horizontal");

            if (horizontal < 0) return RotateLeft;
            if (horizontal > 0) return RotateRight;

            if (GetBoolValueAction("Action")) return PerformAction;

            if (Input.GetButtonDown("LargeMap"))
            {
                MiniMapController.getInstance().SwitchLarge();
            }
            
            if (_actionQueue.Count > 0)
                return _actionQueue.Dequeue();

            return null;
        }

        public void IssueAction(string ActionType, float ActionValue = 0.0f)
        {
            if (!knownActions.Contains(ActionType)) return;

            IssuedAction = ActionType;
            IssuedActionValue = ActionValue;
        }

        private float GetFloatValueAction(string ActionType)
        {
            float input = Input.GetAxis(ActionType);
            if (input != 0.0f && enableKeyboardInteraction)
            {
                return input;
            }
            if (IssuedAction.Equals(ActionType))
            {
                IssuedAction = "";
                return IssuedActionValue;
            }

            return 0.0f;
        }

        private bool GetBoolValueAction(string ActionType)
        {
            bool input = Input.GetButtonDown(ActionType);
            if (input && enableKeyboardInteraction)
            {
                return input;
            }
            if (IssuedAction.Equals(ActionType))
            {
                IssuedAction = "";
                return true;
            }

            return false;
        }

        public void PerformAction()
        {
            if (PerformPutDownAction()) return;

            if (PerformActionOnThisCell()) return;

            PefrormActionOnNextCell();
        }

        public bool PerformPutDownAction()
        {
            if (!PlayerController.getInstance().IsObjectPickedUp()) return false;

            var mapBlock = Map.GetBlockAtLocation(LocationForDirection(m_CurrentDirection, 1));
            if (mapBlock == null) return false;

            foreach (var gameObject in mapBlock.GameObjects)
            {
                var component = gameObject.GetComponent<IDropable>();
                if (component != null)
                {
                    component.DropObject();
                    break;
                }
            }

            return true;
        }

        private void PefrormActionOnNextCell()
        {
            var mapBlock = Map.GetBlockAtLocation(LocationForDirection(m_CurrentDirection, 1));
            if (mapBlock == null) return;

            foreach (var gameObject in mapBlock.GameObjects)
            {
                var component = gameObject.GetComponent<IInteractive>();
                if (component == null) continue;
                component.Activate();
            }
        }

        public bool PerformActionOnThisCell()
        {
            var mapBlock = Map.GetBlockAtLocation(m_CurrentLocation);
            if (mapBlock == null) return false;

            bool CloseActionPerformed = false;

            foreach (var gameObject in mapBlock.GameObjects)
            {
                var component = gameObject.GetComponent<IInteractive>();
                if (component == null) continue;
                CloseActionPerformed |= component.Activate();
            }

            return CloseActionPerformed;
        }

        Action PerformWalkOnAction()
        {
            if (m_WalkActionFired) return null;

            Action action;
            if ((action = TeleportAction()) != null) return action;

            if ((action = PressFloorButtonAction()) != null) return action;

            return null;
        }

        Action TeleportAction()
        {
            var mapBlock = Map.GetBlockAtLocation(m_CurrentLocation);
            if (mapBlock == null || mapBlock.Type != "teleport") return null;

            foreach (var gameObject in mapBlock.GameObjects)
            {
                var component = gameObject.GetComponent<ITeleport>();
                if (component == null) continue;

                return () => { component.Teleport(); };
            }

            return null;
        }

        Action PressFloorButtonAction()
        {
            Action press = PressPressableAtLocation(m_CurrentLocation);
            Action unpress = PressPressableAtLocation(m_LastLocation, false);

            m_TogglePressables = false;

            if (press == null && unpress == null) return null;

            return () =>
            {
                if (press != null) press();
                if (unpress != null) unpress();
            };
        }

        Action PressPressableAtLocation(Vector2 location, bool state = true)
        {
            if (!m_TogglePressables) return null;

            var mapBlock = Map.GetBlockAtLocation(location);
            if (mapBlock == null) return null;

            foreach (var gameObject in mapBlock.GameObjects)
            {
                var component = gameObject.GetComponent<IPressable>();
                if (component == null) continue;
                return () => { component.Press(state); };
            }

            return null;
        }

        public void RotateLeft()
        {
            if (m_IsRotating) return;

            m_IsRotating = true;
            m_TargetDirection = (Direction) (((int) m_CurrentDirection + 7) % 4);
        }

        public void RotateRight()
        {
            if (m_IsRotating) return;

            m_IsRotating = true;
            m_TargetDirection = (Direction) (((int) m_CurrentDirection + 1) % 4);
        }

        public void StrafeLeft()
        {
            if (m_IsMoving) return;
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection((Direction) (((int) m_CurrentDirection + 7) % 4));
            PlayHeadBob();
            m_WalkActionFired = false;
        }

        public void StrafeRight()
        {
            if (m_IsMoving) return;
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection((Direction) (((int) m_CurrentDirection + 1) % 4));
            PlayHeadBob();
            m_WalkActionFired = false;
        }

        public void RotateTo(Direction direction, bool animate = true)
        {
            if (animate)
            {
                m_IsRotating = true;
                m_TargetDirection = direction;
            }
            else
            {
                transform.rotation = direction.GetRotation();
                m_CurrentDirection = direction;
            }
        }

        void Rotate()
        {
            Quaternion target = m_TargetDirection.GetRotation();
            var step = Time.deltaTime * m_RotationSpeed;
            if (Quaternion.Angle(transform.rotation, target) < step)
            {
                transform.rotation = target;
                m_IsRotating = false;
                m_CurrentDirection = m_TargetDirection;
                InterpreterLock.Set();
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, step);
            }
        }

        public void MoveForward()
        {
            if (m_IsMoving) return;
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection(m_CurrentDirection);
            PlayHeadBob();
            m_WalkActionFired = false;
        }

        public void MoveBackward()
        {
            if (m_IsMoving) return;
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection(m_CurrentDirection, -1);
            PlayHeadBob();
            m_WalkActionFired = false;
        }

        void Move()
        {
            if (!Map.IsWalkable(m_TargetLocation))
            {
                m_IsMoving = false;
                return;
            }

            Vector3 target = Map.PositionForLocation(m_TargetLocation);
            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                transform.position = target;
                m_IsMoving = false;
                m_LastLocation = m_CurrentLocation;
                m_CurrentLocation = m_TargetLocation;
                m_TogglePressables = true;
                InterpreterLock.Set();
            }
            else
            {
                var step = Time.deltaTime * m_MovementSpeed;
                transform.position = Vector3.MoveTowards(transform.position, target, step);
            }
        }

        public Vector2 LocationForDirection(Direction direction, int step = 1)
        {
            switch (direction)
            {
                case Direction.North: return new Vector2(m_CurrentLocation.x, m_CurrentLocation.y - step);
                case Direction.South: return new Vector2(m_CurrentLocation.x, m_CurrentLocation.y + step);
                case Direction.East: return new Vector2(m_CurrentLocation.x + step, m_CurrentLocation.y);
                default: /* West */ return new Vector2(m_CurrentLocation.x - step, m_CurrentLocation.y);
            }
        }

        public void MoveTo(Vector2 location)
        {
            m_CurrentLocation = location;
            transform.position = Map.PositionForLocation(location);
            m_IsMoving = false;
        }

        private void GetInput()
        {
            // Read input
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }
        }

        private void PlayHeadBob()
        {
            var sound = GetComponent<AudioSource>();
            sound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            sound.Play();

            var anim = transform.Find("Camera").gameObject.GetComponent<Animator>();
            anim.Play("head_bob");
        }

        public void MovePlayer(Vector2 location)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            PlayerController controller = player.GetComponent<PlayerController>();

            controller.MoveTo(location);
            m_LastLocation = m_CurrentLocation;
            m_CurrentLocation = location;
            transform.position = Map.PositionForLocation(location);
            m_TogglePressables = true;
            visitCurrentLocationOnMinimap();
        }

        private void visitCurrentLocationOnMinimap()
        {
            var miniMapController = MiniMapController.getInstance();
            miniMapController.visit(m_CurrentLocation);
        }
    }
}