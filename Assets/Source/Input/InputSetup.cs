using UnityEngine;

namespace Input
{
    public class InputSetup : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Mover _player;

        private PlayerInput _model;
        private InputPresenter _presenter;

        private void Awake()
        {
            _model = new PlayerInput();
            _presenter = new InputPresenter(_model, _joystick, _player);
        }

        private void OnEnable()
        {
            _presenter.Enable();
        }

        private void OnDisable()
        {
            _presenter.Disable();
        }
    }
}