using UnityEngine;

namespace CharacterControl.State
{
    public class LeftHandChangeState : BaseActionState
    {
        private readonly int _animIdLeftHand = Animator.StringToHash("Left Hand");

        public LeftHandChangeState(ThirdPlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState(ActionStateMachine stateMachine)
        {
            // OnAnimationEnd -> InputBuffer 상태에 따라 StateMachine.StateChange<buffer.Type>()
            Controller.Animator.SetTrigger(_animIdLeftHand);
        }

        public override void OnExitState(ActionStateMachine stateMachine)
        {
        }

        public override void Update(ActionStateMachine stateMachine, bool isOnChange = false)
        {
            Controller.UpdateSpeed();
            Controller.Rotate();
            Controller.Translate();
        }

        public override void LateUpdate(ActionStateMachine stateMachine)
        {
        }

        public override bool StateChangeEnable(ActionStateMachine stateMachine)
        {
            if (Controller.IsGrounded && stateMachine.IsTypeEqualToCurrentState(typeof(IdleState)))
            {
                return true;
            }

            return false;
        }
    }
}