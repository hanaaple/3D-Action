namespace CharacterControl.State
{
    // Manage Fall to Land
    public class FallState : BaseActionState
    {
        public FallState(ThirdPlayerController controller) : base(controller)
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
        }

        public override void LateUpdate(ActionStateMachine stateMachine)
        {
        }

        public override bool StateChangeEnable(ActionStateMachine stateMachine)
        {
            return false;
        }
    }
}