using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static bool IsDestroyed = false;

    public static T Instance
    {
        get
        {
            if(IsDestroyed)
            {
                Debug.LogError($"{typeof(T).Name} isDesttroy");
                _instance = null;
            }
            if(_instance is null)
            {
                Debug.Log(GameObject.FindObjectOfType<T>());
                _instance = GameObject.FindObjectOfType<T>();
                if(_instance is null)
                {
                    Debug.LogError($"{typeof(T).Name} singletone is not exist");
                }
                else
                {
                    IsDestroyed = false;
                }
            }
            return _instance;
        }
    }
    
    public virtual void Awake()
    {
        if(Instance != this && Instance != null)
        {
            Debug.Log("Instance Has Disployed, Destroy This");
            Destroy(gameObject);
        }
    }
}