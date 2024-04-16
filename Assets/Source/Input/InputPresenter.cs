using Menu;
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
        private readonly ILoader _loader;

        public InputPresenter(PlayerInput model, Joystick joystick, IReadable player, ICaster caster, ILoader loader)
        {
            _model = model;
            _joystick = joystick;
            _player = player;
            _caster = caster;
            _loader = loader;
        }

        public void Enable()
        {
            _model.Player.Move.performed += OnMoved;
            _model.Player.Touch.performed += OnTouched;
            _model.Player.Hide.started += OnHid;
            _model.Player.Hide.performed += OnShowed;
            _model.Player.Spit.started += OnCastStarted;
            _model.Player.Spit.performed += OnCastPerformed;
            _model.Player.Load.performed += OnLoadPerformed;
            _joystick.Released += OnJoystickReleased;

            _model.Enable();
        }

        public void Disable()
        {
            _model.Player.Move.performed -= OnMoved;
            _model.Player.Touch.performed -= OnTouched;
            _model.Player.Hide.started -= OnHid;
            _model.Player.Hide.performed -= OnShowed;
            _model.Player.Spit.started -= OnCastStarted;
            _model.Player.Spit.performed -= OnCastPerformed;
            _model.Player.Load.performed -= OnLoadPerformed;
            _joystick.Released -= OnJoystickReleased;

            _model.Disable();
        }

        private void OnMoved(InputAction.CallbackContext context)
        {
            _player.ReadInput(context.ReadValue<Vector2>());
        }

        private void OnTouched(InputAction.CallbackContext context)
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

        private void OnHid(InputAction.CallbackContext context)
        {
            _caster.Hide();
        }
        
        private void OnShowed(InputAction.CallbackContext context)
        {
            _caster.Show();
        }
        
        private void OnCastStarted(InputAction.CallbackContext context)
        {
            _caster.DrawCastTrajectory();
        }
        
        private void OnCastPerformed(InputAction.CallbackContext context)
        {
            _caster.CastSpit();
        }

        private void OnLoadPerformed(InputAction.CallbackContext context)
        {
            _loader.Load();
        }
    }
}