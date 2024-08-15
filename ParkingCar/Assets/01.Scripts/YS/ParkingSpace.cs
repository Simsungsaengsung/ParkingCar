using System;
using UnityEngine;

public class ParkingSpace : MonoBehaviour
{
    public static int SuccessCount = 0;
    private bool _isSuccess;

    [SerializeField] private int _partsNeeded = 4;
    [SerializeField] private LayerMask _whatIsParkingPart;

    private Collider[] _parts = new Collider[4];

    private ParticleSystem _clearParticle;

    private void Awake()
    {
        _parts = new Collider[_partsNeeded];
        _clearParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (_isSuccess) return;

        Vector3 halfScale = transform.localScale / 2;
        int cnt = Physics.OverlapBoxNonAlloc(transform.position, new Vector3(halfScale.x - 0.1f, halfScale.y, halfScale.z - 0.1f), _parts, transform.rotation, _whatIsParkingPart);
        if (cnt >= _partsNeeded)
        {
            _isSuccess = true;
            SuccessCount += 1;
            _clearParticle.Play();
        }
    }
}