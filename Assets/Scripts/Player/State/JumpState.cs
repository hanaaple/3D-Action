using System;
using Player.State.Base;
using UnityEngine;

namespace CharacterControl.State
{
    // Include (Jump, in falling, Land)
    public class JumpState : BasePlayerActionState
    {
        private readonly int _animIdJump = Animator.StringToHash("Jump");

        public JumpState(PlayerContext playerContext, Enum state) : base(playerContext, state)
        {
        }

        protected override void OnEnterState(PlayerStateMachine stateMachine)
        {
            PlayerContext.PlayerController.IsGrounded = false;
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            PlayerContext.PlayerController.VerticalVelocity =
                Mathf.Sqrt(PlayerContext.PlayerController.jumpHeight * -2f * PlayerContext.PlayerController.gravity);
            // update animator if using character
            PlayerContext.PlayerController.animator?.SetTrigger(_animIdJump);
            stateMachine.ChangeStateByInputOrIdle();
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
            if (PlayerContext.PlayerController.IsGrounded && stateMachine.CurrentStateEquals(PlayerStateMode.Idle) &&
                PlayerContext.PlayerController.JumpTimeoutDelta <= 0.0f)
            {
                return true;
            }

            return false;
        }
    }
}