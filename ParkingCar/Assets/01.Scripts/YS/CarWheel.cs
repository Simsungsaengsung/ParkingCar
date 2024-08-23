using System;
using UnityEngine;

public class CarWheel : MonoBehaviour
{
    private Rigidbody _rigid;
    private Car _car;

    private float _currentSpeed = 0;
    private bool _isStart;
    private bool _isSuccess;

    private GameObject _arrow;

    public void SetUp()
    {
        _rigid = GetComponentInParent<Rigidbody>();
        _car = GetComponentInParent<Car>();
        transform.GetChild(0).gameObject.SetActive(true);
        EventManager.AddListener<StartParkingEvent>(HandleStartParking);
        EventManager.AddListener<CarExitsMapEvent>(HandleCarExitsMapEvent);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<StartParkingEvent>(HandleStartParking);
        EventManager.RemoveListener<CarExitsMapEvent>(HandleCarExitsMapEvent);
    }

    private void FixedUpdate()
    {
        if (_isSuccess) return;
        CalculateSpeed();
        _rigid.velocity = transform.forward * _currentSpeed;
    }

    private void CalculateSpeed()
    {
        if (_isStart)
        {
            _currentSpeed += Time.fixedDeltaTime * _car.accel;
            if (_currentSpeed > _car.speed)
                _currentSpeed = _car.speed;
        }
        else
        {
            _currentSpeed -= Time.fixedDeltaTime * _car.accel;
            if (_currentSpeed < 0)
                _currentSpeed = 0;
        }
    }

    private void HandleStartParking(StartParkingEvent evt)
    {
        _isStart = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void HandleCarExitsMapEvent(CarExitsMapEvent evt)
    {
        _isStart = false;
    }

    public void ParkingSucceed()
    {
        _isSuccess = true;
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }
}
