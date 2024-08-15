using UnityEngine;

public class CarWheel : MonoBehaviour
{
    private Rigidbody _rigid;

    [SerializeField] private float _moveSpeed = 5f, _accel;
    private float _currentSpeed = 0;

    private void Awake()
    {
        _rigid = GetComponentInParent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CalculateSpeed();
        _rigid.velocity = transform.forward * _currentSpeed;
    }

    private void CalculateSpeed()
    {
        _currentSpeed += Time.fixedDeltaTime * _accel;
        if (_currentSpeed >= _moveSpeed)
            _currentSpeed = _moveSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }
}
