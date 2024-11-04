using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementManager : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _sprintSpeed = 5f;

    private CharacterController _characterController;
    private InputAction _moveAction;
    private InputAction _sprintAction;
    private bool _isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _moveAction = InputSystem.actions.FindAction("Move");
        _sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        _isSprinting = _sprintAction.IsPressed();
        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        float elapsedTime = Time.deltaTime;

        float speed = _isSprinting ? _sprintSpeed : _speed;

        _characterController.Move(new Vector3(moveValue.x * elapsedTime * speed, 0f, moveValue.y * elapsedTime * speed));
    }
}
