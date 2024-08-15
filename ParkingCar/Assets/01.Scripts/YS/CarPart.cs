using UnityEngine;

public class CarPart : MonoBehaviour
{
    private FixedJoint _fixedJoint;
    [HideInInspector] public Rigidbody rigid;
    
    private CarWheel _wheel;

    public Vector3 Dir => _wheel.transform.forward;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        _wheel = GetComponentInChildren<CarWheel>();
    }

    public void Connect(Rigidbody other)
    {
        _fixedJoint = gameObject.AddComponent<FixedJoint>();
        _fixedJoint.connectedBody = other;
    }
}