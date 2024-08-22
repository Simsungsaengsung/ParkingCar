using System;
using UnityEngine;

[Serializable]
public struct Group
{
    public CarPart left;
    public CarPart right;
}

public class Car : MonoBehaviour
{
    [field:SerializeField] public float speed { get; private set; }
    [field:SerializeField] public float accel { get; private set; }
    [field:SerializeField] public float deAccel { get; private set; }
    
    [SerializeField] private Group _top;
    [SerializeField] private Group _bottom;
    [SerializeField] private Group _left;
    [SerializeField] private Group _right;

    private bool _topLeft, _topRight, _bottomLeft, _bottomRight;

    private void Awake()
    {
        _top.left.SetUp();
        _top.right.SetUp();
        _bottom.left.SetUp();
        _bottom.right.SetUp();
        EventManager.AddListener<StartParkingEvent>(HandleStartParking);
    }

    private void HandleStartParking(StartParkingEvent evt)
    {
        _topLeft =     Vector3.Dot(_top.left.Dir, transform.forward + -transform.right) is > 1f and < 1.414214f;
        _topRight =    Vector3.Dot(_top.right.Dir, transform.forward + transform.right) is > 1f and < 1.414214f;
        _bottomLeft =  Vector3.Dot(_bottom.right.Dir, -transform.forward + -transform.right) is > 1f and < 1.414214f;
        _bottomRight = Vector3.Dot(_bottom.left.Dir, -transform.forward + transform.right) is > 1f and < 1.414214f;
        
        CheckDivision();
    }

    private void CheckDivision()
    {
        if (_topLeft && _topRight && _bottomLeft && _bottomRight)
        {
            Debug.Log("4 Side Divide");
        }
        else
        {
            if (CheckVerticalDivision() == false && CheckHorizontalDivision() == false)
            {
                Debug.Log("Not Divide");
                ConnectAll();
            }
        }
    }

    private bool CheckVerticalDivision()
    {
        Vector3 moveDirLeft = _bottom.right.Dir + _top.left.Dir;
        float crossY1 = Vector3.Cross(transform.right, moveDirLeft).y;
        var leftRot = crossY1 < 0 
            ? Quaternion.Euler(0, -_bottom.right.transform.localRotation.eulerAngles.y, 0)
            : Quaternion.Euler(0, -_top.left.transform.localRotation.eulerAngles.y, 0);
        
        moveDirLeft = leftRot * moveDirLeft;
        var crossYLeft = Vector3.Cross(transform.forward, moveDirLeft).y;

        Vector3 moveDirRight = _bottom.left.Dir + _top.right.Dir;
        float crossY2 = Vector3.Cross(transform.right, moveDirRight).y;
        var rightRot = crossY2 < 0
            ? Quaternion.Euler(0, -_bottom.left.transform.localRotation.eulerAngles.y, 0)
            : Quaternion.Euler(0, -_top.right.transform.localRotation.eulerAngles.y, 0);
        
        moveDirRight = rightRot * moveDirRight;
        var crossYRight = Vector3.Cross(transform.forward, moveDirRight).y;

        Debug.Log(crossYLeft);
        Debug.Log(crossYRight);
        if (crossYLeft < -0.1 && crossYRight > 0.1)
        {
            Debug.Log("Vertical Divide");
            CheckGroupDivision(_left);
            CheckGroupDivision(_right);
        }
        //else if (crossYLeft > 0.1 && crossYRight < -0.1)
        //{
        //    Debug.Log("Vertical Compress");
        //    ConnectAll();
        //} 
        else return false;

        return true;
    }

    private bool CheckHorizontalDivision()
    {
        Vector3 moveDirLeft = _bottom.left.Dir + _bottom.right.Dir;
        float crossY1 = Vector3.Cross(transform.up, moveDirLeft).y;
        var leftRot = crossY1 < 0 
            ? Quaternion.Euler(0, -_bottom.left.transform.localRotation.eulerAngles.y, 0)
            : Quaternion.Euler(0, -_bottom.right.transform.localRotation.eulerAngles.y, 0);
        
        moveDirLeft = leftRot * moveDirLeft;
        var crossYLeft = Vector3.Cross(-transform.right, moveDirLeft).y;

        Vector3 moveDirRight = _top.left.Dir + _top.right.Dir;
        float crossY2 = Vector3.Cross(transform.up, moveDirRight).y;
        var rightRot = crossY2 < 0
            ? Quaternion.Euler(0, -_top.right.transform.localRotation.eulerAngles.y, 0)
            : Quaternion.Euler(0, -_top.left.transform.localRotation.eulerAngles.y, 0);
        
        moveDirRight = rightRot * moveDirRight;
        var crossYRight = Vector3.Cross(-transform.right, moveDirRight).y;

        Debug.Log(crossYLeft);
        Debug.Log(crossYRight);
        if (crossYLeft < -0.1 && crossYRight > 0.1)
        {
            Debug.Log("Horizontal Divide");
            CheckGroupDivision(_top);
            CheckGroupDivision(_bottom);
        }
        //else if (crossYLeft > 0.1 && crossYRight < -0.1)
        //{
        //    Debug.Log("Vertical Compress");
        //    ConnectAll();
        //} 
        else return false;

        return true;
    }
    
    private void CheckGroupDivision(Group group)
    {
        if (Vector3.Dot(group.left.Dir, group.right.Dir) is > 0.1f and < 1f)
        {
            // Connect group's Left and Right
            Connect(group);
        }
    }

    private void Connect(Group group)
    {
        group.left.Connect(group.right);
        group.right.Connect(group.left);
    }

    private void ConnectAll()
    {
        Connect(_top);
        Connect(_bottom);
        Connect(_left);
        Connect(_right);
        _top.left.Connect(_bottom.left);
        _bottom.left.Connect(_top.left);
        
        _top.right.Connect(_bottom.right);
        _bottom.right.Connect(_top.right);
    }
}