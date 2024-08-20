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
        _stageClearPanel.Q<Button>("RetryButton").clicked += HandleNextButtonClicked;
        _stageClearPanel.Q<Button>("ExitButton").clicked += HandleNextButtonClicked;
    }

    private void HandleParkingButtonClicked()
    {
        EventManager.BroadCast(Events.StartParkingEvent);
    }

    private void OnOptionButtonClicked()
    {
        Events.OptionButtonClickEvent.timeStop = true;
        Events.OptionButtonClickEvent.open = !Events.OptionButtonClickEvent.open;
        EventManager.BroadCast(Events.OptionButtonClickEvent);
    }

    private void HandlePauseButtonClicked()
    {
        _pausePanel.AddToClassList("open");
        Time.timeScale = 0;
    }

    private void HandleContinueButtonClicked()
    {
        Time.timeScale = 1;
        _pausePanel.RemoveFromClassList("open");
    }
    
    private void HandleExitButtonClicked()
    {
        Events.SceneChangeEvent.callBack = () =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("YS_Stage");
        };
        EventManager.BroadCast(Events.SceneChangeEvent);
    }
    
    private void HandleQuitButtonClicked()
    {
        Application.Quit();
    }

    private void HandleNextButtonClicked()
    {
        
    }
}
