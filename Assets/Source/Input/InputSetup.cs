using Players;
using UnityEngine;

namespace Input
{
    public class InputSetup : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Mover _player;
        [SerializeField] private AbilityCaster _caster;

        private PlayerInput _model;
        private InputPresenter _presenter;

        public void Initialize()
        {
            _model = new PlayerInput();
            _presenter = new InputPresenter(_model, _joystick, _player, _caster);
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