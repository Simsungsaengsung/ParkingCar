using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Option : MonoBehaviour, ISaveAble
{
    private Slider _sfxVolSlider, _bgmVolSlider;
    private VisualElement _sfxDragger, _bgmDragger;
    private VisualElement _sfxBar, _bgmBar;
    private float _sfxValue, _bgmValue;
    
    private VisualElement _optionPanel;
    private Button _xButton;

    private UIDocument _uiDocument;
    private VisualElement _root;
    private VisualElement _fadePanel;

    [SerializeField] private AudioMixer _audioMixer;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _uiDocument = GetComponent<UIDocument>();
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnEnable()
    {
        EventManager.AddListener<OptionButtonClickEvent>(Open);
        EventManager.AddListener<SceneChangeEvent>(FadeOut);
        
        _root = _uiDocument.rootVisualElement;
        _optionPanel = _root.Q("OptionPanel");
        _xButton = _optionPanel.Q<Button>("ConfirmButton");
        _xButton.clicked += HandleXButtonClicked;
        
        _sfxVolSlider = _root.Q<Slider>("SFXSlider");
        _sfxVolSlider.RegisterCallback<ChangeEvent<float>>(HandleSFXChangeEvent);
        _sfxDragger = _sfxVolSlider.Q("unity-dragger");
        _sfxBar = new VisualElement();
        _sfxBar.AddToClassList("bar");
        _sfxDragger.Add(_sfxBar);
        
        _bgmVolSlider = _root.Q<Slider>("BGMSlider");
        _bgmVolSlider.RegisterCallback<ChangeEvent<float>>(HandleBGMChangeEvent);
        _bgmDragger = _bgmVolSlider.Q("unity-dragger");
        _bgmBar = new VisualElement();
        _bgmBar.AddToClassList("bar");
        _bgmDragger.Add(_bgmBar);

        _fadePanel = _root.Q("FadePanel");
        SaveManager.Instance.Init();
    }

    private void Start()
    {
        LoadData(SaveManager.Instance.ReturnGameData());
    }

    private void HandleBGMChangeEvent(ChangeEvent<float> evt)
    {
        _sfxValue = evt.newValue;
        var volume = _sfxValue == 0 ? 0 : Mathf.Log10(_sfxValue) * 20;
        _audioMixer.SetFloat("BGM", volume);
    }

    private void HandleSFXChangeEvent(ChangeEvent<float> evt)
    {
        _bgmValue = evt.newValue;
        var volume = _bgmValue == 0 ? 0 : Mathf.Log10(_bgmValue) * 20;
        _audioMixer.SetFloat("SFX", volume);
    }

    private void OnDisable()
    {
        SaveManager.Instance.SaveData();
        EventManager.RemoveListener<OptionButtonClickEvent>(Open);
        EventManager.RemoveListener<SceneChangeEvent>(FadeOut);
        Debug.Log("로그를 찍어보자");
    }

    private void HandleXButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        SaveManager.Instance.SaveData();
        _optionPanel.RemoveFromClassList("open");
        Time.timeScale = 1;
        Events.OptionButtonClickEvent.open = false;
    }

    private void Open(OptionButtonClickEvent evt)
    {
        if (evt.open)
        {
            _optionPanel.AddToClassList("open");
            Time.timeScale = 0;
        }
        else
        {
            HandleXButtonClicked();
        }
    }

    private void FadeOut(SceneChangeEvent evt)
    {
        _fadePanel.pickingMode = PickingMode.Position;
        _fadePanel.AddToClassList("out");
        StartCoroutine(DelayCallback(1, evt.callBack));
    }

    private IEnumerator DelayCallback(float delay, Action callback)
    {
        yield return new WaitForSecondsRealtime(delay);
        callback?.Invoke();
    }

    private void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Time.timeScale = 1;
        if (_fadePanel != null && _fadePanel.ClassListContains("out"))
        {
            _fadePanel.pickingMode = PickingMode.Ignore;
            _fadePanel.RemoveFromClassList("out");
        }
    }

    public void LoadData(GameData gameData)
    {
        Debug.Log(gameData.bgmValue);
        
        _sfxVolSlider.value = gameData.sfxValue;
        _sfxValue = gameData.sfxValue;
        var volume1 = _sfxValue == 0 ? 0 : Mathf.Log10(_sfxValue) * 20;
        _audioMixer.SetFloat("SFX", volume1);
        
        _bgmVolSlider.value = gameData.bgmValue;
        _bgmValue = gameData.bgmValue;
        var volume2 = _bgmValue == 0 ? 0 : Mathf.Log10(_bgmValue) * 20;
        Debug.Log(volume2);
        _audioMixer.SetFloat("BGM", volume2);
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.sfxValue = _sfxValue;
        gameData.bgmValue = _bgmValue;
    }
}