using System.Collections.Generic;
using DG.Tweening;
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
        _fixedJoint.enableCollision = false;
        _fixedJoint.enablePreprocessing = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Vector3 reflect = Vector3.Reflect(transform.forward, other.contacts[0].normal);
            Quaternion reflectRot = Quaternion.LookRotation(reflect.normalized);
            foreach (var carPart in group)
            {
                carPart.wheel.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.3f).SetEase(Ease.OutCirc);
                carPart.transform.DORotateQuaternion(reflectRot, 0.3f).SetEase(Ease.OutCirc);
            }
        }
    }
}