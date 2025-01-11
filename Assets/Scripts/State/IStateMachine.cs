using System;

namespace State
{
    public interface IStateMachine
    {
        public void UpdateState();
        public void LateUpdateState();
        public void ChangeState(Enum type, bool isUpdate = false);

        // public T GetState(Type type);

        public Enum GetCurrentState();

        public bool CurrentStateEquals(Enum type);

        public bool ChangeStateEnable(Enum type);
    }
}