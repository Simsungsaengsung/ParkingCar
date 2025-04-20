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

    private bool _topLeft, _topRight, _bottomLeft, _bottomRight;

    private void Awake()
    {
        _top.left.SetUp();
        _top.right.SetUp();
        _bottom.left.SetUp();
        _bottom.right.SetUp();
        EventManager.AddListener<StartParkingEvent>(HandleStartParking);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<StartParkingEvent>(HandleStartParking);
    }

    private void HandleStartParking(StartParkingEvent evt)
    {
        _topLeft =     Vector3.Dot(_top.left.Dir, transform.forward - transform.right) is > 1f and < 1.414214f;
        _topRight =    Vector3.Dot(_top.right.Dir, transform.forward + transform.right) is > 1f and < 1.414214f;
        _bottomLeft =  Vector3.Dot(_bottom.right.Dir, -transform.forward - transform.right) is > 1f and < 1.414214f;
        _bottomRight = Vector3.Dot(_bottom.left.Dir, -transform.forward + transform.right) is > 1f and < 1.414214f;
        
        CheckDivision();
    }

    private void CheckDivision()
    {
        if (_topLeft && _topRight && _bottomLeft && _bottomRight)
        {
            
        }
        else
        {
            if (CheckVerticalDivision() == false && CheckHorizontalDivision() == false)
            {
                ConnectAll();
            }
        }
    }

    private bool CheckVerticalDivision()
    {
        Vector3 moveDirLeft = _bottom.left.Dir + _top.left.Dir;
        float crossY1 = Vector3.Cross(transform.right, moveDirLeft).y;
        var leftRot = crossY1 < 0
            ? Quaternion.Euler(0, -_bottom.left.transform.localRotation.eulerAngles.y, 0)
            : Quaternion.Euler(0, -_top.left.transform.localRotation.eulerAngles.y, 0);
        
        moveDirLeft = leftRot * moveDirLeft;
        var crossYLeft = Vector3.Cross(transform.forward, moveDirLeft).y;

        Vector3 moveDirRight = _bottom.right.Dir + _top.right.Dir;
        float crossY2 = Vector3.Cross(transform.right, moveDirRight).y;
        var rightRot = crossY2 < 0
            ? Quaternion.Euler(0, -_bottom.right.transform.localRotation.eulerAngles.y, 0)
            : Quaternion.Euler(0, -_top.right.transform.localRotation.eulerAngles.y, 0);
        
        moveDirRight = rightRot * moveDirRight;
        var crossYRight = Vector3.Cross(transform.forward, moveDirRight).y;

        if (crossYLeft < -0.1 && crossYRight > 0.1)
        {
            CheckGroupDivision(_bottom.left, _top.left);
            CheckGroupDivision(_top.right, _bottom.right);
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
            ? Quaternion.Euler(0, -_bottom.right.transform.localRotation.eulerAngles.y, 0)
            : Quaternion.Euler(0, -_bottom.left.transform.localRotation.eulerAngles.y, 0);
        
        moveDirLeft = leftRot * moveDirLeft;
        var crossYLeft = Vector3.Cross(-transform.right, moveDirLeft).y;

        Vector3 moveDirRight = _top.left.Dir + _top.right.Dir;
        float crossY2 = Vector3.Cross(transform.up, moveDirRight).y;
        var rightRot = crossY2 < 0
            ? Quaternion.Euler(0, -_top.right.transform.localRotation.eulerAngles.y, 0)
            : Quaternion.Euler(0, -_top.left.transform.localRotation.eulerAngles.y, 0);
        
        moveDirRight = rightRot * moveDirRight;
        var crossYRight = Vector3.Cross(-transform.right, moveDirRight).y;

        if (crossYLeft < -0.1 && crossYRight > 0.1)
        {
            CheckGroupDivision(_top.left, _top.right);
            CheckGroupDivision(_bottom.right, _bottom.left);
        }
        //else if (crossYLeft > 0.1 && crossYRight < -0.1)
        //{
        //    Debug.Log("Vertical Compress");
        //    ConnectAll();
        //} 
        else return false;

        return true;
    }
    
    private void CheckGroupDivision(CarPart left, CarPart right)
    {
        if (Vector3.Dot(left.Dir, right.Dir) is > 0.1f and < 1f)
        {
            // Connect group's Left and Right
            Connect(left, right);
        }
    }

    private void Connect(CarPart left, CarPart right)
    {
        left.Connect(right);
        right.Connect(left);
    }

    private void ConnectAll()
    {
        Connect(_top.left, _top.right);
        Connect(_bottom.right, _bottom.left);
        Connect(_bottom.left, _top.left);
        Connect(_top.right, _bottom.right);
        _top.left.Connect(_bottom.left);
        _bottom.left.Connect(_top.left);
        
        _top.right.Connect(_bottom.right);
        _bottom.right.Connect(_top.right);
    }
}