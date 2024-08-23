using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InGame : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _pausePanel, _stageClearPanel;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        EventManager.AddListener<StageFinishEvent>(HandleStageFinishEvent);
    }

    public void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;
        var pauseBtn = root.Q<Button>("PauseButton");
        pauseBtn.clicked += HandlePauseButtonClicked;

        var optionBtn = root.Q<Button>("OptionButton");
        optionBtn.clicked += OnOptionButtonClicked;

        var parkingBtn = root.Q<Button>("ParkingButton");
        parkingBtn.clicked += HandleParkingButtonClicked;

        _pausePanel = root.Q("PausePanel");
        _pausePanel.Q<Button>("ContinueButton").clicked += HandleContinueButtonClicked;
        _pausePanel.Q<Button>("ExitButton").clicked += HandleExitButtonClicked;
        _pausePanel.Q<Button>("QuitButton").clicked += HandleQuitButtonClicked;

        _stageClearPanel = root.Q("StageClearPanel");
        _stageClearPanel.Q<Button>("NextButton").clicked += HandleNextButtonClicked;
        _stageClearPanel.Q<Button>("RetryButton").clicked += HandleRetryButtonClicked;
        _stageClearPanel.Q<Button>("ExitButton").clicked += HandleExitButtonClicked;
    }

    private void Start()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.InGameBgm);
    }

    private void HandleParkingButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        EventManager.BroadCast(Events.StartParkingEvent);
    }

    private void OnOptionButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        Events.OptionButtonClickEvent.timeStop = true;
        Events.OptionButtonClickEvent.open = !Events.OptionButtonClickEvent.open;
        EventManager.BroadCast(Events.OptionButtonClickEvent);
    }

    private void HandlePauseButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        _pausePanel.AddToClassList("open");
        Time.timeScale = 0;
    }

    private void HandleContinueButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        Time.timeScale = 1;
        _pausePanel.RemoveFromClassList("open");
    }
    
    private void HandleExitButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        Events.SceneChangeEvent.callBack = () =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("YS_Stage");
        };
        EventManager.BroadCast(Events.SceneChangeEvent);
    }
    
    private void HandleQuitButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        Application.Quit();
    }

    private void HandleNextButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        StageManager.Instance.NextStage();
    }

    private void HandleRetryButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        StageManager.Instance.RetryStage();
    }

    private void HandleStageFinishEvent(StageFinishEvent evt)
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        if (evt.isClear)
            _stageClearPanel.AddToClassList("open");
        else
            DOVirtual.DelayedCall(3, () => StageManager.Instance.RetryStage());
    }
}
