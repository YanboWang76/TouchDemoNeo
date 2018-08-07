using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class FadeInOutController : MonoBehaviour
{
    public event EventHandler GameOverTextFadeIn;
    public float FadeDuration = 5f;

    private void Start()
    {
        FadeToClear();
    }

    private void FadeToClear()
    {
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, FadeDuration);
    }

    private void FadeToBlack()
    {
        GameOverTextFadeIn(this, new EventArgs());
        SteamVR_Fade.Start(Color.black, FadeDuration);
        StartCoroutine(WaitAndSwitchScene(FadeDuration));
    }

    IEnumerator WaitAndSwitchScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Manager.Instance.GameSceneManager.LoadEntryScene();
    }

    public void EndScene()
    {
        FadeToBlack();
    }
}