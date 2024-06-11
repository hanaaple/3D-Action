using UnityEngine;

namespace Util
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager instance => _instance;
        
        public AudioClip lootingAudio;
        public AudioClip openChestAudio;

        private void Awake()
        {
            if (_instance)
            {
                DestroyImmediate(this);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
        }
    }
}
