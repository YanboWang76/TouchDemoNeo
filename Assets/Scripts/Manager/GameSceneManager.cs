using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager :MonoBehaviour {

    public void LoadEntryScene(){
        SceneManager.LoadScene(SceneModel.EntryScene);
    }

    public void LoadBombExpertScene()
    {
        SceneManager.LoadScene(SceneModel.TouchDemoNeo);
    }
    
}
