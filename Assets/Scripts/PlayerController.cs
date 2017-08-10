using System;
using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
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

        private MapGenerator Map {
            get {
                return GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>();
            }
        }

        private void Start()
        {
            RotateTo(m_CurrentDirection, false);
        }

        private void Update()
        {
            MiniMapController.getInstance().PlayerLocation = m_CurrentLocation;

            var action = GetAction();
            if (action != null)
            {
                action();
            }
        }

        Action GetAction()
        {
            Action action;
            if ((action = PerformWalkOnAction()) != null)
            {
                m_WalkActionFired = true;
                return action;
            }

            if (m_IsRotating) return Rotate;
            if (m_IsMoving) return Move;

            var vertical = Input.GetAxis("Vertical");
            if (vertical > 0) return MoveForward;
            if (vertical < 0) return MoveBackward;

            var strafe = Input.GetAxis("Strafe");
            if (strafe > 0) return StrafeRight;
            if (strafe < 0) return StrafeLeft;

            var horizontal = Input.GetAxis("Horizontal");

            if (horizontal < 0) return RotateLeft;
            if (horizontal > 0) return RotateRight;

            if(Input.GetButtonDown("Action")) return PerformAction;

            return null;
        }

        void PerformAction()
        {
            var mapBlock = Map.GetBlockAtLocation(m_CurrentLocation);
            if (mapBlock == null || !mapBlock.Interactive) return;
            
            var component = mapBlock.GameObject.GetComponent<IInteractive>();
            if (component == null) return;
            component.Activate();
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

            var component = mapBlock.GameObject.GetComponent<ITeleport>();
            if (component == null) return null;

            return () => { component.Teleport(); };
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
            if (mapBlock == null || !mapBlock.Interactive) return null;
            
            var component = mapBlock.GameObject.GetComponent<IPressable>();
            if (component == null) return null;
            return () => { component.Press(state); };
        }

        void RotateLeft() 
        {
            m_IsRotating = true;
            m_TargetDirection = (Direction)(((int)m_CurrentDirection + 7) % 4);
        }

        void RotateRight() 
        {
            m_IsRotating = true;
            m_TargetDirection = (Direction)(((int)m_CurrentDirection + 1) % 4);
        }
        
        void StrafeLeft() 
        {
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection((Direction)(((int)m_CurrentDirection + 7) % 4));
            PlayHeadBob();
            m_WalkActionFired = false;
        }

        void StrafeRight() 
        {
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection((Direction)(((int)m_CurrentDirection + 1) % 4));
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
            } 
            else 
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, step);
            }
        }

        void MoveForward() 
        {
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection(m_CurrentDirection);
            PlayHeadBob();
            m_WalkActionFired = false;
        }
        
        void MoveBackward() 
        {
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection(m_CurrentDirection, -1);
            PlayHeadBob();
            m_WalkActionFired = false;
        }

        void Move()
        {
            if (!Map.IsWalkable(m_TargetLocation)) {
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
            } 
            else 
            {
                var step = Time.deltaTime * m_MovementSpeed;
                transform.position = Vector3.MoveTowards(transform.position, target, step);
            }
        }

        Vector2 LocationForDirection(Direction direction, int step = 1)
        {
            switch (direction)
            {
                case Direction.North: return new Vector2(m_CurrentLocation.x, m_CurrentLocation.y - step);
                case Direction.South: return new Vector2(m_CurrentLocation.x, m_CurrentLocation.y + step);
                case Direction.East:  return new Vector2(m_CurrentLocation.x + step, m_CurrentLocation.y);
                default: /* West */   return new Vector2(m_CurrentLocation.x - step, m_CurrentLocation.y);
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
        }
    }
}
