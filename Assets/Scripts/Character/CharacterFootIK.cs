using Player;
using UnityEngine;

namespace Character
{
    public class CharacterFootIK : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private float distanceGround;
        [SerializeField] private Vector3 offset;

        [SerializeField] private float weightSmoothTime;
        
        private Animator _animator;
        private PlayerController _controller;

        private float _ikWeight;
        private float _weightVelocity;

        private Vector3 _leftFootPosition;
        private Vector3 _rightFootPosition;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponentInParent<PlayerController>();
        }

        private void Update()
        {
            if (_controller.GetMoveSpeed() > 0.1f)
            {
                _ikWeight = Mathf.SmoothDamp(_ikWeight, 0, ref _weightVelocity, weightSmoothTime);
            }
            else
            {
                _ikWeight = Mathf.SmoothDamp(_ikWeight, 1, ref _weightVelocity, weightSmoothTime);
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            _leftFootPosition = _animator.GetIKPosition(AvatarIKGoal.LeftFoot);
            _rightFootPosition = _animator.GetIKPosition(AvatarIKGoal.RightFoot);
            
            transform.localPosition = new Vector3(0, -Mathf.Abs(_leftFootPosition.y - _rightFootPosition.y) / 2f * _ikWeight, 0);

            SetFootIK(AvatarIKGoal.LeftFoot);
            SetFootIK(AvatarIKGoal.RightFoot);
        }

        private void SetFootIK(AvatarIKGoal ikGoal)
        {
            _animator.SetIKPositionWeight(ikGoal, _ikWeight);
            _animator.SetIKRotationWeight(ikGoal, _ikWeight);
            
            var ray = new Ray(_animator.GetIKPosition(ikGoal) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out var hit, distanceGround, LayerMask.GetMask("Ground")))
            {
                var footPosition = hit.point + offset;

                _animator.SetIKPosition(ikGoal, footPosition);
                _animator.SetIKRotation(ikGoal, Quaternion.LookRotation(transform.forward, hit.normal));
            }
        }
    
        private void OnDrawGizmos()
        {
            if (!_animator) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_leftFootPosition + Vector3.up, _leftFootPosition + Vector3.up + Vector3.down * distanceGround);
            Gizmos.DrawLine(_rightFootPosition + Vector3.up, _rightFootPosition + Vector3.up + Vector3.down * distanceGround);
        }
    }
}
