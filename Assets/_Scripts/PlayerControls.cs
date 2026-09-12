using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float _playerSpeed = 2f;

    private PlayerInput _playerInput;
    private InputAction _moveAction;


    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector2 moveDirection = _moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(moveDirection.x, 0, moveDirection.y) * (_playerSpeed * Time.deltaTime);
        Debug.Log(moveDirection);
    }
}
