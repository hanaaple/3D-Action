using UnityEngine;
using Random = UnityEngine.Random;

namespace Character
{
    public class CharacterAnimationEvent : MonoBehaviour
    {
        [SerializeField] private AudioClip landingAudioClip;
        [SerializeField] private AudioClip[] footstepAudioClips;
        [Range(0, 1)] [SerializeField] private float footstepAudioVolume = 0.5f;
        [Range(0, 1)] [SerializeField] private float landingAudioVolume = 0.5f;

        protected Animator Animator;
        
        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
        }

        // this work by animation event
        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f) return;
            if (footstepAudioClips.Length <= 0) return;

            var index = Random.Range(0, footstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.position, footstepAudioVolume);
        }

        // this work by animation event
        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f) return;
            AudioSource.PlayClipAtPoint(landingAudioClip, transform.position, landingAudioVolume);
        }
    }
}