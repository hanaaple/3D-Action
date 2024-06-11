using CharacterControl;
using Save;
using UnityEngine;
using Util;

namespace Interaction
{
    public class Chest : SavableInteraction
    {
        [SerializeField] private LootingItem lootingItem;
        [SerializeField] private Transform animationStart;

        private PlayerInteraction _playerInteraction;
        private Animator _animator;

        private static readonly int OpenChest = Animator.StringToHash("OpenChest");
        private static readonly int IsOpened = Animator.StringToHash("IsOpened");

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsInteracted && other.TryGetComponent(out PlayerInteraction playerInteraction))
            {
                playerInteraction.AddCloseItem(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerInteraction playerInteraction))
            {
                playerInteraction.TryRemoveCloseItem(this);
            }
        }

        public override Vector3 GetPosition()
        {
            return transform.position;
        }

        public override void Interact(PlayerInteraction playerInteraction)
        {
            IsInteracted = true;
            _playerInteraction = playerInteraction;

            AudioSource.PlayClipAtPoint(AudioManager.instance.openChestAudio, transform.position, 1);
            
            _animator.SetTrigger(OpenChest);

            _playerInteraction.TryRemoveCloseItem(this);
            _playerInteraction.Teleport(animationStart);
            _playerInteraction.Animator.SetTrigger(OpenChest);
        }

        public override void OnInteractionEnd()
        {
            enabled = false;
            
            lootingItem.SetLootEnable(true);

            _playerInteraction.Player.StateMachine.ChangeStateByInputOrIdle();
        }

        public override void LoadData(SaveData saveData)
        {
            if (saveData.InteractableSaveData.TryGetValue(id, out var interactableSaveData))
            {
                IsInteracted = interactableSaveData.isInteracted;

                if (IsInteracted)
                {
                    _animator.SetBool(IsOpened, true);
                    lootingItem.SetLootEnable(true);
                }
                else
                {
                    _animator.SetBool(IsOpened, false);
                    lootingItem.SetLootEnable(false);
                }
            }
        }

        public override InteractableSaveData GetSaveData()
        {
            var interactableSaveData = new InteractableSaveData
            {
                isInteracted = IsInteracted
            };
            return interactableSaveData;
        }
    }
}