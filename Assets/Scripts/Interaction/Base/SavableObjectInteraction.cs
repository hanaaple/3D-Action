using System;
using Data;
using Player;
using Save;
using UnityEngine;
using Util;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Interaction.Base
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SavableObjectInteraction), true)]
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
                    SavableObjectInteraction myMonoBehaviour = (SavableObjectInteraction)target;
                    myMonoBehaviour.GenerateNewId();
                    EditorUtility.SetDirty(myMonoBehaviour);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    /// <summary>
    /// Save & Load에 따른 인스턴스의 상태를 따로 관리해야되는 오브젝트
    /// GameObject를 복사하는 경우, Id를 새롭게 Generate 해야한다.
    /// TODO: 인터페이스에 의해 public으로 사용해야 되는 메서드들이 마음에 안듬.
    /// </summary>
    public abstract class SavableObjectInteraction : MonoBehaviour, IInteractable, ISavableObject
    {
        [UniqueId] public string id;
        protected bool IsInteracted;
        
        [TextArea] public string uiContext;

        protected virtual void Awake()
        {
            RegistSaveData();
            
            LoadOrCreateData();
        }

        public void LoadOrCreateData()
        {
            if (PlayDataManager.instance.TryLoad(out var saveData))
            {
                var data = saveData.GetSaveData<InteractableObjectSaveData>(GetLabel(), GetId());

                if (data != null)
                {
                    LoadData(data);
                }
                else
                {
                    CreateData();
                }
            }
            else
            {
                CreateData();
            }
        }

        public void GenerateNewId()
        {
            id = Guid.NewGuid().ToString();
        }
        
        // 상호작용 UI에 나오는 설명란
        public string GetUIContext()
        {
            return uiContext;
        }

        public string GetName()
        {
            return gameObject.name;
        }

        public string GetLabel()
        {
            return "Interaction";
        }
        
        public string GetId()
        {
            return id;
        }
        
        public void RegistSaveData()
        {
            PlayDataManager.instance.RegistSavable(this);
        }
        
        public virtual void OnAnimationEvent()
        {
        }
        
        public abstract Vector3 GetPosition();
        public abstract void Interact(PlayerInteractor playerInteractor);
        public abstract void OnInteractionEnd();
        public abstract void CreateData();
        protected abstract void LoadData(InteractableObjectSaveData saveData);
        public abstract IObjectSaveData GetSaveData();
        protected abstract void SetInteractable(bool isInteractable);
    }
}