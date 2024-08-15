using System;
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
    [SerializeField] private Group _top;
    [SerializeField] private Group _bottom;
    [SerializeField] private Group _left;
    [SerializeField] private Group _right;

    private void Awake()
    {
        CheckDivision();
    }

    private void CheckDivision()
    {
        _top.cross = Vector3.Cross(_top.left.Dir, _top.right.Dir);
        _top.dot = Vector3.Dot(_top.left.Dir, _top.right.Dir);
        
        _bottom.cross = Vector3.Cross(_bottom.left.Dir, _bottom.right.Dir);
        _bottom.dot = Vector3.Dot(_bottom.left.Dir, _bottom.right.Dir);
        
        _left.cross = Vector3.Cross(_left.left.Dir, _left.right.Dir);
        _left.dot = Vector3.Dot(_left.left.Dir, _left.right.Dir);
        
        _right.cross = Vector3.Cross(_right.left.Dir, _right.right.Dir);
        _right.dot = Vector3.Dot(_right.left.Dir, _right.right.Dir);

        bool topLeft =     Vector3.Dot(_top.left.Dir, Vector3.up + Vector3.left) is > 0f and < 0.7071f;
        bool topRight =    Vector3.Dot(_top.right.Dir, Vector3.up + Vector3.right) is > 0 and < 0.7071f;
        bool bottomLeft =  Vector3.Dot(_bottom.left.Dir, Vector3.down + Vector3.left) is > 0 and < 0.7071f;
        bool bottomRight = Vector3.Dot(_bottom.right.Dir, Vector3.down + Vector3.right) is > 0 and < 0.7071f;
        
        if (topLeft && topRight && bottomLeft && bottomRight)
        {
            // do nothing
            Debug.Log("do nothing");
        }
        else
        {
            CheckVerticalDivision();
        }
    }

    private void CheckVerticalDivision()
    {
        if (_top.dot + _bottom.dot < 1.94f)
        {
            if ((_top.cross + _bottom.cross).y >= 0
                || Vector3.Dot((_left.left.Dir + _left.right.Dir).normalized,
                    (_right.left.Dir + _right.right.Dir).normalized) is > -1 and < 0)
            {
                CheckGroupDivision(_left);
                CheckGroupDivision(_right);
            }
            else
            {
                Debug.Log("compression");
                Connect(_top);
                Connect(_bottom);
                Connect(_left);
                Connect(_right);
            }
        }
        else
        {
            CheckHorizontalDivision();
        }
    }

    private void CheckHorizontalDivision()
    {
        if (_left.dot + _right.dot < 1.94f)
        {
            if ((_left.cross + _right.cross).y >= 0
                || Vector3.Dot((_top.left.Dir + _top.right.Dir).normalized,
                    (_bottom.left.Dir + _bottom.right.Dir).normalized) is > -1 and < 0)
            {
                Debug.Log("horizontal divided");
                CheckGroupDivision(_top);
                CheckGroupDivision(_bottom);
            }
            else
            {
                Debug.Log("compression");
                Connect(_top);
                Connect(_bottom);
                Connect(_left);
                Connect(_right);
            }
        }
        else
        {
            Connect(_top);
            Connect(_bottom);
            Connect(_left);
            Connect(_right);
        }
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
}