using System;
using CharacterControl;
using Data;
using Save;
using UnityEditor;
using UnityEngine;
using Util;

namespace Interaction.Base
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SavableInteraction), true)]
    [CanEditMultipleObjects]
    public class SavableInteractionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);
            do
            {
                EditorGUILayout.PropertyField(property, true);
            } while (property.NextVisible(false));

            // 고유 ID 생성 버튼 추가
            if (GUILayout.Button("Generate New Unique ID"))
            {
                foreach (var target in targets)
                {
                    SavableInteraction myMonoBehaviour = (SavableInteraction)target;
                    myMonoBehaviour.GenerateNewId();
                    EditorUtility.SetDirty(myMonoBehaviour);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    /// <summary>
    /// Save 가능한 인터랙션
    /// Unique Id를 통해 데이터를 가져온다.
    /// 복사하는 경우, Id를 새롭게 Generate 해야한다.
    /// </summary>
    public abstract class SavableInteraction : MonoBehaviour, IInteractable, ISavable
    {
        [UniqueId] public string id;
        protected bool IsInteracted;
        
        [TextArea] public string uiContext;

        protected virtual void Awake()
        {
            DataManager.instance.RegistSavableInteraction(this);
        }

        public void GenerateNewId()
        {
            id = Guid.NewGuid().ToString();
        }
        
        public string GetUIContext()
        {
            return uiContext;
        }

        public string GetName()
        {
            return gameObject.name;
        }

        public abstract Vector3 GetPosition();

        public abstract void Interact(PlayerInteraction playerInteraction);

        public abstract void OnInteractionEnd();

        public virtual void OnAnimationEvent()
        {
            
        }
        
        public abstract void LoadData(SaveData saveData);
        public abstract InteractableSaveData GetSaveData();
    }
}