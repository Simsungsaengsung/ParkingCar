using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneStart : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("YS_Start");
    }
}
