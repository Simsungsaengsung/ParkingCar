using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct StageIdxAndObj
{
    public int stageIndex;
    public GameObject stageObj;
}
public class StageManager : MonoSingleton<StageManager>, ISaveAble
{
    [SerializeField] private List<StageIdxAndObj> _stages;
    
    private Dictionary<int, GameObject> _stageObjDictionary = new ();
    private Dictionary<int, StageSaveData> _stageSaveDatas = new Dictionary<int, StageSaveData>();
    public Dictionary<int, StageSaveData> StageSaveDatas => _stageSaveDatas;

    private int _currentStageIdx;
    
    public override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        base.Awake();
        _currentStageIdx = 11;
        _stages.ForEach(x=> _stageObjDictionary.Add(x.stageIndex, x.stageObj));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
            StageClear();
    }
    
    private void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log(_currentStageIdx);
        Instantiate(_stageObjDictionary[_currentStageIdx], Vector3.zero, Quaternion.identity);
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    public void OpenStage(int index)
    {
        _currentStageIdx = index;
        if(!_stageSaveDatas[_currentStageIdx].isUnlock)
            return;
        
        SceneManager.sceneLoaded += HandleSceneLoaded;
        SceneManager.LoadScene("Stage");
    }

    public void NextStage()
    {
        int nextIdx = GetNextStageIdx();
        if (_stageObjDictionary.ContainsKey(nextIdx))
        {
            OpenStage(nextIdx);
            _currentStageIdx = nextIdx;
        }
        else
        {
            Debug.LogError("Over Stage");
        }
    }

    public void RetryStage()
    {
        OpenStage(_currentStageIdx);
    }
    
    public void ReturnMenu()
    {
        SceneManager.LoadScene("Chapter");
    }

    public void StageClear()
    {
        UIManager_inhae.Instance.GameClear();
        _stageSaveDatas[_currentStageIdx].isClear = true;
        
        int nextIdx = GetNextStageIdx();
        
        if(_stageObjDictionary.ContainsKey(nextIdx))
            _stageSaveDatas[nextIdx].isUnlock = true;
        
        SaveManager.Instance.SaveData();
    }

    private int GetNextStageIdx()
    {
        int index = _currentStageIdx + 1;
        if (!_stageObjDictionary.ContainsKey(index))
        {
            int chapterIdx = index / 10 + 1;
            int stageIdx = 1;

            index = chapterIdx * 10 + stageIdx;
        }
        
        return index;
    }
    
    public void LoadData(GameData gameData)
    {
        _stageSaveDatas = gameData.stageSaveDatas;
        if (_stageSaveDatas.Count != _stageObjDictionary.Count)
        {
            foreach (var stageIdx in _stageObjDictionary)
            {
                int idx = stageIdx.Key;

                if (!_stageSaveDatas.ContainsKey(idx))
                {
                    _stageSaveDatas[idx] = new StageSaveData();
                }
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.stageSaveDatas = _stageSaveDatas as SDictionary<int, StageSaveData>;
    }
}
