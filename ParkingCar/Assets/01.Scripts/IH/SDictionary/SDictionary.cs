using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SDictionary<K,V> : Dictionary<K,V>, ISerializationCallbackReceiver
{
    public K TempKey;
    public V TempValue;

    public List<K> SerializedKeys = new List<K>();
    public List<V> SerializedValues = new List<V>();

    public void OnAfterDeserialize()
    {
        SyncToSerializeData();
    }

    private void SyncToSerializeData()
    {
        this.Clear(); //내 딕셔너리를 지워버린다.
        if(SerializedKeys != null && SerializedValues != null)
        {
            int maxCount = Mathf.Min(SerializedKeys.Count, SerializedValues.Count);
            for(int i = 0; i < maxCount; i++)
            {
                if (SerializedKeys[i] == null) continue;

                this[SerializedKeys[i]] = SerializedValues[i];
            }
        }else
        {
            SerializedKeys = new List<K>();
            SerializedValues = new List<V>();
        }
    }

    public void OnBeforeSerialize()
    {
        SerializedKeys.Clear();
        SerializedValues.Clear();

        foreach(K key in Keys)
        {
            SerializedKeys.Add(key);
        }

        foreach(V value in Values)
        {
            SerializedValues.Add(value);
        }
    }
}
