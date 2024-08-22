using System;
using UnityEngine;

public class CarPart : MonoBehaviour
{
    private FixedJoint _fixedJoint;
    [HideInInspector] public Rigidbody rigid;
    
    private CarWheel _wheel;

    public Vector3 Dir => _wheel.transform.forward;

    public void SetUp()
    {
        rigid = GetComponent<Rigidbody>();
        _wheel = GetComponentInChildren<CarWheel>();
        _wheel.SetUp();
    }

    public void Connect(Rigidbody other)
    {
        _fixedJoint = gameObject.AddComponent<FixedJoint>();
        _fixedJoint.connectedBody = other;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            
        }
    }
}