using UnityEngine;
using UnityEngine.InputSystem;

public class InputPresenter
{
    private readonly PlayerInput _model;
    private readonly Joystick _joystick;

    public InputPresenter(PlayerInput model, Joystick joystick)
    {
        _model = model;
        _joystick = joystick;
    }

    public void Enable()
    {
        _model.Player.Move.performed += OnMove;
        _model.Player.Touch.performed += OnTouch;

        _model.Enable();
    }

    public void Disable()
    {
        _model.Player.Move.performed -= OnMove;
        _model.Player.Touch.performed -= OnTouch;

        _model.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
    }

    private void OnTouch(InputAction.CallbackContext context)
    {
        Vector2 position = _model.Player.ScreenPosition.ReadValue<Vector2>();
            
        if (position == Vector2.zero)
            return;
            
        _joystick.Activate(position);
    }
}