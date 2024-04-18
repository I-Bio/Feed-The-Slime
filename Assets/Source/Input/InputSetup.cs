﻿using Menu;
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

        public void Initialize(ILoader loader)
        {
            _model = new PlayerInput();
            _presenter = new InputPresenter(_model, _joystick, _player, _caster, loader);
            _presenter.Enable();
        }

        private void OnDestroy()
        {
            _presenter.Disable();
        }
    }
}