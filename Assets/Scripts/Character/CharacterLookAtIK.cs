using UnityEngine;

namespace Character
{
    public class CharacterLookAtIK : MonoBehaviour
    {
        public Transform lookAtTarget;
        
        private Animator _animator;

        private float _ikWeight;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _ikWeight = Mathf.Lerp(_ikWeight, lookAtTarget != null ? 1 : 0, Time.deltaTime);
            _animator.SetLookAtWeight(_ikWeight);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (lookAtTarget != null)
            {
                _animator.SetLookAtPosition(lookAtTarget.position);
            }
        }
    }
}