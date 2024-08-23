using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Stage : MonoBehaviour
{
    private UIDocument _uiDocument;
    private List<Button> _stageBtns;
    private Button _optionBtn;
    private Button _homeBtn;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.StageSelectBgm);
        var root = _uiDocument.rootVisualElement;
        _stageBtns = root.Query<Button>("StageButton").ToList();
        for (int i = 0; i < _stageBtns.Count; i++)
        {
            var label = _stageBtns[i].Q<Label>();
            int check = StageManager.Instance.StageCheck(i + 1);
            if (check == -1)
            {
                label.AddToClassList("lock");
                continue;
            }
            
            if (check == 1)
            {
                label.text = "클리어!";
                label.AddToClassList("clear");
            }
            
            _stageBtns[i].RegisterCallback<ClickEvent, int>(HandleStageButtonClicked, i);
        }

        _optionBtn = root.Q<Button>("OptionButton");
        _optionBtn.clicked += HandleOptionButtonClicked;

        _homeBtn = root.Q<Button>("ExitButton");
        _homeBtn.clicked += HandleHomeButtonClicked;
    }

    private void HandleHomeButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        Events.SceneChangeEvent.callBack = () => SceneManager.LoadScene("YS_Start");
        EventManager.BroadCast(Events.SceneChangeEvent);
    }

    private void HandleOptionButtonClicked()
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        Events.OptionButtonClickEvent.timeStop = false;
        Events.OptionButtonClickEvent.open = !Events.OptionButtonClickEvent.open;
        EventManager.BroadCast(Events.OptionButtonClickEvent);
    }

    private void HandleStageButtonClicked(ClickEvent evt, int idx)
    {
        SoundManager.Instance.PlayWithBasePitch(Sound.ButtonClickSfx);
        Events.SceneChangeEvent.callBack = () => StageManager.Instance.OpenStage(idx + 1);
        EventManager.BroadCast(Events.SceneChangeEvent);
    }
}