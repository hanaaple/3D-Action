using UnityEngine;

namespace CharacterControl.State
{
    public class RollState : BaseActionState
    {
        private readonly int _animIdRoll = Animator.StringToHash("Roll");

        public RollState(ThirdPlayerController controller) : base(controller)
        {
        }

        // Roll
        public override void OnEnterState(ActionStateMachine stateMachine)
        {
            Controller.RotateImmediately();

            Controller.MoveSpeed = Controller.rollSpeed;

            Controller.Animator?.SetTrigger(_animIdRoll);
        }

        public override void OnExitState(ActionStateMachine stateMachine)
        {
        }

        public override void Update(ActionStateMachine stateMachine, bool isOnChange = false)
        {
            Controller.Translate();
        }

        public override void LateUpdate(ActionStateMachine stateMachine)
        {
        }

        public override bool StateChangeEnable(ActionStateMachine stateMachine)
        {
            if (Controller.IsGrounded && stateMachine.IsTypeEqualToCurrentState(typeof(IdleState)) &&
                Controller.RollTimeoutDelta <= 0.0f)
            {
                return true;
            }

            return false;
        }
    }
}