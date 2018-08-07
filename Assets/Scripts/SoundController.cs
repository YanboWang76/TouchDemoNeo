using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class SoundController : MonoBehaviour
{
    public AudioSource TeleportSoundSource;
    public AudioSource WelcomeSoundSource;
    public AudioSource WellDoneNextStageAudioSource;
    public AudioSource CongratsFreePlayModeAudioSource;
    public AudioSource PowerUpSoundSource;
    public AudioSource PowerDownSoundSource;
    public List<AudioClip> BoltDropAudioList = new List<AudioClip> { null };
    public List<AudioClip> SlideInWoodAudioList = new List<AudioClip> { null };
    public List<AudioClip> SlideInMetalAudioList = new List<AudioClip> { null };
    public List<AudioClip> WoodDropAudioList = new List<AudioClip> { null };
    public List<AudioClip> MetalDropAudioList = new List<AudioClip> {null}; 
    public List<AudioClip> RubberDropAudioList = new List<AudioClip> {null}; 
    public AudioSource PointAwardingSoundSource;
    public AudioSource BonusPointAwardingSoundSource;

    public static SoundController Instance { get; private set; }

    private GameObject _globalAudioSourceObj;
    private AudioSource _globalAudioSource;
    private Dictionary<List<AudioClip>, GameObject> _clipListSourceDict;
    private delegate void SoundPlayer();
    private int _clipCounter;
    private int _clipNumberMax;

    public enum SlidingSoundType
    {
        SlideInWood,
        SlideInMetal
    }

    public enum DropSoundType
    {
        MetalDrop,
        WoodDrop,
        BoltDrop,
        RubberDrop,
        TyreDrop
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        _globalAudioSourceObj = new GameObject();
        _globalAudioSource = _globalAudioSourceObj.AddComponent<AudioSource>();
        _globalAudioSource.playOnAwake = false;
        _globalAudioSource.spatialBlend = 0;
        _clipListSourceDict = new Dictionary<List<AudioClip>, GameObject>();
        _clipCounter = 0;
        _clipNumberMax = 5;
    }

    IEnumerator DecrementClipCounterWhenFinishedRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        _clipCounter--;
        if (_clipCounter < 0)
        {
            _clipCounter = 0;
        }
    }

    private void DecrementClipCounterWhenFinished(float clipLength)
    {
        StartCoroutine(DecrementClipCounterWhenFinishedRoutine(clipLength));
    }

    // Update is called once per frame
    void Update() {

    }

    private void PlayRandomGlobal(List<AudioClip> clipList)
    {
        int n = clipList.Count;
        if (n == 0)
        {
            throw new ArgumentException("Audio clip list cannot be zero.");
        }
        if (!_globalAudioSource.isPlaying)
        {
            int randomIndex = UnityEngine.Random.Range(0, n - 1);
            _globalAudioSource.clip = clipList[randomIndex];
            _globalAudioSource.Play();
        }
    }

    private void PlayRandomOneShot(List<AudioClip> clipList, Vector3 sourcePosition, 
        float volume = 1f)
    {
        int n = clipList.Count;
        if (n == 0)
        {
            throw new ArgumentException("Audio clip list cannot be zero.");
        }
        if (_clipCounter > _clipNumberMax)
        {
            return;
        }
        int randomIndex = UnityEngine.Random.Range(0, n - 1);
        AudioClip clip = clipList[randomIndex];
        AudioSource.PlayClipAtPoint(clip, sourcePosition, volume);
        _clipCounter++;
        DecrementClipCounterWhenFinished(clip.length);
    }

    private void PlayRandomWaitForLastToFinish(List<AudioClip> clipList, Vector3 sourcePosition, 
        float volume = 1f)
    {
        int n = clipList.Count;
        if (n == 0)
        {
            throw new ArgumentException("Audio clip list cannot be zero.");
        }
        bool spawnNewAudioSource = true;
        
        if (_clipListSourceDict.ContainsKey(clipList))
        {
            GameObject audioSourceObj = _clipListSourceDict[clipList];
            if (audioSourceObj != null)
            {
                // The audio is still playing.
                spawnNewAudioSource = false;
            }
            else
            {
                _clipListSourceDict.Remove(clipList);
            }
        }
        if (spawnNewAudioSource)
        {
            GameObject audioSourceObj = new GameObject();
            _clipListSourceDict.Add(clipList, audioSourceObj);
            audioSourceObj.transform.position = sourcePosition;
            AudioSource audioSource = audioSourceObj.AddComponent<AudioSource>();
            int randomIndex = UnityEngine.Random.Range(0, n - 1);
            AudioClip clip = clipList[randomIndex];
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(audioSourceObj, clip.length);
        }
    }

    public void PlayTeleportSound()
    {
        TeleportSoundSource.Play();
    }

    public void PlayBoltDropSound(Vector3 sourcePosition)
    {
        PlayRandomOneShot(BoltDropAudioList, sourcePosition);
    }

    public void PlaySlidingSound(SlidingSoundType type, Vector3 sourcePosition)
    {
        List<AudioClip> audioList = null;
        switch (type)
        {
            case SlidingSoundType.SlideInWood:
                audioList = SlideInWoodAudioList;
                break;

            case SlidingSoundType.SlideInMetal:
                audioList = SlideInMetalAudioList;
                break;
        }
        PlayRandomWaitForLastToFinish(audioList, sourcePosition);
    }

    public void PlayWoodSlidingSound(Vector3 sourcePosition)
    {
        PlayRandomWaitForLastToFinish(SlideInWoodAudioList, sourcePosition);
    }

    public void PlayMetalSlidingSound(Vector3 sourcePosition)
    {
        PlayRandomWaitForLastToFinish(SlideInMetalAudioList, sourcePosition);
    }

    public void PlayOneShotFromSoundType(DropSoundType type, Vector3 sourcePosition)
    {
        switch (type)
        {
            case DropSoundType.BoltDrop:
                PlayRandomOneShot(BoltDropAudioList, sourcePosition);
                break;

            case DropSoundType.MetalDrop:
                PlayRandomOneShot(MetalDropAudioList, sourcePosition);
                break;

            case DropSoundType.RubberDrop:
                PlayRandomOneShot(RubberDropAudioList, sourcePosition);
                break;

            case DropSoundType.WoodDrop:
                PlayRandomOneShot(WoodDropAudioList, sourcePosition);
                break;
        }
    }

    public void StopSlidingSound(SlidingSoundType type)
    {
        List<AudioClip> audioList = null;
        switch (type)
        {
            case SlidingSoundType.SlideInWood:
                audioList = SlideInWoodAudioList;
                break;
                
            case SlidingSoundType.SlideInMetal:
                audioList = SlideInMetalAudioList;
                break;
        }
        if (_clipListSourceDict.ContainsKey(audioList))
        {
            GameObject obj = _clipListSourceDict[audioList];
            if (obj != null)
            {
                Destroy(obj);
                _clipListSourceDict.Remove(audioList);
            }
        }
    }

    public void StopWoodSlidingSound()
    {
        if (_clipListSourceDict.ContainsKey(SlideInMetalAudioList))
        {
            GameObject obj = _clipListSourceDict[SlideInMetalAudioList];
            if (obj != null)
            {
                Destroy(obj);
                _clipListSourceDict.Remove(SlideInWoodAudioList);
            }
        }
    }

    public void StopMetalSlidingSound()
    {
        if (_clipListSourceDict.ContainsKey(SlideInWoodAudioList))
        {
            GameObject obj = _clipListSourceDict[SlideInWoodAudioList];
            if (obj != null)
            {
                Destroy(obj);
                _clipListSourceDict.Remove(SlideInWoodAudioList);
            }
        }
    }

    public void PlayTableDropSound(Vector3 sourcePosition)
    {
        PlayRandomOneShot(WoodDropAudioList, sourcePosition);
    }

    public void PlayPointAwardingSound()
    {
        PointAwardingSoundSource.Play();
    }

    public void PlayBonusPointAwardingSound()
    {
        BonusPointAwardingSoundSource.Play();
    }

    private void PlayWelcomeSoundNoDelay()
    {
        WelcomeSoundSource.Play();
    }

    public void PlayWelcomeSound()
    {
        PlaySoundWithDelay(PlayWelcomeSoundNoDelay, 3);
    }

    private IEnumerator PlaySoundWithDelayRoutine(SoundPlayer play, float delay)
    {
        yield return new WaitForSeconds(delay);
        play();
    }

    private void PlaySoundWithDelay(SoundPlayer play, float delay)
    {
        StartCoroutine(PlaySoundWithDelayRoutine(play, delay));
    }

    public void PlayWellDoneSound()
    {
        WellDoneNextStageAudioSource.Play();
    }

    public void PlayEnterFreePlayModeSound()
    {
        CongratsFreePlayModeAudioSource.Play();
    }

    public void PlayPowerUpSound()
    {
        PowerUpSoundSource.Play();
    }

    public void PlayPowerDownSound()
    {
        PowerDownSoundSource.Play();
    }
}