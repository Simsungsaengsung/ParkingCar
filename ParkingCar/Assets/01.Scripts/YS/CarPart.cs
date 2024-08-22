using System;
using System.Collections.Generic;
using UnityEngine;

public class CarPart : MonoBehaviour
{
    private FixedJoint _fixedJoint;
    [HideInInspector] public Rigidbody rigid;
    public List<CarPart> group;
    
    private CarWheel wheel;

    public Vector3 Dir => wheel.transform.forward;

    public void SetUp()
    {
        rigid = GetComponent<Rigidbody>();
        wheel = GetComponentInChildren<CarWheel>();
        wheel.SetUp();
        
        group = new();
        group.Add(this);
    }

    public void Connect(CarPart otherPart)
    {
        group.Add(otherPart);
        _fixedJoint = gameObject.AddComponent<FixedJoint>();
        _fixedJoint.connectedBody = otherPart.rigid;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Vector3 reflect = Vector3.Reflect(transform.forward, other.contacts[0].normal);
            foreach (var carPart in group)
            {
                carPart.rigid.velocity = Vector3.zero;
                carPart.transform.forward = reflect;
                carPart.wheel.transform.localRotation = Quaternion.identity;
            }
        }
    }
}