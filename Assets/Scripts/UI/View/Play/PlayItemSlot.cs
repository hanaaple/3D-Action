using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Play
{
    [Serializable]
    public class PlayItemImage
    {
        public Image itemImage;
        public Image slot;
    }

    /// <summary>
    /// UI 좌측 하단에 보이는 장착 중인 무기, 도구의 각 슬롯
    /// </summary> 
    [Serializable]
    public class PlayItemSlot
    {
        [SerializeField] private PlayItemImage[] itemSlots;
        [SerializeField] private Animator animator;
        
        private static readonly int BlinkHash = Animator.StringToHash("Blink");

        public void DisplaySlotUI(Sprite itemIcon, int index = 0)
        {
            foreach (var itemSlot in itemSlots)
            {
                itemSlot.itemImage.sprite = null;
                itemSlot.itemImage.enabled = false;
                itemSlot.slot.enabled = false;
            }
            itemSlots[0].slot.enabled = true;

            if (itemIcon != null)
            {
                itemSlots[index].itemImage.sprite = itemIcon;
                itemSlots[index].itemImage.enabled = true;
            }
        }

        public void DisplaySlotUI(IEnumerable<Sprite> itemIcons)
        {
            foreach (var itemSlot in itemSlots)
            {
                itemSlot.itemImage.sprite = null;
                itemSlot.itemImage.enabled = false;
                itemSlot.slot.enabled = false;
            }
            itemSlots[0].slot.enabled = true;

            int i = 0;
            foreach (var itemIcon in itemIcons)
            {
                var itemSlot = itemSlots[i];
                itemSlot.itemImage.sprite = itemIcon;
                itemSlot.itemImage.enabled = true;
                itemSlot.slot.enabled = true;
                i++;
            }
        }

        public void BlinkSlot()
        {
            animator.SetTrigger(BlinkHash);
        }

        public int GetCount()
        {
            return itemSlots.Length;
        }
    }
}