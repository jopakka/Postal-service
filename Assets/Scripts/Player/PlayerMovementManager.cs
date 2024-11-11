using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMovementManager : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _sprintSpeed = 5f;
        [SerializeField] private Transform _camera;

        private Rigidbody _rigidbody;
        private InputAction _moveAction;
        private InputAction _sprintAction;
        private Vector3 _movement;
        private bool _isSprinting;
        private bool _isEnabled = true;
        private float _groundAngle;
        private CapsuleCollider _capsuleCollider;

        public int Speed => Mathf.RoundToInt(_rigidbody.linearVelocity.magnitude);

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            _moveAction = InputSystem.actions.FindAction("Move");
            _sprintAction = InputSystem.actions.FindAction("Sprint");
        }

        // Update is called once per frame
        private void Update()
        {
            CalculateGround();
            Move();
        }

        private void Move()
        {
            if (!_isEnabled)
            {
                _movement = Vector3.zero;
                return;
            }

            _isSprinting = _sprintAction.IsPressed();
            var moveValue = _moveAction.ReadValue<Vector2>();

            var direction = new Vector3(moveValue.x, 0f, moveValue.y).normalized;

            if (!(direction.magnitude > 0.1f))
            {
                _movement = Vector3.zero;
                return;
            }

            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            var targetRotation = Quaternion.Euler(0, targetAngle, 0);
            
            _movement = targetRotation * Vector3.forward;
        }

        private void FixedUpdate()
        {
            var speed = _isSprinting ? _sprintSpeed : _speed;
            _rigidbody.linearVelocity = speed * 40f * Time.deltaTime * _movement;
        }

        private void CalculateGround()
        {
            var feetRayDistance = _capsuleCollider.height / 2f + 0.5f;
            // Cast a ray from the bottom of the object downwards
            if (!Physics.SphereCast(transform.position, _capsuleCollider.radius, Vector3.down, out var hit, feetRayDistance)) return;

            // Get the normal of the surface we hit
            var groundNormal = hit.normal;

            // Calculate the angle between the ground normal and the object's up direction
            _groundAngle = Vector3.Angle(groundNormal, Vector3.up);

            // Optional: Draw the ray and normal in the editor for visualization
            Debug.DrawRay(hit.point, groundNormal, Color.green);
        }

        public void EnableMovement()
        {
            _isEnabled = true;
        }

        public void DisableMovement()
        {
            _isEnabled = false;
        }
    }
}