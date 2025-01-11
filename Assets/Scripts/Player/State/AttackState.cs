using System;
using Data.Item.Base;
using Data.Item.Data;
using Data.Item.Scriptable;
using Data.ViewModel;
using Player.State.Base;
using UnityEngine;
using Util;

namespace CharacterControl.State
{
    public abstract class AttackState : BasePlayerActionState
    {
        private bool _isRotateEnable;
        private bool _isComboEnable;
        private int _comboCount;

        private readonly int _animIdAttackCombo = Animator.StringToHash("AttackCombo");
        private readonly int _animIdAttackMotionSpeed = Animator.StringToHash("AttackMotionSpeed");

        protected AttackState(PlayerContext playerContext, Enum state) : base(playerContext, state)
        {
        }

        protected abstract int GetAttackHash();
        protected abstract Weapon GetCurrentWeapon();
        protected abstract PlayerStateMode GetPlayerStateMode();

        private void Attack()
        {
            var weapon = GetCurrentWeapon();
            var weaponData = weapon.GetWeaponData();    // TODO: 데메테르 법칙 예외 발생.
            
            var staminaAmountUsed = weaponData.staminaAmountUsed;
            var playerDataViewModel = PlayerContext.PlayerDataManager.playerDataViewModel;
            playerDataViewModel.DecreaseStaminaPoint(staminaAmountUsed, StaminaUseType.Attack);
            
            
            PlayerContext.PlayerController.animator.applyRootMotion = true;

            _isRotateEnable = false;
            _isComboEnable = false;
            
            if (_comboCount == 0)
            {
                PlayerContext.PlayerController.animator.SetTrigger(GetAttackHash());
            }

            PlayerContext.PlayerController.animator.SetInteger(_animIdAttackCombo, _comboCount);
        }

        protected override void OnEnterState(PlayerStateMachine stateMachine)
        {
            var animationEventHandler = PlayerContext.PlayerController.GetComponentInChildren<AnimationEventHandler>();
            animationEventHandler.OnRotationEnableChanged += SetRotationEnable;
            animationEventHandler.OnComboEnableChanged += SetComboEnable;

            PlayerContext.PlayerController.InitMoveState();
            
            _comboCount = 0;

            var weapon = GetCurrentWeapon();

            // 맨손
            if (weapon.IsNullOrEmpty())
            {
                Debug.LogWarning("Weapon is null");
                // PlayerContext.PlayerController.animator.runtimeAnimatorController = PlayerContext.PlayerController.OriginalAnimatorController;
                // PlayerContext.PlayerController.animator.SetFloat(_animIdAttackMotionSpeed, 1);
            }
            else
            {
                var weaponData = (WeaponStaticData)weapon.GetItemData();
                PlayerContext.PlayerController.animator.runtimeAnimatorController = weaponData.runtimeAnimatorController;
                PlayerContext.PlayerController.animator.SetFloat(_animIdAttackMotionSpeed, weaponData.attackMotionSpeed);
            }

            Attack();
        }

        // AttackBehaviour에 의해 종료
        protected override void OnExitState(PlayerStateMachine stateMachine)
        {
            var animationEventHandler = PlayerContext.PlayerController.GetComponentInChildren<AnimationEventHandler>();
            animationEventHandler.OnRotationEnableChanged -= SetRotationEnable;
            animationEventHandler.OnComboEnableChanged -= SetComboEnable;

            PlayerContext.PlayerController.animator.runtimeAnimatorController = PlayerContext.PlayerController.OriginalAnimatorController;
            PlayerContext.PlayerController.animator.applyRootMotion = false;
        }

        // 같은 종류의 무기 (연계 공격이 가능한) 타입의 경우 -> 연계 공격 (무기 2개로 공격하는?)
        // 맨손, 도끼, 대형 도끼, 망치, 대형 망치, 특대형 무기, 자검, 대검, 직검, 단검 등
        protected override void Update(PlayerStateMachine stateMachine, bool isOnChange = false)
        {
            // n ~ m초에 이동 방향으로 회전
            if (_isRotateEnable)
            {
                // 현재 누르고 있는 방향으로 회전 (빠르게 Smooth)
                PlayerContext.PlayerController.Rotate();
            }

            // m초 ~ end 사이에 추가 공격 입력이 들어온 경우
            if (_isComboEnable 
                // && !PlayerContext.PlayerController.IsTired
                )
            {
                // 좌 우 번갈아가는 콤보 공격은 없음

                // Check InputBuffer
                if (PlayerContext.PlayerController.TryGetInput(PlayerStateMode.RightAttack))
                {
                    // 콤보 공격
                    _comboCount++;
                    Attack();
                }
            }
        }

        protected override void LateUpdate(PlayerStateMachine stateMachine)
        {
        }
        
        protected override void FixedUpdate(PlayerStateMachine stateMachine, bool isOnChange = false)
        {
        }

        private void SetRotationEnable(bool isRotateEnable)
        {
            _isRotateEnable = isRotateEnable;
        }

        private void SetComboEnable(bool isComboEnable)
        {
            _isComboEnable = isComboEnable;
        }
    }
}