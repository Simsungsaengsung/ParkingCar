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
        _currentStageIdx = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Claer");
            StageClear();
        }
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
        SceneManager.LoadScene("YS");
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
        SceneManager.LoadScene("YS_Stage");
    }

    public void StageClear()
    {
        _stageSaveDatas[_currentStageIdx].isClear = true;
        
        int nextIdx = GetNextStageIdx();
        
        if(_stageObjDictionary.ContainsKey(nextIdx))
            _stageSaveDatas[nextIdx].isUnlock = true;
        
        SoundManager.Instance.PlayWithBasePitch(Sound.ClearSfx);
        SaveManager.Instance.SaveData();
    }

    public int StageCheck(int idx)
    {
        if (!_stageSaveDatas.ContainsKey(idx))
        {
            SaveManager.Instance.Init();
            LoadData(SaveManager.Instance.GameData);
            Debug.Log("Load");
        }
        Debug.Log(_stageSaveDatas[1].isUnlock);
        if (_stageSaveDatas[idx].isClear)
            return 1;
        
        if (_stageSaveDatas[idx].isUnlock)
            return 0;
        
        return -1;
    }

    private int GetNextStageIdx()
    {
        int index = _currentStageIdx + 1;
        if (!_stageObjDictionary.ContainsKey(index))
            return _currentStageIdx;
        
        return index;
    }
    
    public void LoadData(GameData gameData)
    {
        if (_stages.Count == _stageObjDictionary.Count)
            return;

        _stages.ForEach(x=> _stageObjDictionary.Add(x.stageIndex, x.stageObj));

        if(_stageSaveDatas.Count == _stages.Count)
            return;
        
        Debug.Log(gameData.stageSaveDatas.Count);
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
        Debug.Log("Asdfadsfasdfasdf");
        _stageSaveDatas[1].isUnlock = true;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.stageSaveDatas = _stageSaveDatas as SDictionary<int, StageSaveData>;
    }
}
