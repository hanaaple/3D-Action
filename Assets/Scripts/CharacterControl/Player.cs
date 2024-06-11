using CharacterControl.State.Base;
using Data.Play;
using Save;
using UnityEngine;
using ViewModel;

namespace CharacterControl
{
    // 세팅 값은 존재하지 않음
    // State Pattern - Client
    [RequireComponent(typeof(InputStateHandler), typeof(ThirdPlayerController))]
    public class Player : MonoBehaviour, ISavable
    {
        internal ActionStateMachine StateMachine;

        public bool isStateMachineDebug;

        public Transform leftHand;
        public Transform rightHand;

        public StatusViewModel statusViewModel { get; private set; }
        public PlayerDataViewModel playerDataViewModel { get; private set; }
        public EquipViewModel equipViewModel { get; private set; }
        public OwnedItemViewModel ownedItemViewModel { get; private set; }

        // For Debug
        [Space(20)] [SerializeField] private StatusData statusData;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private EquippedItemData equippedItemData;
        [SerializeField] private OwnedItemData ownedItemData;

        private void Start()
        {
            var controller = GetComponent<ThirdPlayerController>();
            var inputStateHandler = GetComponent<InputStateHandler>();
            var playerInteraction = GetComponent<PlayerInteraction>();

            PlayerContext playerContext = new PlayerContext
            {
                Controller = controller,
                PlayerInputHandler = inputStateHandler,
                PlayerInteraction = playerInteraction
            };

            StateMachine = new ActionStateMachine();
            StateMachine.Initialize(playerContext, isStateMachineDebug);
        }

        private void OnValidate()
        {
            if (StateMachine != null)
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

        public void LoadData(SaveData saveData)
        {
            statusData = saveData.playerSaveData.statusData;
            equippedItemData = saveData.playerSaveData.equippedItemData;
            ownedItemData = saveData.playerSaveData.ownedItemData;
            playerData = saveData.playerSaveData.playerData;

            InitializeViewModel();
        }

        public void CreateData()
        {
            statusData = new StatusData();
            equippedItemData = new EquippedItemData(3, 3, 4, 8);
            ownedItemData = new OwnedItemData();
            playerData = new PlayerData();

            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            statusViewModel = new StatusViewModel();
            statusViewModel.Initialize(statusData);

            playerDataViewModel = new PlayerDataViewModel();
            playerDataViewModel.Initialize(playerData, equippedItemData, statusData);

            equipViewModel = new EquipViewModel();
            equipViewModel.Initialize(equippedItemData);

            ownedItemViewModel = new OwnedItemViewModel();
            ownedItemViewModel.Initialize(ownedItemData);
        }

        public void BroadCastModelChange()
        {
            statusData.OnPropertyChanged();
            equippedItemData.OnPropertyChanged();
            ownedItemData.OnPropertyChanged();
        }

        public PlayerSaveData GetSaveData()
        {
            PlayerSaveData playerSaveData = new PlayerSaveData
            {
                statusData = statusData,
                equippedItemData = equippedItemData,
                ownedItemData = ownedItemData,
                playerData = playerData
            };
            return playerSaveData;
        }
    }
}