using UnityEngine;

public class CarWheel : MonoBehaviour
{
    private Rigidbody _rigid;
    private Car _car;

    private float _currentSpeed = 0;

    private void Awake()
    {
        _rigid = GetComponentInParent<Rigidbody>();
        _car = GetComponentInParent<Car>();
    }

    private void FixedUpdate()
    {
        CalculateSpeed();
        _rigid.velocity = transform.forward * _currentSpeed;
    }

    private void CalculateSpeed()
    {
        _currentSpeed += Time.fixedDeltaTime * _car.accel;
        if (_currentSpeed >= _car.speed)
            _currentSpeed = _car.speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }
}
