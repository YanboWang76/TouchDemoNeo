using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CTool.AudioPlayer
{
    /// <summary>
    /// 音频播放控制器
    /// 提供Play方法
    /// 提供Pause方法
    /// 提供Resume方法
    /// 提供Stop方法
    /// 提供Destory方法
    /// </summary>
    public class CAudioPlayer
    {
        private static CAudioPlayer instance;

        private CAudioPlayer() { }

        private GameObject audioPlayerObj;

        public static CAudioPlayer GetInstance()
        {
            if (instance == null)
            {
                instance = new CAudioPlayer();
            }
            return instance;
        }

        private void CreatAudioPlayerObj()
        {
            audioPlayerObj = new GameObject("CAudioPlayer");
            MonoBehaviour.DontDestroyOnLoad(audioPlayerObj);
        }

        internal AudioSource AddAudioSource()
        {
            if (audioPlayerObj == null)
                CreatAudioPlayerObj();
            AudioSource audioSource;
            audioSource = audioPlayerObj.AddComponent<AudioSource>();
            audioSource.volume = 1;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            return audioSource;
        }

        internal void Play(AudioSource audioSource, AudioClip clip)
        {
            Play(audioSource,clip, false, 1);
        }
        internal void Play(AudioSource audioSource, AudioClip clip, bool isLoop)
        {
            Play(audioSource,clip, isLoop, 1);
        }
        internal void Play(AudioSource audioSource, AudioClip clip, bool isLoop, float volume)
        {
            if (audioSource == null)
                return;
            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.clip = clip;
            audioSource.loop = isLoop;
            audioSource.volume = Mathf.Clamp01(volume);
            audioSource.Play();
        }
        internal void Pause(AudioSource audioSource)
        {
            if (audioSource == null || audioSource.clip == null)
                return;
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
        internal void Resume(AudioSource audioSource)
        {
            if (audioSource == null || audioSource.clip == null || audioSource.isPlaying)
                return;
            audioSource.Play();
        }
        internal void Stop(AudioSource audioSource)
        {
            if (audioSource != null)
                audioSource.Stop();
        }
        internal void Destory()
        {
            if (audioPlayerObj != null)
            {
                audioPlayerObj.gameObject.DestorySelf();
                audioPlayerObj = null;
            }
        }
    }

}
