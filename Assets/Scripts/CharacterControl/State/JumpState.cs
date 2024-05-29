using UnityEngine;

namespace CharacterControl.State
{
    // Include (Jump, in falling, Land)
    public class JumpState : BaseActionState
    {
        private readonly int _animIdJump = Animator.StringToHash("Jump");

        public JumpState(ThirdPlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState(ActionStateMachine stateMachine)
        {
            Controller.IsGrounded = false;
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            Controller.VerticalVelocity = Mathf.Sqrt(Controller.jumpHeight * -2f * Controller.gravity);
            // update animator if using character
            Controller.Animator?.SetTrigger(_animIdJump);
            Controller.ChangeStateByInputOrIdle(stateMachine);
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
            if (Controller.IsGrounded && stateMachine.IsTypeEqualToCurrentState(typeof(IdleState)) && Controller.JumpTimeoutDelta <= 0.0f)
            {
                return true;
            }
            
            return false;
        }
    }
}