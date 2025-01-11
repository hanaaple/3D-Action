using System;
using Player.State.Base;
using UnityEngine;

namespace CharacterControl.State
{
    public class RightHandChangeState : IdleState
    {
        private readonly int _animIdRightHand = Animator.StringToHash("RightWeaponChange");

        public RightHandChangeState(PlayerContext playerContext, Enum state) : base(playerContext, state)
        {
        }

        protected override void OnEnterState(PlayerStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            
            // WeaponChangeBehaviour.OnStateExit()에 의해 종료
            PlayerContext.PlayerController.animator.SetTrigger(_animIdRightHand);
        }
        protected override bool StateChangeEnable(PlayerStateMachine stateMachine)
        {
            if (PlayerContext.PlayerController.IsGrounded && stateMachine.CurrentStateEquals(PlayerStateMode.Idle))
            {
                return true;
            }

            return false;
        }
    }
}