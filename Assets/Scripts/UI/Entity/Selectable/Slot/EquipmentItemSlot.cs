#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI.Entity.Selectable.Slot
{
#if UNITY_EDITOR
    [CustomEditor(typeof(EquipmentItemSlot), true)]
    [CanEditMultipleObjects]
    public class EquipmentItemSlotEditor : SelectableSlotEditor
    {
        private SerializedProperty _slotType;
        //private SerializedProperty _selectableContentProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _slotType = serializedObject.FindProperty("slotType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_slotType);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
#endif
    public enum EquipSlotType
    {
        LeftWeapon,
        RightWeapon,
        Helmet,
        BreastPlate,
        Leggings,
        Shoes,
        Accessory,
        Tool,
        All
    }

    public class EquipmentItemSlot : SelectableItemSlot
    {
        public EquipSlotType slotType;
    }
}