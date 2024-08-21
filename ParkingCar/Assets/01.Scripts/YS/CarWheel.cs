using UnityEngine;

public class CarWheel : MonoBehaviour
{
    private Rigidbody _rigid;
    private Car _car;

    private float _currentSpeed = 0;
    private bool _isStart;

    public void SetUp()
    {
        _rigid = GetComponentInParent<Rigidbody>();
        _car = GetComponentInParent<Car>();
        EventManager.AddListener<StartParkingEvent>(HandleStartParking);
    }

    private void FixedUpdate()
    {
        if (_isStart == false) return;
        CalculateSpeed();
        _rigid.velocity = transform.forward * _currentSpeed;
    }

    private void CalculateSpeed()
    {
        _currentSpeed += Time.fixedDeltaTime * _car.accel;
        if (_currentSpeed >= _car.speed)
            _currentSpeed = _car.speed;
    }

    private void HandleStartParking(StartParkingEvent evt)
    {
        _isStart = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
