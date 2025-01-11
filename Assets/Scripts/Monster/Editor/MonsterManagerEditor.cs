using UnityEditor;
using UnityEngine;

namespace Monster.Editor
{
    [CustomEditor(typeof(MonsterManager), true), CanEditMultipleObjects]
    public class MonsterManagerEditor : UnityEditor.Editor
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
                MonsterManager myMonoBehaviour = (MonsterManager)target;
                myMonoBehaviour.GenerateNewId();
                EditorUtility.SetDirty(myMonoBehaviour);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}