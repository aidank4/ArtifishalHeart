using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float _playerSpeed = 2f;
    [SerializeField] private float _rotateSpeed = 100f;
    [SerializeField] private Transform fisherModel;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

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
        Vector3 moveDirectionVec3 = new Vector3(moveDirection.x, 0, moveDirection.y);
        rb.linearVelocity = moveDirectionVec3 * _playerSpeed;

        bool moving = moveDirection.magnitude > 0.01f;

        if(moving){
            var targetRot =  Quaternion.LookRotation(moveDirectionVec3, Vector3.up);
            fisherModel.rotation = Quaternion.Slerp(fisherModel.rotation, targetRot, Time.deltaTime * _rotateSpeed); 
        }

        animator.SetBool("WALKING", moving);
    }
}
