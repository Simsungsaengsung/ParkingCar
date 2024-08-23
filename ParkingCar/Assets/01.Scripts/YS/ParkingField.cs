using System.Collections.Generic;
using UnityEngine;

public class ParkingField : MonoBehaviour
{
    [SerializeField] private List<Transform> _carParts = new();
    [SerializeField] private float _range;

    private bool _gameFail;

    private void Update()
    {
        if (_gameFail) return;
        foreach (var carPartTrm in _carParts)
        {
            if (Vector3.Distance(transform.position, carPartTrm.position) > _range)
            {
                _gameFail = true;
                EventManager.BroadCast(Events.CarExitsMapEvent);
                gameObject.SetActive(false);
                
                Events.StageClearEvent.isClear = false;
                EventManager.BroadCast(Events.StageClearEvent);
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
