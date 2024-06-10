﻿using CharacterControl.State.Base;

namespace CharacterControl.State
{
    public class InteractionState : BaseActionState
    {
        public InteractionState(PlayerContext playerContext) : base(playerContext)
        {
        }

        public override void OnEnterState(ActionStateMachine stateMachine)
        {
            PlayerContext.PlayerInteraction.Loot();
            
            // 애니메이션 추가, 종료 후 ChangeState
            stateMachine.ChangeStateByInputOrIdle();
        }

        public override void OnExitState(ActionStateMachine stateMachine)
        {
        }

        public override void Update(ActionStateMachine stateMachine, bool isOnChange = false)
        {
        }

        public override void LateUpdate(ActionStateMachine stateMachine)
        {
        }

        public override bool StateChangeEnable(ActionStateMachine stateMachine)
        {
            if (stateMachine.GetCurrentStateType() == typeof(IdleState) &&
                PlayerContext.PlayerInteraction.IsInteractionExist() && PlayerContext.Controller.IsGrounded)
            {
                return true;
            }

            return false;
        }
    }
}