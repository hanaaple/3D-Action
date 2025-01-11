using System;
using Player.State.Base;

namespace CharacterControl.State
{
    public class StrongAttackState : BasePlayerActionState
    {
        public StrongAttackState(PlayerContext playerContext, Enum state) : base(playerContext, state)
        {
        }

        protected override void OnEnterState(PlayerStateMachine stateMachine)
        {
        }

        protected override void OnExitState(PlayerStateMachine stateMachine)
        {
        }

        protected override void Update(PlayerStateMachine stateMachine, bool isOnChange = false)
        {
            // 움직임 불가능
        }

        protected override void LateUpdate(PlayerStateMachine stateMachine)
        {
        }
        
        protected override void FixedUpdate(PlayerStateMachine stateMachine, bool isOnChange = false)
        {
        }

        protected override bool StateChangeEnable(PlayerStateMachine stateMachine)
        {
            // if (!PlayerContext.PlayerController.IsTired &&
            //     stateMachine.CurrentStateEquals(PlayerStateMode.Idle) 
            //     // ||
            //     // stateMachine.CurrentStateEquals(PlayerStateMode.RightAttack) ||
            //     // stateMachine.CurrentStateEquals(PlayerStateMode.StrongRightAttack)
            //     )
            // {
            //     return true;
            // }

            return false;
        }
    }
}