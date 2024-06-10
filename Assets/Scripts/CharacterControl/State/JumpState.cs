using CharacterControl.State.Base;
using UnityEngine;

namespace CharacterControl.State
{
    // Include (Jump, in falling, Land)
    public class JumpState : BaseActionState
    {
        private readonly int _animIdJump = Animator.StringToHash("Jump");

        public JumpState(PlayerContext playerContext) : base(playerContext)
        {
        }

        public override void OnEnterState(ActionStateMachine stateMachine)
        {
            PlayerContext.Controller.IsGrounded = false;
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            PlayerContext.Controller.VerticalVelocity =
                Mathf.Sqrt(PlayerContext.Controller.jumpHeight * -2f * PlayerContext.Controller.gravity);
            // update animator if using character
            PlayerContext.Controller.Animator?.SetTrigger(_animIdJump);
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
            if (PlayerContext.Controller.IsGrounded && stateMachine.IsTypeEqualToCurrentState(typeof(IdleState)) &&
                PlayerContext.Controller.JumpTimeoutDelta <= 0.0f)
            {
                return true;
            }

            return false;
        }
    }
}