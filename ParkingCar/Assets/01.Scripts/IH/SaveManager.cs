using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoSingleton<SaveManager>
{
    [SerializeField] private string _fileName = "";
    private string _directoryPath;
    
    [SerializeField] private GameData _gameData;
    private List<ISaveAble> _saveAbleObjs;

    public override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        base.Awake();
        _directoryPath = Application.persistentDataPath;
        Init();
    }

    public void SaveData()
    {
        foreach (ISaveAble manager in _saveAbleObjs)
        {
            manager.SaveData(ref _gameData);
        }
        
        Save(_gameData);
    }
        
    // 초기화. 가져와서 없으면 만들기
    private void Init()
    {
        _saveAbleObjs = FindAllSaveObjects();
        
        _gameData = Load();
        if (_gameData == null)
            _gameData = new GameData();
        
        foreach (ISaveAble manager in _saveAbleObjs)
        {
            manager.LoadData(_gameData);
        }
    }

    private List<ISaveAble> FindAllSaveObjects()
    {
        return FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveAble>().ToList();
    }

    private void Save(GameData data)
    {
        string fullPath = Path.Combine(_directoryPath, _fileName);
        
        try
        {
            Directory.CreateDirectory(_directoryPath); //존재하면 안만듦
            string dataToStore = JsonUtility.ToJson(data, true);

            //없으면 생성. 있으면 덮어씀
             using (FileStream writeStream = new FileStream(fullPath, FileMode.Create))
             {
                 using (StreamWriter writer = new StreamWriter(writeStream))
                 {
                     writer.Write(dataToStore);
                 }
             }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error on trying to save data to file {fullPath}\n {e.Message}");
        }
        
    }

    private GameData Load()
    {
        string fullPath = Path.Combine(_directoryPath, _fileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = string.Empty;

                using (FileStream readStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(readStream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.LogError($"Error on trying to load data to file {fullPath}\n {e.Message}");
            }
        }

        return loadedData;
    }
}
