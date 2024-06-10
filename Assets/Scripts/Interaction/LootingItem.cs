using System;
using CharacterControl;
using Data;
using Data.PlayItem;
using Data.Static.Scriptable;
using UnityEngine;

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
    public class LootingItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private Looting<Weapon, WeaponData>[] weapons;
        [SerializeField] private Looting<Armor, ArmorData>[] armors;
        [SerializeField] private Looting<Accessory, AccessoryData>[] accessories;
        [SerializeField] private Looting<Tool, ToolData>[] tools;

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
            if (other.TryGetComponent(out PlayerInteraction playerInteraction))
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

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Interact(PlayerInteraction playerInteraction)
        {
            playerInteraction.TryRemoveCloseItem(this);
            gameObject.SetActive(false);

            foreach (var looting in weapons)
                DataManager.instance.playerOwnedItemViewModel.AddItem(looting.item);
            foreach (var looting in armors)
                DataManager.instance.playerOwnedItemViewModel.AddItem(looting.item);
            foreach (var looting in accessories)
                DataManager.instance.playerOwnedItemViewModel.AddItem(looting.item);
            foreach (var looting in tools)
                DataManager.instance.playerOwnedItemViewModel.AddItem(looting.item);
        }
    }
}