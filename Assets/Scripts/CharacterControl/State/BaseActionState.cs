namespace CharacterControl.State
{
    public abstract class BaseActionState
    {
        internal readonly ThirdPlayerController Controller;

        protected BaseActionState(ThirdPlayerController controller)
        {
            Controller = controller;
        }

        // this execution order is slower than Controller
        public abstract void OnEnterState(ActionStateMachine stateMachine);
        public abstract void OnExitState(ActionStateMachine stateMachine);
        public abstract void Update(ActionStateMachine stateMachine, bool isOnChange = false);
        public abstract void LateUpdate(ActionStateMachine stateMachine);

        public abstract bool StateChangeEnable(ActionStateMachine stateMachine);
    }
}