using System;
using Data;
using Data.PlayItem;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Entity.Selectable.Slot
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SelectableItemSlot), true)]
    [CanEditMultipleObjects]
    public class SelectableSlotEditor : SelectableEditor
    {
        private SerializedProperty _checkIconProperty;
        private SerializedProperty _iconProperty;
        private SerializedProperty _slotName;
        private SerializedProperty _equipmentType;
        private SerializedProperty _slotIndex;

        protected override void OnEnable()
        {
            base.OnEnable();
            _iconProperty = serializedObject.FindProperty("icon");
            _slotName = serializedObject.FindProperty("slotName");
            _equipmentType = serializedObject.FindProperty("equipmentType");
            _slotIndex = serializedObject.FindProperty("slotIndex");
            _checkIconProperty = serializedObject.FindProperty("checkIcon");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_slotName);
            EditorGUILayout.PropertyField(_slotIndex);
            EditorGUILayout.PropertyField(_iconProperty);
            EditorGUILayout.PropertyField(_checkIconProperty);
            EditorGUILayout.PropertyField(_equipmentType);
            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
#endif


// 각 슬롯은 Selectable이며, Slot에 따라 DescribeView가 변한다.

    public enum CheckType
    {
        Select,
        Other,
        None
    }
    
    [Flags]
    public enum EquipmentType
    {
        Weapon = 1,
        Armor = 2,
        Accessory = 4,
        Tool = 8
    }
    
    // 아이템 슬롯인데
    // -> Equip 창 -> 부위별 나누기
    // -> Inventory 창 -> 타입별 나누기
    public class SelectableItemSlot : SelectableSlot
    {
        public EquipmentType equipmentType;
        
        [SerializeField] private int slotIndex;
        [SerializeField] private string slotName;
        [SerializeField] private Image icon;
        [SerializeField] private Image checkIcon;
        
        [SerializeField] private Color selectColor;
        [SerializeField] private Color otherColor;
        
        public int equippedItemIndex => slotIndex - 1;
        
        private Item _item;

        public override void Select()
        {
            base.Select();

            if (EventSystem.current == null || EventSystem.current.alreadySelecting)
                return;

            DataManager.instance.selectedUiViewModel.selectedItemSlotData = this;
        }

        public void SetItem(Item item)
        {
            _item = item;
            if (item.IsNullOrEmpty())
            {
                icon.sprite = null;
                icon.enabled = false;
            }
            else
            {
                icon.sprite = item.GetItemData().slotSprite;
                icon.enabled = true;
            }
        }
        
        public Item GetItem()
        {
            return _item;
        }

        public T GetItem<T>() where T : Item
        {
            return _item as T;
        }

        public void Check(CheckType checkType)
        {
            if(!checkIcon) return;
            
            if (checkType == CheckType.Select)
            {
                checkIcon.color = selectColor;
            }
            else if (checkType == CheckType.Other)
            {
                checkIcon.color = otherColor;
            }
            else if (checkType == CheckType.None)
            {
                var color = checkIcon.color;
                color.a = 0;
                checkIcon.color = color;
            }
        }

        public string GetSlotName()
        {
            if (slotIndex == 0)
            {
                return $"{slotName}";
            }

            return $"{slotName} {slotIndex}";
        }
    }
}