using Character;
using Data.Item.Base;
using Data.Play;
using Manager;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationEvent : CharacterAnimationEvent
    {
        [SerializeField] private AudioClip rollingAudioClip;
        [Range(0, 1)] [SerializeField] private float rollingAudioVolume = 0.5f;

        private PlayerInteractor _playerInteractor;

        private readonly int _animIdRightGrip = Animator.StringToHash("RightGrip");
        private readonly int _animIdLeftGrip = Animator.StringToHash("LeftGrip");

        protected override void Start()
        {
            base.Start();

            _playerInteractor = GetComponentInParent<PlayerInteractor>();
        }

        // this work by animation event
        private void OnRoll(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f) return;
            AudioSource.PlayClipAtPoint(rollingAudioClip, transform.position, rollingAudioVolume);
        }

        // 이걸 실행시키는건 어딘데
        public void ChangeLeftWeapon(AnimationEvent animationEvent)
        {
            ChangeWeapon(WeaponEquipType.Left);
        }

        public void ChangeRightWeapon(AnimationEvent animationEvent)
        {
            ChangeWeapon(WeaponEquipType.Right);
        }

        public void AnimationEvent(AnimationEvent animationEvent)
        {
            _playerInteractor.OnAnimationEvent();
        }

        private void ChangeWeapon(WeaponEquipType weaponEquipType)
        {
            int animId = 0;
            if (weaponEquipType == WeaponEquipType.Left)
            {
                animId = _animIdLeftGrip;
            }
            else if (weaponEquipType == WeaponEquipType.Right)
            {
                animId = _animIdRightGrip;
            }

            var equipViewModel = PlaySceneManager.instance.playerDataManager.equipViewModel;
            equipViewModel.MoveToNextWeaponIndex(weaponEquipType);

            var weapon = equipViewModel.GetCurrentWeapon(weaponEquipType);
            // TODO: Weapon에 따른 Grip 변경
            if (weapon.IsNullOrBare())
            {
                Animator.SetBool(animId, false);
            }
            else
            {
                Animator.SetBool(animId, true);
            }
        }
    }
}