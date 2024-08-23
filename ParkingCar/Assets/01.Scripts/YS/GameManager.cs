using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _successCountNeeded;
    [SerializeField] private float _lifeTime = 7;
    private bool _gameStart;
    private bool _gameClear;

    private float _currentTime = 0;
    
    private void Awake()
    {
        ParkingSpace.SuccessCount = 0;
        EventManager.AddListener<StartParkingEvent>(HandleStartParkingEvent);
    }

    private void HandleStartParkingEvent(StartParkingEvent evt)
    {
        _gameStart = true;
    }

    private void Update()
    {
        if (_gameStart == false && _gameClear) return;
        _currentTime += Time.deltaTime;
        if (_currentTime > _lifeTime)
        {
            _gameStart = false;
            Events.StageClearEvent.isClear = false;
            EventManager.BroadCast(Events.StageClearEvent);
        }
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