using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartSceneUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startBtn, _optionBtn, _quitBtn;

    private void Awake()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.IntroBgm);
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;

        _startBtn = root.Q<Button>("StartButton");
        _startBtn.clicked += HandleStartButtonClicked;
        _startBtn.clicked += HandleSoundButtonClick;

        _optionBtn = root.Q<Button>("OptionButton");
        _optionBtn.clicked += HandleOptionButtonClicked;
        _optionBtn.clicked += HandleSoundButtonClick;

        _quitBtn = root.Q<Button>("QuitButton");
        _quitBtn.clicked += HandleQuitButtonClicked;
        _quitBtn.clicked += HandleSoundButtonClick;
    }

    private void HandleSoundButtonClick()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
    }

    private void HandleStartButtonClicked()
    {
        Events.SceneChangeEvent.callBack = () => SceneManager.LoadScene("YS_Stage");
        EventManager.BroadCast(Events.SceneChangeEvent);
    }

    private void HandleOptionButtonClicked()
    {
        Events.OptionButtonClickEvent.timeStop = false;
        Events.OptionButtonClickEvent.open = !Events.OptionButtonClickEvent.open;
        EventManager.BroadCast(Events.OptionButtonClickEvent);
    }

    private void HandleQuitButtonClicked()
    {
        Application.Quit();
    }
}
