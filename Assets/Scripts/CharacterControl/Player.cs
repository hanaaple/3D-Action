using CharacterControl.State;
using UnityEngine;

namespace CharacterControl
{
    // Player Data 존재
    // 세팅 값은 존재하지 않음
    // State Pattern - Client
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        
        internal ActionStateMachine StateMachine;

        public bool isStateMachineDebug;

        private void Start()
        {
            var controller = GetComponent<ThirdPlayerController>();
            StateMachine = new ActionStateMachine();
            StateMachine.Initialize(controller, isStateMachineDebug);
        }

        private void OnValidate()
        {
            if(StateMachine != null)
                StateMachine.IsDebug = isStateMachineDebug;
        }

        // Input -> Controller -> Player
        private void Update()
        {
            StateMachine.UpdateState();
        }
        
        private void LateUpdate()
        {
            StateMachine?.LateUpdateState();
        }
    }
}