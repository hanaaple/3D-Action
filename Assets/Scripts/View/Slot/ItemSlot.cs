using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace View.Slot
{
    [Serializable]
    public class ItemSlot
    {
        [SerializeField] private Image[] itemImages;
        [SerializeField] private Image[] itemSlots;
        
        private Animator _animator;
        private readonly int _blinkHash = Animator.StringToHash("Blink");

        public void DisplaySlotUI(Item.Item item)
        {
            for (var i = 0; i < itemImages.Length; i++)
            {
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                itemSlots[i].enabled = false;
            }
            itemSlots[0].enabled = true;

            if (item != null)
            {
                itemImages[0].sprite = item.slotImage;
                itemImages[0].enabled = true;
            }
        }

        public void DisplaySlotUI(ReadOnlyArray<Item.Item> items)
        {
            for (var i = 0; i < itemImages.Length; i++)
            {
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                itemSlots[i].enabled = false;
            }
            itemSlots[0].enabled = true;

            for (int i = 0; i < items.Count; i++)
            {
                itemImages[i].sprite = items[i].slotImage;
                itemImages[i].enabled = true;
                itemSlots[i].enabled = true;
            }
        }

        public void BlinkSlot()
        {
            // _animator.SetTrigger(_blinkHash);
        }

        public int GetCount()
        {
            return itemSlots.Length;
        }
    }
}