using System;
using UnityEngine;

namespace Scripts
{
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool m_IsMoving;
        [SerializeField] private bool m_IsRotating;
        [SerializeField] private int m_RotationSpeed = 5;
        [SerializeField] private float m_MovementSpeed = 0.3f;

        
        private Camera m_Camera;
        private CharacterController m_CharacterController;
        private Vector3 m_OriginalCameraPosition;
        private Vector2 m_CurrentLocation = new Vector2(0, 0);
        private Vector2 m_TargetLocation = new Vector2(0, 0);
        [SerializeField] private Direction m_CurrentDirection = Direction.North;
        [SerializeField] private Direction m_TargetDirection = Direction.North;
        private float m_StepCycle;
        private float m_NextStep;
        private AudioSource m_AudioSource;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;

        private MapGenerator Map {
            get {
                return (MapGenerator)GameObject.FindGameObjectWithTag("Map").GetComponent(typeof(MapGenerator));
            }
        }

        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_AudioSource = GetComponent<AudioSource>();

            RotateTo(m_CurrentDirection, false);
        }

        private void FixedUpdate()
        {
            if (!m_IsRotating && !m_IsMoving)
            {
                float vertical = Input.GetAxis("Vertical");
                float horizontal = Input.GetAxis("Horizontal");
                
                if (horizontal < 0) RotateLeft();
                else if (horizontal > 0) RotateRight();
                else if (vertical > 0) MoveForward();
                else if (vertical < 0) MoveBackward();
            }
            else if (m_IsRotating)
            {
                Rotate();
            }
            else if (m_IsMoving)
            {
                Move();
            }
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

        public void RotateTo(Direction direction, bool animate = true)
        {
            if (animate)
            {
                m_IsRotating = true;
                m_TargetDirection = direction;
            }
            else
            {
                transform.rotation = RotationForDirection(direction);
                m_CurrentDirection = direction;
            }
        }

        void Rotate()
        {
            Quaternion target = RotationForDirection(m_TargetDirection);
            if (Quaternion.Angle(transform.rotation, target) < 1)
            {
                transform.rotation = target;
                m_IsRotating = false;
                m_CurrentDirection = m_TargetDirection;
            } 
            else 
            {
                var step = Time.fixedDeltaTime * m_RotationSpeed;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, step);
            }
        }

        Quaternion RotationForDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return Quaternion.LookRotation(new Vector3(-1, 0, 0));
                case Direction.East:  return Quaternion.LookRotation(new Vector3(0, 0, 1));
                case Direction.South: return Quaternion.LookRotation(new Vector3(1, 0, 0));
                default: /* West */   return Quaternion.LookRotation(new Vector3(0, 0, -1));
            }
        }

        void MoveForward() 
        {
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection(m_CurrentDirection);
        }
        void MoveBackward() 
        {
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection(m_CurrentDirection, -1);
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
                m_CurrentLocation = m_TargetLocation;
            } 
            else 
            {
                var step = Time.fixedDeltaTime * m_MovementSpeed;
                transform.position = Vector3.MoveTowards(transform.position, target, step);
            }
        }

        Vector2 LocationForDirection(Direction direction, int step = 1)
        {
            switch (direction)
            {
                case Direction.North: return new Vector2(m_CurrentLocation.x - step, m_CurrentLocation.y);
                case Direction.South: return new Vector2(m_CurrentLocation.x + step, m_CurrentLocation.y);
                case Direction.East:  return new Vector2(m_CurrentLocation.x, m_CurrentLocation.y + step);
                default: /* West */   return new Vector2(m_CurrentLocation.x, m_CurrentLocation.y - step);
            }
        }

        public void MoveTo(Vector2 location, bool animated = true)
        {
            if (!Map.IsWalkable(location)) {
                m_IsMoving = false;
                return;
            }

            if (animated)
            {
                
            }
            else
            {
                m_CurrentLocation = location;
                transform.position = Map.PositionForLocation(location);
            }

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

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // Rigidbody body = hit.collider.attachedRigidbody;
            // //dont move the rigidbody if the character is on top of it
            // if (m_CollisionFlags == CollisionFlags.Below)
            // {
            //     return;
            // }

            // if (body == null || body.isKinematic)
            // {
            //     return;
            // }
            // body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
