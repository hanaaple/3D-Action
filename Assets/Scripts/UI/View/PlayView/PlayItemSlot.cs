using System;
using Data.Static.Scriptable;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace UI.View.PlayView
{
    /// <summary>
    /// 장착 중인 무기, 도구의 각 슬롯
    /// </summary>
    [Serializable]
    public class PlayItemSlot
    {
        [SerializeField] private Image[] itemImages;
        [SerializeField] private Image[] itemSlots;
        
        [SerializeField] private Animator animator;
        private readonly int _blinkHash = Animator.StringToHash("Blink");
        
        public void DisplaySlotUI(ItemData itemData)
        {
            for (var i = 0; i < itemImages.Length; i++)
            {
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                itemSlots[i].enabled = false;
            }
            itemSlots[0].enabled = true;

            if (itemData != null)
            {
                itemImages[0].sprite = itemData.slotSprite;
                itemImages[0].enabled = true;
            }
        }

        public void DisplaySlotUI(ReadOnlyArray<ItemData> items)
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
                itemImages[i].sprite = items[i].slotSprite;
                itemImages[i].enabled = true;
                itemSlots[i].enabled = true;
            }
        }

        public void BlinkSlot()
        {
            animator.SetTrigger(_blinkHash);
        }

        public int GetCount()
        {
            return itemSlots.Length;
        }
    }
}