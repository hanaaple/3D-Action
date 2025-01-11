using Interaction.Base;
using Player;
using Save;
using UnityEngine;
using Util;

namespace Interaction.Object
{
    public class Chest : SavableObjectInteraction
    {
        [SerializeField] private LootingItem lootingItem;
        [SerializeField] private Transform animationStart;

        private PlayerInteractor _playerInteractor;
        private Animator _animator;
        private Collider _collider;

        private static readonly int OpenChest = Animator.StringToHash("OpenChest");
        private static readonly int IsOpened = Animator.StringToHash("IsOpened");

        protected override void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
            lootingItem.Initialize();
            base.Awake();
        }
        
        public override void CreateData()
        {
            IsInteracted = false;
            
            SetInteractable(true);
        }

        protected override void LoadData(InteractableObjectSaveData saveData)
        {
            IsInteracted = saveData.isInteracted;

            SetInteractable(!IsInteracted);
        }

        // Player에서 Trigger vs Interaction에서 Trigger
        
        private void OnTriggerEnter(Collider other)
        {
            if (!IsInteracted && other.TryGetComponent(out PlayerInteractor playerInteraction))
            {
                playerInteraction.AddCloseItem(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerInteractor playerInteraction))
            {
                playerInteraction.TryRemoveCloseItem(this);
            }
        }

        public override Vector3 GetPosition()
        {
            return transform.position;
        }

        public override void Interact(PlayerInteractor playerInteractor)
        {
            IsInteracted = true;
            _playerInteractor = playerInteractor;

            AudioSource.PlayClipAtPoint(AudioCollector.instance.GetAudioClip(AudioEnum.OpenChestAudio), transform.position, 1);
            
            _animator.SetTrigger(OpenChest);

            _playerInteractor.TryRemoveCloseItem(this);
            _playerInteractor.Teleport(animationStart);
            _playerInteractor.AnimatePlayer(OpenChest);
            
        }

        public override void OnInteractionEnd()
        {
            enabled = false;
            
            lootingItem.SetLootEnable(true);

            _playerInteractor.ChangePlayerStateByInputOrIdle();
        }
        
        /// <summary>
        /// Serialize
        /// </summary>
        public override IObjectSaveData GetSaveData()
        {
            var interactableSaveData = new InteractableObjectSaveData
            {
                isInteracted = IsInteracted
            };
            return interactableSaveData;
        }

        protected override void SetInteractable(bool isInteractable)
        {
            _collider.enabled = isInteractable;
            _animator.SetBool(IsOpened, !isInteractable);
            lootingItem.SetLootEnable(!isInteractable);
        }
    }
}