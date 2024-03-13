using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _rotationPoint;

    private Rigidbody _rigidbody;
    private Vector2 _input;
    private Vector3 _forward;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _forward = transform.forward;
    }

    private void FixedUpdate()
    {
        Move();
        RotateAlongMove();
    }

    public void ReadInput(Vector2 input)
    {
        _input = input;
    }

    private void Move()
    {
        if (_rigidbody.velocity == Vector3.zero && _input == Vector2.zero)
            return;

        _rigidbody.velocity = new Vector3(_input.x, 0f, _input.y) * _speed;
    }

    private void RotateAlongMove()
    {
        if (_input == Vector2.zero)
            return;
        
        Vector3 direction = new Vector3(_input.x, 0f, _input.y);
        _rotationPoint.eulerAngles = new Vector3(0f, Vector3.SignedAngle(_forward, direction, Vector3.up), 0f);
    }
}