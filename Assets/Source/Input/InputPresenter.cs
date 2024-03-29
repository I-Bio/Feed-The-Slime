using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputPresenter : IPresenter
    {
        private readonly PlayerInput _model;
        private readonly Joystick _joystick;
        private readonly IReadable _player;
        private readonly ICaster _caster;

        public InputPresenter(PlayerInput model, Joystick joystick, IReadable player, ICaster caster)
        {
            _model = model;
            _joystick = joystick;
            _player = player;
            _caster = caster;
        }

        public void Enable()
        {
            _model.Player.Move.performed += OnMove;
            _model.Player.Touch.performed += OnTouch;
            _model.Player.Hide.started += OnHide;
            _model.Player.Hide.performed += OnShow;
            _joystick.Released += OnJoystickReleased;

            _model.Enable();
        }

        public void Disable()
        {
            _model.Player.Move.performed -= OnMove;
            _model.Player.Touch.performed -= OnTouch;
            _model.Player.Hide.started -= OnHide;
            _model.Player.Hide.performed -= OnTouch;
            _joystick.Released -= OnJoystickReleased;

            _model.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _player.ReadInput(context.ReadValue<Vector2>());
        }

        private void OnTouch(InputAction.CallbackContext context)
        {
            Vector2 position = _model.Player.ScreenPosition.ReadValue<Vector2>();
            
            if (position == Vector2.zero)
                return;
            
            _joystick.Activate(position);
        }

        private void OnJoystickReleased()
        {
            _player.ReadInput(Vector2.zero);
        }

        private void OnHide(InputAction.CallbackContext context)
        {
            _caster.Hide();
        }
        
        private void OnShow(InputAction.CallbackContext context)
        {
            _caster.Show();
        }
        
        private void OnStartCast(InputAction.CallbackContext context)
        {
            _caster.DrawCastTrajectory();
        }
        
        private void OnPerformCast()
        {
            _caster.CastSpit();
        }
    }
}