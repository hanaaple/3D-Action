using CharacterControl.State.Base;

namespace CharacterControl.State
{
    // Include Locomotion (Idle, Move, Run, Falling)
    public class IdleState : BaseActionState
    {
        public IdleState(PlayerContext playerContext) : base(playerContext)
        {
        }

        public override void OnEnterState(ActionStateMachine stateMachine)
        {
        }

        public override void OnExitState(ActionStateMachine stateMachine)
        {
        }

        public override void Update(ActionStateMachine stateMachine, bool isOnChange = false)
        {
            if (!isOnChange && stateMachine.TryChangeStateByInput())
            {
                return;
            }
            PlayerContext.Controller.UpdateAnimationSpeed();
            PlayerContext.Controller.UpdateSpeed();
            PlayerContext.Controller.Rotate();
            PlayerContext.Controller.Translate();
        }

        public override void LateUpdate(ActionStateMachine stateMachine)
        {
            
        }

        public override bool StateChangeEnable(ActionStateMachine stateMachine)
        {
            return true;
        }
    }
}