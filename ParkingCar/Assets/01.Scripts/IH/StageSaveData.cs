using System;
using UnityEngine;

[Serializable]
public class StageSaveData
{
    public bool isClear;
    public bool isUnlock;

    public StageSaveData()
    {
        isClear = false;
        isUnlock = false;
    }
}
