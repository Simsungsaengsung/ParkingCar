using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Stage : MonoBehaviour
{
    private UIDocument _uiDocument;
    private List<Button> _stageBtns;

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
            _stageBtns[i].RegisterCallback<ClickEvent, int>(HandleStageButtonClicked, i);
            var label = _stageBtns[i].Q<Label>();
            int check = StageManager.Instance.StageCheck(i + 1);
            if (check == -1)
            {
                label.AddToClassList("lock");
            }
            else if (check == 1)
            {
                label.text = "클리어!";
                label.AddToClassList("clear");
            }
        }
    }

    private void HandleStageButtonClicked(ClickEvent evt, int idx)
    {
        Events.SceneChangeEvent.callBack = () => StageManager.Instance.OpenStage(idx + 1);
        EventManager.BroadCast(Events.SceneChangeEvent);
    }
}