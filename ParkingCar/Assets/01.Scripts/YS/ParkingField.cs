using System.Collections.Generic;
using UnityEngine;

public class ParkingField : MonoBehaviour
{
    [SerializeField] private List<Transform> _carParts = new();
    [SerializeField] private float _range;

    private void Update()
    {
        foreach (var carPartTrm in _carParts)
        {
            if (Vector3.Distance(transform.position, carPartTrm.position) > _range)
            {
                EventManager.BroadCast(Events.CarExitsMapEvent);
                gameObject.SetActive(false);
                
                SoundManager.Instance.PlayWithBasePitch(Sound.GameOverSfx);
                Events.StageClearEvent.isClear = false;
                EventManager.BroadCast(Events.StageClearEvent);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
