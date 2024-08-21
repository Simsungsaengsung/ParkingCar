using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Option : MonoBehaviour, ISaveAble
{
    private Slider _sfxVolSlider, _bgmVolSlider;
    private VisualElement _sfxDragger, _bgmDragger;
    private VisualElement _sfxBar, _bgmBar;
    private float _sfx, _bgm;
    
    private VisualElement _optionPanel;
    private Button _xButton;

    private UIDocument _uiDocument;
    private VisualElement _root;
    private VisualElement _fadePanel;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _uiDocument = GetComponent<UIDocument>();
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    public void OnEnable()
    {
        EventManager.AddListener<OptionButtonClickEvent>(Open);
        EventManager.AddListener<SceneChangeEvent>(FadeOut);
        
        _root = _uiDocument.rootVisualElement;
        _optionPanel = _root.Q("OptionPanel");
        _xButton = _optionPanel.Q<Button>("ConfirmButton");
        _xButton.clicked += HandleXButtonClicked;
        
        _sfxVolSlider = _root.Q<Slider>("SFXSlider");
        _sfxDragger = _sfxVolSlider.Q("unity-dragger");
        _sfxBar = new VisualElement();
        _sfxBar.AddToClassList("bar");
        _sfxDragger.Add(_sfxBar);
        
        _bgmVolSlider = _root.Q<Slider>("BGMSlider");
        _bgmDragger = _bgmVolSlider.Q("unity-dragger");
        _bgmBar = new VisualElement();
        _bgmBar.AddToClassList("bar");
        _bgmDragger.Add(_bgmBar);

        _fadePanel = _root.Q("FadePanel");
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<OptionButtonClickEvent>(Open);
        EventManager.RemoveListener<SceneChangeEvent>(FadeOut);
    }

    private void HandleXButtonClicked()
    {
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
        if (_fadePanel != null && _fadePanel.ClassListContains("out"))
        {
            _fadePanel.pickingMode = PickingMode.Ignore;
            _fadePanel.RemoveFromClassList("out");
        }
    }

    public void LoadData(GameData gameData)
    {
        
    }

    public void SaveData(ref GameData gameData)
    {
        
    }
}