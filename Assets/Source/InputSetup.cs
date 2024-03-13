using UnityEngine;

public class InputSetup : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;

    private PlayerInput _model;
    private InputPresenter _presenter;

    private void Awake()
    {
        _model = new PlayerInput();
        _presenter = new InputPresenter(_model, _joystick);
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