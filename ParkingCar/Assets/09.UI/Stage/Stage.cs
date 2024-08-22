using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Stage : MonoBehaviour
{
    private UIDocument _uiDocument;
    private List<Button> _stageBtns;
    private Button _optionBtn;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void Start()
    {
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
    }

    private void HandleOptionButtonClicked()
    {
        Events.OptionButtonClickEvent.timeStop = false;
        Events.OptionButtonClickEvent.open = !Events.OptionButtonClickEvent.open;
        EventManager.BroadCast(Events.OptionButtonClickEvent);
    }

    private void HandleStageButtonClicked(ClickEvent evt, int idx)
    {
        Events.SceneChangeEvent.callBack = () => StageManager.Instance.OpenStage(idx + 1);
        EventManager.BroadCast(Events.SceneChangeEvent);
    }
}