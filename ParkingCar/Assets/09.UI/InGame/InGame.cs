using UnityEngine;
using UnityEngine.UIElements;

public class InGame : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _pausePanel, _stageClearPanel;

    [SerializeField] private Option _option;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    public void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;
        var pauseBTN = root.Q<Button>("PauseButton");
        pauseBTN.clicked += HandlePauseButtonClicked;

        var optionBTN = root.Q<Button>("OptionButton");
        optionBTN.clicked += OnOptionButtonClicked;

        var parkingBTN = root.Q<Button>("ParkingButton");

        var pausePanel = root.Q("PausePanel");
        pausePanel.Q<Button>("ContinueButton").clicked += HandleContinueButtonClicked;
        pausePanel.Q<Button>("ExitButton").clicked += HandleExitButtonClicked;
        pausePanel.Q<Button>("QuitButton").clicked += HandleQuitButtonClicked;

        var stageClearPanel = root.Q("StageClearPanel");
        stageClearPanel.Q<Button>("NextButton").clicked += HandleNextButtonClicked;
        stageClearPanel.Q<Button>("RetryButton").clicked += HandleNextButtonClicked;
        stageClearPanel.Q<Button>("ExitButton").clicked += HandleNextButtonClicked;
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
        Debug.Log("To The Title");
    }
    
    private void HandleQuitButtonClicked()
    {
        Application.Quit();
    }

    private void HandleNextButtonClicked()
    {
        
    }
}
