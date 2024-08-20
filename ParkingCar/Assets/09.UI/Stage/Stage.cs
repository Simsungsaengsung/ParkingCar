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

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;
        _stageBtns = root.Query<Button>("StageButton").ToList();
        for (int i = 0; i < _stageBtns.Count; i++)
        {
            _stageBtns[i].RegisterCallback<ClickEvent, int>(HandleStageButtonClicked, i);
            var label = _stageBtns[i].Q<Label>();
            
        }
    }

    private void HandleStageButtonClicked(ClickEvent evt, int idx)
    {
        Events.SceneChangeEvent.callBack = () => StageManager.Instance.OpenStage(idx + 1);
        EventManager.BroadCast(Events.SceneChangeEvent);
    }
}