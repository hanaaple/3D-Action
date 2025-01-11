using Data.Item.Base;
using Data.Item.Data;
using Data.Item.Scriptable;
using Interaction.Base;
using Manager;
using Player;
using Save;
using UnityEngine;
using Util;

namespace Interaction.Object
{
    /// <summary>
    /// 인터랙션 - 아이템 줍기
    /// </summary>
    public class LootingItem : SavableObjectInteraction
    {
        // ItemStaticData와 Item의
        [SerializeField] private ItemData<Weapon, WeaponStaticData>[] weapons;
        [SerializeField] private ItemData<Armor, ArmorStaticData>[] armors;
        [SerializeField] private ItemData<Accessory, AccessoryStaticData>[] accessories;
        [SerializeField] private ItemData<Tool, ToolStaticData>[] tools;

        [SerializeField] private GameObject lootItem;

        private PlayerInteractor _playerInteractor;
        private Collider _collider;
        private bool _initialized;

        private static readonly int AnimIdPickUp = Animator.StringToHash("PickUp");

        // Initialize를 어디서 시켜주고 싶음.
        
        // 이거 Start로 바꾸고 LoadData를 Start에서 하도록 하고 싶음.
        protected override void Awake()
        {
            Initialize();
            base.Awake();
        }

        public void Initialize()
        {
            if(_initialized) return;
            
            _initialized = true;
            _collider = GetComponent<Collider>();
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
        
        private void OnTriggerEnter(Collider other)
        {
            //Debug.LogWarning($"Trigger enter   {!IsInteracted}");
            if (!IsInteracted
                //&& lootItem.activeSelf
                && other.TryGetComponent(out PlayerInteractor playerInteraction))
            {
                //Debug.LogWarning("Add Interaction");
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

        public override void Interact(PlayerInteractor playerInteractor)
        {
            IsInteracted = true;
            _playerInteractor = playerInteractor;
            
            _playerInteractor.InitMoveState();
            _playerInteractor.AnimatePlayer(AnimIdPickUp);
        }

        public override void OnAnimationEvent()
        {
            AudioSource.PlayClipAtPoint(AudioCollector.instance.GetAudioClip(AudioEnum.LootingAudio), transform.position);
            _playerInteractor.TryRemoveCloseItem(this);
            SetInteractable(false);
            var ownedItemViewModel = PlaySceneManager.instance.playerDataManager.ownedItemViewModel;
            var toastMessageManager = PlaySceneManager.instance.toastMessageManager;
            
            foreach (var looting in weapons)
            {
                var item = looting.GetItem();
                toastMessageManager.ToastMessage(item.GetItemDisplayName(), item.GetItemData().slotSprite, 1);
                ownedItemViewModel.AddItem(item);
            }

            foreach (var looting in armors)
            {
                var item = looting.GetItem();
                toastMessageManager.ToastMessage(item.GetItemDisplayName(), item.GetItemData().slotSprite, 1);
                ownedItemViewModel.AddItem(item);
            }

            foreach (var looting in accessories)
            {
                var item = looting.GetItem();
                toastMessageManager.ToastMessage(item.GetItemDisplayName(), item.GetItemData().slotSprite, 1);
                ownedItemViewModel.AddItem(item);
            }

            foreach (var looting in tools)
            {
                var item = looting.GetItem();
                toastMessageManager.ToastMessage(item.GetItemDisplayName(), item.GetItemData().slotSprite, item.possessionCount);
                ownedItemViewModel.AddItem(item);
            }
        }

        public override void OnInteractionEnd()
        {
            _playerInteractor.ChangePlayerStateByInputOrIdle();
        }
        
        public override Vector3 GetPosition()
        {
            return transform.position;
        }

        public override IObjectSaveData GetSaveData()
        {
            var interactableSaveData = new InteractableObjectSaveData
            {
                isInteracted = IsInteracted
            };
            return interactableSaveData;
        }

        /// <summary>
        /// Do not use in this class
        /// </summary>
        /// <param name="isEnable"></param>
        public void SetLootEnable(bool isEnable)
        {
            if (IsInteracted) return;

            SetInteractable(isEnable);
        }

        /// <summary>
        /// use in this class
        /// </summary>
        /// <param name="isInteractable"></param>
        protected override void SetInteractable(bool isInteractable)
        {
            gameObject.SetActive(isInteractable);
            _collider.enabled = isInteractable;
        }
    }
}