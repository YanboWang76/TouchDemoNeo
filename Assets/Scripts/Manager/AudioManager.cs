using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTool.AudioPlayer;

public class AudioManager : MonoBehaviour
{
    private CAudioPlayer cAudioPlayer;
    [SerializeField]
    private AudioClip bgClip;
    private AudioSource bgClipAudioSource;

    [SerializeField]
    private AudioClip collisionClip;
    private AudioSource collisionAudioSource;

    [SerializeField]
    private AudioClip tiktokClip;
    private AudioSource tiktokAudioSource;

    [SerializeField]
    private AudioClip knobClip;
    private AudioSource knobAudioSource;

    [SerializeField]
    private AudioClip bombCaseOpenClip;
    private AudioSource bombCaseOpenAudioSource;

    void Awake()
    {
        cAudioPlayer = CAudioPlayer.GetInstance();
    }

    void Start()
    {
        //PlayBgAudio();
    }
    private void PlayBgAudio()
    {
        if (bgClip == null)
            return;
        bgClipAudioSource = cAudioPlayer.AddAudioSource();
        cAudioPlayer.Play(bgClipAudioSource, bgClip, true);
    }

    public void PlayCollisionAudio()
    {
        if (collisionClip == null)
            return;
        
        collisionAudioSource = cAudioPlayer.AddAudioSource();
        cAudioPlayer.Play(collisionAudioSource, collisionClip, false);
    }

    public void PlayTiktokAudio()
    {
        if (tiktokClip == null)
            return;

        tiktokAudioSource = cAudioPlayer.AddAudioSource();
        cAudioPlayer.Play(tiktokAudioSource, tiktokClip, false);
    }

    public void PlayKnobAudio()
    {
        if (knobClip == null)
            return;

        knobAudioSource = cAudioPlayer.AddAudioSource();
        knobAudioSource.pitch = 1.5f;
        knobAudioSource.volume = 0.6f;
        cAudioPlayer.Play(knobAudioSource, knobClip, false);
    }

    public void PlayBombCaseOpenAudio()
    {
        if (bombCaseOpenClip == null)
            return;

        bombCaseOpenAudioSource = cAudioPlayer.AddAudioSource();
        cAudioPlayer.Play(bombCaseOpenAudioSource, bombCaseOpenClip, false);
    }

    void OnDestory()
    {
        cAudioPlayer.Destory();
        cAudioPlayer = null;
    }
}
