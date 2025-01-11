using System;
using Player.State.Base;
using UnityEngine;

namespace CharacterControl.State
{
    // 이거 진짜 아니다.
    public class InteractionState : BasePlayerActionState
    {
        public InteractionState(PlayerContext playerContext, Enum state) : base(playerContext, state)
        {
        }

        protected override void OnEnterState(PlayerStateMachine stateMachine)
        {
            // 각 애니메이션은 IInteractable의 Interaction()에 의해 작동된다.
            // 종료 후 ChangeState 또한 IInteractable에 의해 작동한다.
            PlayerContext.PlayerInteractor.Interact();
        }

        protected override void OnExitState(PlayerStateMachine stateMachine)
        {
        }

        protected override void Update(PlayerStateMachine stateMachine, bool isOnChange = false)
        {
        }

        protected override void LateUpdate(PlayerStateMachine stateMachine)
        {
        }

        protected override void FixedUpdate(PlayerStateMachine stateMachine, bool isOnChange = false)
        {
        }
        
        protected override bool StateChangeEnable(PlayerStateMachine stateMachine)
        {
            // 어떻게 입력이 들어오는가
            if (stateMachine.GetCurrentState().Equals(PlayerStateMode.Idle) &&
                PlayerContext.PlayerInteractor.IsInteractionExist() && PlayerContext.PlayerController.IsGrounded)
            {
                return true;
            }

            return false;
        }
    }
}