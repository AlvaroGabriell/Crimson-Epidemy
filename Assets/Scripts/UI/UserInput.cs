using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 Movement {get; private set;}
    public bool MoveUp { get; private set; }
    public bool MoveDown { get; private set; }
    public bool MoveLeft { get; private set; }
    public bool MoveRight { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _movement;
    private InputAction _moveUp;
    private InputAction _moveDown;
    private InputAction _moveLeft;
    private InputAction _moveRight;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _playerInput = GetComponent<PlayerInput>();
        SetInputActions();
    }

    private void SetInputActions()
    {
        _movement = _playerInput.actions["Move"];
        _moveUp = _playerInput.actions["Move"];
        _moveDown = _playerInput.actions["Move"];
        _moveLeft = _playerInput.actions["Move"];
        _moveRight = _playerInput.actions["Move"];
    }

    private void UpdateInputs()
    {
        Movement = _movement.ReadValue<Vector2>();
        MoveUp = _moveUp.WasPerformedThisFrame();
        MoveDown = _moveDown.WasPerformedThisFrame();
        MoveLeft = _moveLeft.WasPerformedThisFrame();
        MoveRight = _moveRight.WasPerformedThisFrame();
    }
}
