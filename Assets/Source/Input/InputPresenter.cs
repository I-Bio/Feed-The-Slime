using Menu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputPresenter : IPresenter
    {
        private readonly PlayerInput Model;
        private readonly Joystick Joystick;
        private readonly IReadable Player;
        private readonly ICaster Caster;
        private readonly ILoader Loader;

        public InputPresenter(PlayerInput model, Joystick joystick, IReadable player, ICaster caster, ILoader loader)
        {
            Model = model;
            Joystick = joystick;
            Player = player;
            Caster = caster;
            Loader = loader;
        }

        public void Enable()
        {
            Model.Player.Move.performed += OnMoved;
            Model.Player.Touch.performed += OnTouched;
            Model.Player.Hide.started += OnHid;
            Model.Player.Hide.performed += OnShowed;
            Model.Player.Spit.started += OnCastStarted;
            Model.Player.Spit.performed += OnCastPerformed;
            Model.Player.Load.performed += OnLoadPerformed;
            Joystick.Released += OnJoystickReleased;

            Model.Enable();
        }

        public void Disable()
        {
            Model.Player.Move.performed -= OnMoved;
            Model.Player.Touch.performed -= OnTouched;
            Model.Player.Hide.started -= OnHid;
            Model.Player.Hide.performed -= OnShowed;
            Model.Player.Spit.started -= OnCastStarted;
            Model.Player.Spit.performed -= OnCastPerformed;
            Model.Player.Load.performed -= OnLoadPerformed;
            Joystick.Released -= OnJoystickReleased;

            Model.Disable();
        }

        private void OnMoved(InputAction.CallbackContext context)
        {
            Player.ReadInput(context.ReadValue<Vector2>());
        }

        private void OnTouched(InputAction.CallbackContext context)
        {
            if (Joystick.isActiveAndEnabled == false)
                return;
            
            Vector2 position = Model.Player.ScreenPosition.ReadValue<Vector2>();
            
            if (position == Vector2.zero)
                return;
            
            Joystick.Activate(position);
        }

        private void OnJoystickReleased()
        {
            Player.ReadInput(Vector2.zero);
        }

        private void OnHid(InputAction.CallbackContext context)
        {
            Caster.Hide();
        }
        
        private void OnShowed(InputAction.CallbackContext context)
        {
            Caster.Show();
        }
        
        private void OnCastStarted(InputAction.CallbackContext context)
        {
            Caster.DrawCastTrajectory();
        }
        
        private void OnCastPerformed(InputAction.CallbackContext context)
        {
            Caster.CastSpit();
        }

        private void OnLoadPerformed(InputAction.CallbackContext context)
        {
            Loader.Load();
        }
    }
}