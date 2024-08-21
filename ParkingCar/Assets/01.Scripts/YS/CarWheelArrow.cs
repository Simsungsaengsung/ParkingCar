using System;
using UnityEngine;

public class CarWheelArrow : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsGround;
    private BoxCollider _collider;

    private bool _isChosen;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (_collider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                _isChosen = true;
                var dir = hit.point - transform.position;
                dir.y = 0;
                var lookRot = Quaternion.LookRotation(dir);
                transform.rotation = lookRot;
            }
        }
        else if (_isChosen && Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _whatIsGround))
            {
                var dir = hit.point - transform.position;
                dir.y = 0;
                var lookRot = Quaternion.LookRotation(dir);
                transform.rotation = lookRot;
            }
        }
        else
        {
            _isChosen = false;
        }
    }
}
