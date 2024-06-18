using System;
using CharacterControl;
using Data;
using Data.PlayItem;
using Data.Static.Scriptable;
using Interaction.Base;
using Save;
using UI.Entity.ToastMessage;
using UnityEngine;
using Util;

namespace Interaction
{
    [Serializable]
    public class Looting<T, TData> where T : Item where TData : ItemData
    {
        public TData itemData;
        public T item;
    }

    /// <summary>
    /// 인터랙션 - 아이템 줍기
    /// </summary>
    public class LootingItem : SavableInteraction
    {
        [SerializeField] private Looting<Weapon, WeaponData>[] weapons;
        [SerializeField] private Looting<Armor, ArmorData>[] armors;
        [SerializeField] private Looting<Accessory, AccessoryData>[] accessories;
        [SerializeField] private Looting<Tool, ToolData>[] tools;

        [SerializeField] private GameObject lootItem;

        private PlayerInteraction _playerInteraction;
        private Collider _collider;

        private readonly int _animIdPickUp = Animator.StringToHash("PickUp");

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider>();

            OnValidate();
        }

        private void OnValidate()
        {
            foreach (var looting in weapons)
            {
                looting.item.SetItemData(looting.itemData);
            }

            foreach (var looting in armors)
            {
                looting.item.SetItemData(looting.itemData);
            }

            foreach (var looting in accessories)
            {
                looting.item.SetItemData(looting.itemData);
            }

            foreach (var looting in tools)
            {
                looting.item.SetItemData(looting.itemData);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsInteracted && lootItem.activeSelf && other.TryGetComponent(out PlayerInteraction playerInteraction))
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
            
            _playerInteraction.InitMoveState();
            _playerInteraction.Animator.SetTrigger(_animIdPickUp);
        }

        public override void OnAnimationEvent()
        {
            AudioSource.PlayClipAtPoint(AudioManager.instance.lootingAudio, transform.position);
            _playerInteraction.TryRemoveCloseItem(this);
            SetEnable(false);
            
            foreach (var looting in weapons)
            {
                ToastMessageManager.instance.Toast(looting.item.GetItemName(), looting.item.GetItemData().slotSprite, 1);
                DataManager.instance.playerOwnedItemViewModel.AddItem(looting.item);
            }

            foreach (var looting in armors)
            {
                ToastMessageManager.instance.Toast(looting.item.GetItemName(), looting.item.GetItemData().slotSprite, 1);
                DataManager.instance.playerOwnedItemViewModel.AddItem(looting.item);
            }

            foreach (var looting in accessories)
            {
                ToastMessageManager.instance.Toast(looting.item.GetItemName(), looting.item.GetItemData().slotSprite, 1);
                DataManager.instance.playerOwnedItemViewModel.AddItem(looting.item);
            }

            foreach (var looting in tools)
            {
                ToastMessageManager.instance.Toast(looting.item.GetItemName(), looting.item.GetItemData().slotSprite, looting.item.possessionCount);
                DataManager.instance.playerOwnedItemViewModel.AddItem(looting.item);
            }
        }

        public override void OnInteractionEnd()
        {
            _playerInteraction.Player.StateMachine.ChangeStateByInputOrIdle();
        }

        public override void LoadData(SaveData saveData)
        {
            if (saveData.InteractableSaveData.TryGetValue(id, out var interactableSaveData))
            {
                IsInteracted = interactableSaveData.isInteracted;

                if (IsInteracted)
                    SetEnable(false);
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

        /// <summary>
        /// Do not use in this class
        /// </summary>
        /// <param name="isEnable"></param>
        public void SetLootEnable(bool isEnable)
        {
            if (IsInteracted) return;

            SetEnable(isEnable);
        }

        /// <summary>
        /// use in this class
        /// </summary>
        /// <param name="isEnable"></param>
        private void SetEnable(bool isEnable)
        {
            lootItem.SetActive(isEnable);
            _collider.enabled = isEnable;
        }
    }
}