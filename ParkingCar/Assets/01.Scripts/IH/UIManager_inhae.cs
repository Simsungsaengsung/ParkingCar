using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager_inhae : MonoSingleton<UIManager_inhae>
{
    [SerializeField] private Transform _canvas;

    private Transform _stagePanelTrm;
    private Transform _clearPanelTrm;

    private Dictionary<int, Image> _stageButtonDictionary = new ();

    public override void Awake()
    {
        base.Awake();
        _stagePanelTrm = _canvas.Find("StagePanel");
        _clearPanelTrm = _canvas.Find("ClearPanel");
        
        foreach (var button in _stagePanelTrm.GetComponentsInChildren<Button>(true))
        {
            string name = button.name;
            name = name.Replace("-", "");
            _stageButtonDictionary.Add(int.Parse(name), button.GetComponent<Image>());
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Dictionary<int, StageSaveData> stageSaveDatas = StageManager.Instance.StageSaveDatas;
        foreach (var stage in stageSaveDatas)
        {
            int key = stage.Key;
            StageSaveData value = stage.Value;
            
            if (!_stageButtonDictionary.ContainsKey(key))
            {
                Debug.LogError("No Contain Button");
                return;
            }

            if (value.isClear)
                _stageButtonDictionary[key].color = Color.green;
            else if(value.isUnlock)
                _stageButtonDictionary[key].color = Color.white;
            else
                _stageButtonDictionary[key].color = Color.gray;
        }
    }
    
    public void NextStage()
    {
        StageManager.Instance.NextStage();
    }

    public void RetryStage()
    {
        StageManager.Instance.RetryStage();
    }
    
    public void ReturnMenu()
    {
        StageManager.Instance.ReturnMenu();
    }

    public void OpenStage(int idx)
    {
        StageManager.Instance.OpenStage(idx);
    }
    public void GameClear()
    {
        _clearPanelTrm.gameObject.SetActive(true);
    }
}
