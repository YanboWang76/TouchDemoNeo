/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using UnityEngine;

namespace Libdexmo.Unity.Utility
{
    [Serializable]
    public class AudioSourceGroup
    {
        public List<AudioSourceField> AudioSources
        {
            get { return _audioSourceGroup; }
            set { _audioSourceGroup = value; }
        }
        [SerializeField]
        private List<AudioSourceField> _audioSourceGroup;

        private AudioSource _audioSourcePlaying;

        public void Play(int index)
        {
            int n = _audioSourceGroup.Count;
            if (index >= n)
            {
                return;
            }
            Stop(_audioSourcePlaying);
            AudioSource source = _audioSourceGroup[index].Source;
            if (source != null)
            {
                source.Play();
                _audioSourcePlaying = source;
            }
        }

        private void Stop(AudioSource source)
        {
            if (source != null && source.isPlaying)
            {
                source.Stop();
            }
        }

        public void Stop(int index)
        {
            int n = _audioSourceGroup.Count;
            if (index >= n)
            {
                return;
            }
            AudioSource source = _audioSourceGroup[index].Source;
            Stop(source);
        }

        public void PlayRandom()
        {
            int n = _audioSourceGroup.Count;
            if (n == 0)
            {
                return;
            }
            Stop(_audioSourcePlaying);
            int randomIndex = UnityEngine.Random.Range(0, n);
            AudioSource source = _audioSourceGroup[randomIndex].Source;
            if (source != null)
            {
                source.Play();
                _audioSourcePlaying = source;
            }
        }
    }
}
