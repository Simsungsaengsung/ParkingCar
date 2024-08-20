using System;

[Serializable]
public class GameData
{
    public float sfxValue;
    public float bgmValue;
    
    public SDictionary<int, StageSaveData> stageSaveDatas;
    
    public GameData()
    {
        sfxValue = 1;
        bgmValue = 1;
        stageSaveDatas = new SDictionary<int, StageSaveData>();
        
        StageSaveData firstStageData = new StageSaveData();
        firstStageData.isUnlock = true;
        stageSaveDatas.Add(1, firstStageData);
    }
}
