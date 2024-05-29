namespace CharacterControl.State
{
    // Include Locomotion (Idle, Move, Run, Falling)
    public class IdleState : BaseActionState
    {
        public IdleState(ThirdPlayerController controller) : base(controller)
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
            if (!isOnChange && Controller.TryChangeStateByInput(stateMachine))
            {
                return;
            }
            
            Controller.UpdateSpeed();
            Controller.Rotate();
            Controller.Translate();
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