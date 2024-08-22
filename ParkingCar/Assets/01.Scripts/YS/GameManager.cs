using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _successCountNeeded;
    private bool _gameClear;
    
    private void Awake()
    {
        ParkingSpace.SuccessCount = 0;
    }

    private void Update()
    {
        if (_gameClear) return;
        if (ParkingSpace.SuccessCount >= _successCountNeeded)
        {
            Debug.Log("Clear");
            _gameClear = true;
            Time.timeScale = 0;
            Events.StageClearEvent.isClear = true;
            EventManager.BroadCast(Events.StageClearEvent);
        }
    }
}