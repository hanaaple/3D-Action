using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public enum AudioEnum
    {
        LootingAudio,
        OpenChestAudio
    }

    [Serializable]
    public class AudioData
    {
        public AudioEnum audioEnum;
        public AudioClip audioClip;
    }

    public class AudioCollector : MonoBehaviour
    {
        public static AudioCollector instance { get; private set; }

        private readonly Dictionary<AudioEnum, AudioClip> _dictionary = new(EnumComparer.For<AudioEnum>());

        [SerializeField] private AudioData[] audioData;

        private void Awake()
        {
            if (instance)
            {
                DestroyImmediate(this);
            }
            else
            {
                Initialize();
                instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void Initialize()
        {
            foreach (var data in audioData)
            {
                _dictionary.Add(data.audioEnum, data.audioClip);
            }
        }

        public AudioClip GetAudioClip(AudioEnum audioEnum)
        {
            if (!_dictionary.TryGetValue(audioEnum, out var audioClip))
            {
                Debug.LogWarning($"Audio Data 없음. {audioEnum}");
                return null;
            }

            return audioClip;
        }
    }
}