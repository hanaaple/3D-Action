using System;
using Player.State.Base;

namespace CharacterControl.State
{
    // Manage Fall to Land
    public class FallState : BasePlayerActionState
    {
        public FallState(PlayerContext playerContext, Enum state) : base(playerContext, state)
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
        }

        protected override bool StateChangeEnable(PlayerStateMachine stateMachine)
        {
            return false;
        }
    }
}