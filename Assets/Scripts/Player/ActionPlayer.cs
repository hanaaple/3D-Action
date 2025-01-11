using System.ComponentModel;
using Character;
using Cinemachine;
using Manager;
using Player.State.Base;
using UI;
using UI.View.Play;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    // 메서드를 통해 얻게 하느냐
    // 컴포넌트에 접근시켜서 알아서 하게 하나
    // 적절히 섞어서 분배.
    
    
    // 세팅 값은 존재하지 않음
    // State Pattern - Client
    [RequireComponent(typeof(InputStateHandler))]
    [RequireComponent(typeof(CharacterBodyManager))]
    [RequireComponent(typeof(PlayerDataManager))]
    [RequireComponent(typeof(PlayerInteractor))]
    [RequireComponent(typeof(PlayerController))]
    public class ActionPlayer : MonoBehaviour
    {
        internal PlayerStateMachine StateMachine;
        
        public bool isStateMachineDebug;
        
        public PlayerDataManager playerDataManager;
        [SerializeField] private CharacterBodyManager characterBodyManager; // GetComponent를 통해 가져올 수 있지만, Awake에 묶여 Order에 의존성이 발생한다.
        // public PlayerController playerController;
        
        public CharacterBodyManager body => characterBodyManager;

        public void Initialize(GameObject lockOnTarget, CinemachineVirtualCamera virtualCamera, UIManager uiManager)
        {
            var inputStateHandler = GetComponent<InputStateHandler>();
            var playerController = GetComponent<PlayerController>();
            playerController.Initialize(lockOnTarget, virtualCamera);
            inputStateHandler.Initialize(uiManager);
        }
        
        private void Awake()
        {
            // UI를 가져올 방법이 마땅치 않음. 가져올 경우 외부 의존성이 발생한다. 또한, 큰 성능의 문제가 발생하지 않는다. (타이틀 화면 -> 게임 진입 시 1회)
            var interactionUIView = FindObjectOfType<InteractionBaseUIView>(true);
            var playerController = GetComponent<PlayerController>();
            var playerInteraction = GetComponent<PlayerInteractor>();
            
            playerInteraction.Initialize(this, playerController, interactionUIView);
        }

        private void Start()
        {
            var playerController = GetComponent<PlayerController>();
            var inputStateHandler = GetComponent<InputStateHandler>();
            var playerInteraction = GetComponent<PlayerInteractor>();
            
            PlayerContext playerContext = new PlayerContext
            {
                PlayerController = playerController,
                PlayerDataManager = playerDataManager,
                PlayerInputHandler = inputStateHandler,
                PlayerInteractor = playerInteraction
            };
            
            StateMachine = new PlayerStateMachine();
            StateMachine.Initialize(playerContext, isStateMachineDebug);
        }

        private void OnValidate()
        {
            if (StateMachine != null)
                StateMachine.IsDebug = isStateMachineDebug;
        }

        private void OnEnable()
        {
            playerDataManager.AddPropertyChange(ViewModelType.Equip, UpdateEquipmentInstance);
        }

        private void OnDisable()
        {
            playerDataManager.RemovePropertyChange(ViewModelType.Equip, UpdateEquipmentInstance);
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
        
        private void FixedUpdate()
        {
            StateMachine?.FixedUpdateState();
        }

        public void Teleport(Transform spawnTransform)
        {
            // 
            transform.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
        }
        
        private void UpdateEquipmentInstance(object s, PropertyChangedEventArgs e)
        {
            PlaySceneManager.instance.equipmentInstanceManager.UpdateEquipment(characterBodyManager, playerDataManager.equipViewModel);
        }
    }
}