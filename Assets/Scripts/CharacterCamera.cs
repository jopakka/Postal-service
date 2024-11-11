using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Camera))]
public class CharacterCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _objectToFollow;
    [SerializeField] private Vector3 _objectOffset;
    [SerializeField, Range(0f, 80f)] private float _initialCameraAngle = 40f;
    [SerializeField] private float _maxDistanceFromTarget = 5f;
    
    [Header("Look settings")]
    [SerializeField] private bool _autoLockCursor;
    [SerializeField] private bool _inverseY;
    [SerializeField, Min(0.1f)] private float _lookXSpeed = 6f;
    [SerializeField, Min(0.1f)] private float _lookYSpeed = 6f;
    [SerializeField] private float _minVerticalAngle = -30.0f;
    [SerializeField] private float _maxVerticalAngle = 60.0f;

    private InputAction _lookAction;
    private Vector3 _targetOffset;
    private float _currentRotationX;  // Horizontal rotation around the target
    private float _currentRotationY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Cursor.lockState = _autoLockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        _lookAction = InputSystem.actions.FindAction("Look");

        var angles = transform.eulerAngles;
        _currentRotationX = angles.x;
        _currentRotationY = angles.y;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        // HandleCursorLock();
        RotateCamera();
        MoveCamera();
    }

    private void MoveCamera()
    {
        // Calculate the new rotation based on the current rotation values
        var rotation = Quaternion.Euler(_currentRotationY, _currentRotationX, 0);

        // Calculate the new position by moving back by 'distance' from the target
        var offset = new Vector3(0, 0, -_maxDistanceFromTarget);
        var position = rotation * offset + _objectToFollow.position;

        // Apply the calculated position and rotation
        transform.position = position;
        transform.LookAt(_objectToFollow);
    }

    private void RotateCamera()
    {
        var lookValue = _lookAction.ReadValue<Vector2>();
        var inverseY = _inverseY ? 1f : -1f;
        var yValue = lookValue.y * _lookYSpeed * Time.deltaTime * inverseY;
        var xValue = lookValue.x * _lookXSpeed * Time.deltaTime;

        _currentRotationX += xValue;
        _currentRotationY += yValue;
        _currentRotationY = Mathf.Clamp(_currentRotationY, _minVerticalAngle, _maxVerticalAngle);
    }

    private static void HandleCursorLock()
    {
        Cursor.lockState = Cursor.lockState switch
        {
            CursorLockMode.None when Input.GetMouseButtonDown(0) => CursorLockMode.Locked,
            CursorLockMode.Locked when Input.GetKeyDown(KeyCode.Escape) => CursorLockMode.None,
            _ => Cursor.lockState
        };
    }
}
