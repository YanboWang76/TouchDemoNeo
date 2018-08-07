using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if UNITY_5
using UnityEngine.SceneManagement;
#endif

public class SceneLoading : MonoBehaviour {

    public Slider slider;
    static string targetLevelName;
    AsyncOperation asyncOperation;

    void Start() {
#if UNITY_5
        asyncOperation = SceneManager.LoadSceneAsync(targetLevelName);
#else
        asyncOperation = Application.LoadLevelAsync(targetLevelName);
#endif
    }

    void FixedUpdate() {
        slider.value = asyncOperation.progress;
    }

    public static void LoadScene(string levelName) {
        targetLevelName = levelName;
#if UNITY_5
        SceneManager.LoadScene("Loading");
#else
        Application.LoadLevel("Loading");
#endif
    }

}
