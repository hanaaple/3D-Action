using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Test)), CanEditMultipleObjects]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Read Data"))
        {
            var test = (Test)target;
            test.Read();
        }
    }
}