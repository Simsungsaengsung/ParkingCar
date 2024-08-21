using System;
using Cinemachine.Utility;
using UnityEngine;

[Serializable]
public struct Group
{
    public CarPart left;
    public CarPart right;

    [HideInInspector] public Vector3 cross;
    [HideInInspector] public float dot;
}

public class Car : MonoBehaviour
{
    [field:SerializeField] public float speed { get; private set; }
    [field:SerializeField] public float accel { get; private set; }
    
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
        _top.cross = Vector3.Cross(_top.left.Dir, _top.right.Dir);
        _top.dot = Vector3.Dot(_top.left.Dir, _top.right.Dir);
        
        _bottom.cross = Vector3.Cross(_bottom.left.Dir, _bottom.right.Dir);
        _bottom.dot = Vector3.Dot(_bottom.left.Dir, _bottom.right.Dir);
        
        _left.cross = Vector3.Cross(_left.left.Dir, _left.right.Dir);
        _left.dot = Vector3.Dot(_left.left.Dir, _left.right.Dir);
        
        _right.cross = Vector3.Cross(_right.left.Dir, _right.right.Dir);
        _right.dot = Vector3.Dot(_right.left.Dir, _right.right.Dir);
        
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
            Vector3 dir = _top.left.Dir + _top.right.Dir + _bottom.left.Dir + _bottom.right.Dir;
            Vector3 absDir = new Vector3(Mathf.Abs(dir.x), 0, Mathf.Abs(dir.z));

            if (absDir.x < absDir.z)
            {
                if (dir.z > 0)
                    CheckVerticalDivision();
                else
                    CheckVerticalDivision();
            }
            else if (absDir.x > absDir.z)
            {
                if (dir.x > 0)
                    CheckHorizontalDivision();
                else
                    CheckHorizontalDivision();
            }
            else if (Equals(absDir.x, absDir.z))
            {
                if (CheckVerticalDivision() == false)
                    CheckHorizontalDivision();
            }
        }
    }

    private bool CheckVerticalDivision()
    {
        if (_top.dot + _bottom.dot < 1.94f)
        {
            if (_left.cross.y < -0.1f && _right.cross.y < -0.1f)
            {
                Debug.Log("Vertical Divide");
                CheckGroupDivision(_left);
                CheckGroupDivision(_right);
            }
            else if (_left.cross.y > 0.1f && _right.cross.y > 0.1f)
            {
                Debug.Log("Vertical Compress");
                ConnectAll();
            }
            else
            {
                Debug.Log("Not Divide 1");
                ConnectAll();
                return false;
            }

            return true;
        }
        
        Debug.Log("Not Divide 2");
        ConnectAll();
        return false;
    }

    private bool CheckHorizontalDivision()
    {
        if (_left.dot + _right.dot < 1.94f)
        {
            if (_top.cross.y < -0.1f && _bottom.cross.y < -0.1f)
            {
                Debug.Log("Horizontal Divide");
                CheckGroupDivision(_top);
                CheckGroupDivision(_bottom);
            }
            else if (_top.cross.y > 0.1f && _bottom.cross.y > 0.1f)
            {
                Debug.Log("Horizontal Compress");
                ConnectAll();
            }
            else
            {
                Debug.Log("Not Divide");
                ConnectAll();
                return false;
            }

            return true;
        }
        
        Debug.Log("Not Divide");
        ConnectAll();
        return false;
    }
    
    private void CheckGroupDivision(Group group)
    {
        if (group.dot is > -1 and < 0f && group.cross.y > 0)
        {
            
        }
        else
        {
            // Connect group's Left and Right
            Connect(group);
        }
    }

    private void Connect(Group group)
    {
        group.left.Connect(group.right.rigid);
        group.right.Connect(group.left.rigid);
    }

    private void ConnectAll()
    {
        Connect(_top);
        Connect(_bottom);
        Connect(_left);
        Connect(_right);
    }
}