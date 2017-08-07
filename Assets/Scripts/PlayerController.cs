using System;
using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool m_IsMoving;
        [SerializeField] private bool m_IsRotating;
        [SerializeField] private int m_RotationSpeed = 5;
        [SerializeField] private float m_MovementSpeed = 0.3f;

        
        private Vector2 m_CurrentLocation = new Vector2(0, 0);
        private Vector2 m_TargetLocation = new Vector2(0, 0);
        [SerializeField] private Direction m_CurrentDirection = Direction.North;
        [SerializeField] private Direction m_TargetDirection = Direction.North;
        private Vector2 m_Input;

        private MapGenerator Map {
            get {
                return (MapGenerator)GameObject.FindGameObjectWithTag("Map").GetComponent(typeof(MapGenerator));
            }
        }

        private void Start()
        {
            RotateTo(m_CurrentDirection, false);
        }

        private void Update()
        {
            if (!m_IsRotating && !m_IsMoving)
            {
                if (Input.GetButtonDown("Vertical"))
                {
                    var vertical = Input.GetAxis("Vertical");
                    if (vertical > 0) MoveForward();
                    else if (vertical < 0) MoveBackward();
                }
                else if(Input.GetButtonDown("Strafe"))
                {
                    var strafe = Input.GetAxis("Strafe");
                    if (strafe > 0) StrafeRight();
                    else if (strafe < 0) StrafeLeft();
                }
                else if (Input.GetButtonDown("Horizontal"))
                {
                    var horizontal = Input.GetAxis("Horizontal");
                    if (horizontal < 0) RotateLeft();
                    else if (horizontal > 0) RotateRight();
                }
                else if(Input.GetButtonDown("Action"))
                {
                    var GO = Map.GetInteractive(m_CurrentLocation);
                    if (GO)
                    {
                        var cont = GO.GetComponent<IInteractive>();
                        cont.Activate();
                    }
                }
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
        
        void StrafeLeft() 
        {
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection((Direction)(((int)m_CurrentDirection + 7) % 4));
        }

        void StrafeRight() 
        {
            m_IsMoving = true;
            m_TargetLocation = LocationForDirection((Direction)(((int)m_CurrentDirection + 1) % 4));
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
                case Direction.North: return new Vector2(m_CurrentLocation.x, m_CurrentLocation.y - step);
                case Direction.South: return new Vector2(m_CurrentLocation.x, m_CurrentLocation.y + step);
                case Direction.East:  return new Vector2(m_CurrentLocation.x + step, m_CurrentLocation.y);
                default: /* West */   return new Vector2(m_CurrentLocation.x - step, m_CurrentLocation.y);
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
    }
}
