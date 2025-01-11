using System;
using Player.State.Base;
using UnityEngine;

namespace CharacterControl.State
{
    // Include Locomotion (Idle, Move, Run, Falling)
    public class IdleState : BasePlayerActionState
    {
        public IdleState(PlayerContext playerContext, Enum state) : base(playerContext, state)
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
        }

        protected override void LateUpdate(PlayerStateMachine stateMachine)
        {

        }

        protected override void FixedUpdate(PlayerStateMachine stateMachine, bool isOnChange = false)
        {
            if (!isOnChange && stateMachine.TryChangeStateByInput())
            {
                return;
            }

            PlayerContext.PlayerController.Move(Time.fixedDeltaTime, PlayerContext.PlayerInputHandler.run);
        }

        protected override bool StateChangeEnable(PlayerStateMachine stateMachine)
        {
            return true;
        }
    }
}