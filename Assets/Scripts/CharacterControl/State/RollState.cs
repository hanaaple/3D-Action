using UnityEngine;

namespace CharacterControl.State
{
    public class RollState : BaseActionState
    {
        private readonly int _animIdRoll = Animator.StringToHash("Roll");

        public RollState(PlayerContext playerContext) : base(playerContext)
        {
        }

        // Roll
        public override void OnEnterState(ActionStateMachine stateMachine)
        {
            PlayerContext.Controller.RotateImmediately();

            PlayerContext.Controller.MoveSpeed = PlayerContext.Controller.rollSpeed;

            PlayerContext.Controller.Animator?.SetTrigger(_animIdRoll);
        }

        public override void OnExitState(ActionStateMachine stateMachine)
        {
        }

        public override void Update(ActionStateMachine stateMachine, bool isOnChange = false)
        {
            PlayerContext.Controller.Translate();
        }

        public override void LateUpdate(ActionStateMachine stateMachine)
        {
        }

        public override bool StateChangeEnable(ActionStateMachine stateMachine)
        {
            if (PlayerContext.Controller.IsGrounded && stateMachine.IsTypeEqualToCurrentState(typeof(IdleState)) &&
                PlayerContext.Controller.RollTimeoutDelta <= 0.0f)
            {
                return true;
            }

            return false;
        }
    }
}