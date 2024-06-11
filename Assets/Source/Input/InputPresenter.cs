using Players;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputPresenter
    {
        private readonly PlayerInput Model;
        private readonly Joystick Joystick;
        private readonly IReader Player;
        private readonly ICaster Caster;

        public InputPresenter(PlayerInput model, Joystick joystick, IReader player, ICaster caster)
        {
            Model = model;
            Joystick = joystick;
            Player = player;
            Caster = caster;
        }

        public void Enable()
        {
            Model.Player.Move.started += OnMoveStarted;
            Model.Player.Move.performed += OnMoved;
            Model.Player.Move.canceled += OnMoveEnded;
            Model.Player.Touch.performed += OnTouched;
            Model.Player.Hide.started += OnHid;
            Model.Player.Hide.performed += OnShowed;
            Model.Player.Spit.started += OnCastStarted;
            Model.Player.Spit.performed += OnCastPerformed;
            Joystick.Released += OnJoystickReleased;

            Model.Enable();
        }

        public void Disable()
        {
            Model.Player.Move.started -= OnMoveStarted;
            Model.Player.Move.performed -= OnMoved;
            Model.Player.Move.canceled -= OnMoveEnded;
            Model.Player.Touch.performed -= OnTouched;
            Model.Player.Hide.started -= OnHid;
            Model.Player.Hide.performed -= OnShowed;
            Model.Player.Spit.started -= OnCastStarted;
            Model.Player.Spit.performed -= OnCastPerformed;
            Joystick.Released -= OnJoystickReleased;

            Model.Disable();
        }
        
        private void OnMoveStarted(InputAction.CallbackContext context)
        {
            Joystick.Activate(context.control.device != Keyboard.current);
        }
        
        private void OnMoved(InputAction.CallbackContext context)
        {
            Player.ReadInput(context.ReadValue<Vector2>());
        }

        private void OnMoveEnded(InputAction.CallbackContext context)
        {
            Joystick.Release();
        }

        private void OnTouched(InputAction.CallbackContext context)
        {
            Vector2 position = Model.Player.ScreenPosition.ReadValue<Vector2>();
            
            if (position == Vector2.zero)
                return;
            
            Joystick.CalculatePosition(position);
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
    }
}