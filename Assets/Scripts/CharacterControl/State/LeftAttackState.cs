using Data;
using Data.PlayItem;
using Data.Static.Scriptable;
using UnityEngine;
using Util;

namespace CharacterControl.State
{
    public class LeftAttackState : BaseActionState
    {
        private bool _isRotateEnable;
        private bool _isComboEnable;
        private int _comboCount;

        private readonly int _animIdAttackCombo = Animator.StringToHash("AttackCombo");
        private readonly int _animIdLeftAttack = Animator.StringToHash("LeftAttack");
        private readonly int _animIdAttackMotionSpeed = Animator.StringToHash("AttackMotionSpeed");

        public LeftAttackState(PlayerContext playerContext) : base(playerContext)
        {
        }

        private void Attack()
        {
            PlayerContext.Controller.Animator.applyRootMotion = true;

            _isRotateEnable = false;
            _isComboEnable = false;

            if (_comboCount == 0)
            {
                PlayerContext.Controller.Animator.SetTrigger(_animIdLeftAttack);
            }

            PlayerContext.Controller.Animator.SetInteger(_animIdAttackCombo, _comboCount);
        }

        public override void OnEnterState(ActionStateMachine stateMachine)
        {
            var animationEventHandler = PlayerContext.Controller.GetComponent<AnimationEventHandler>();
            animationEventHandler.OnRotationEnableChanged += SetRotationEnable;
            animationEventHandler.OnComboEnableChanged += SetComboEnable;

            _comboCount = 0;

            var weapon = DataManager.instance.playerEquipViewModel.GetCurrentLeftWeapon();

            // 맨손
            if (weapon.IsNullOrEmpty())
            {
                PlayerContext.Controller.Animator.runtimeAnimatorController =
                    PlayerContext.Controller.OriginalAnimatorController;
                PlayerContext.Controller.Animator.SetFloat(_animIdAttackMotionSpeed, 1);
            }
            else
            {
                PlayerContext.Controller.Animator.runtimeAnimatorController =
                    ((WeaponData)weapon.GetItemData()).runtimeAnimatorController;
                PlayerContext.Controller.Animator.SetFloat(_animIdAttackMotionSpeed,
                    ((WeaponData)weapon.GetItemData()).attackMotionSpeed);
            }

            Attack();
        }

        // AttackBehaviour에 의해 종료
        public override void OnExitState(ActionStateMachine stateMachine)
        {
            var animationEventHandler = PlayerContext.Controller.GetComponent<AnimationEventHandler>();
            animationEventHandler.OnRotationEnableChanged -= SetRotationEnable;
            animationEventHandler.OnComboEnableChanged -= SetComboEnable;

            PlayerContext.Controller.Animator.applyRootMotion = false;
        }

        // 같은 종류의 무기 (연계 공격이 가능한) 타입의 경우 -> 연계 공격 (무기 2개로 공격하는?)
        // 맨손, 도끼, 대형 도끼, 망치, 대형 망치, 특대형 무기, 자검, 대검, 직검, 단검 등
        public override void Update(ActionStateMachine stateMachine, bool isOnChange = false)
        {
            // n ~ m초에 이동 방향으로 회전
            if (_isRotateEnable)
            {
                // 현재 누르고 있는 방향으로 회전 (빠르게 Smooth)
                PlayerContext.Controller.Rotate();
            }

            // m초 ~ end 사이에 추가 공격 입력이 들어온 경우
            if (_isComboEnable)
            {
                // 좌 우 번갈아가는 콤보 공격은 없음

                // Check InputBuffer
                if (PlayerContext.Controller.TryGetInput<LeftAttackState>())
                {
                    // 콤보 공격
                    _comboCount++;
                    Attack();
                }
            }
        }

        public override void LateUpdate(ActionStateMachine stateMachine)
        {
        }

        // 공격 중에 다른 State 불가능하도록 추가 구현
        public override bool StateChangeEnable(ActionStateMachine stateMachine)
        {
            if (stateMachine.IsTypeEqualToCurrentState(typeof(IdleState))
                // || stateMachine.IsTypeEqualToCurrentState(typeof(AttackState))
                // || stateMachine.IsTypeEqualToCurrentState(typeof(StrongAttackState))
               )
            {
                return true;
            }

            return false;
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