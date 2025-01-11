using System;
using Data.ViewModel;
using Player.State.Base;
using UnityEngine;

namespace CharacterControl.State
{
    public class RollState : BasePlayerActionState
    {
        private readonly int _animIdRoll = Animator.StringToHash("Roll");

        public RollState(PlayerContext playerContext, Enum state) : base(playerContext, state)
        {
        }

        // Roll
        protected override void OnEnterState(PlayerStateMachine stateMachine)
        {
            // 캐릭터 무게에 따른 스태미나 사용
            var playerDataViewModel = PlayerContext.PlayerDataManager.playerDataViewModel;
            playerDataViewModel.DecreaseStaminaPoint(
                playerDataViewModel.EquipWeight * PlayerContext.PlayerController.rollStaminaUseAmount,
                StaminaUseType.Roll);

            PlayerContext.PlayerController.RotateImmediately();

            PlayerContext.PlayerController.animator.applyRootMotion = true;
            PlayerContext.PlayerController.animator?.SetTrigger(_animIdRoll);
        }

        protected override void OnExitState(PlayerStateMachine stateMachine)
        {
            PlayerContext.PlayerController.animator.applyRootMotion = false;
        }

        protected override void Update(PlayerStateMachine stateMachine, bool isOnChange = false)
        {
        }

        protected override void LateUpdate(PlayerStateMachine stateMachine)
        {
        }

        protected override void FixedUpdate(PlayerStateMachine stateMachine, bool isOnChange = false)
        {
            PlayerContext.PlayerController.Translate(Time.fixedDeltaTime);
        }

        protected override bool StateChangeEnable(PlayerStateMachine stateMachine)
        {
            if (
                // !PlayerContext.PlayerController.IsTired && 
                PlayerContext.PlayerController.IsGrounded
                && stateMachine.CurrentStateEquals(PlayerStateMode.Idle)
                && PlayerContext.PlayerController.RollTimeoutDelta <= 0.0f)
            {
                return true;
            }

            return false;
        }
    }
}