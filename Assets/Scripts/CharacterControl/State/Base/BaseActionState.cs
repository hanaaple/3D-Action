namespace CharacterControl.State.Base
{
    public abstract class BaseActionState
    {
        protected readonly PlayerContext PlayerContext;

        protected BaseActionState(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
        }

        // this execution order is slower than Controller
        public abstract void OnEnterState(ActionStateMachine stateMachine);
        public abstract void OnExitState(ActionStateMachine stateMachine);
        public abstract void Update(ActionStateMachine stateMachine, bool isOnChange = false);
        public abstract void LateUpdate(ActionStateMachine stateMachine);

        public abstract bool StateChangeEnable(ActionStateMachine stateMachine);
    }
}