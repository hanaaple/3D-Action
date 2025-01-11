using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Util
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ConditionalHideInInspectorAttribute : PropertyAttribute
    {
        public string ComparedProperty { get; private set; }
        public object ComparedPropertyValue { get; private set; }

        public ConditionalHideInInspectorAttribute(string comparedProperty, object comparedPropertyValue)
        {
            ComparedProperty = comparedProperty;
            ComparedPropertyValue = comparedPropertyValue;
        }

        public ConditionalHideInInspectorAttribute(string booleanProperty)
        {
            ComparedProperty = booleanProperty;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ConditionalHideInInspectorAttribute))]
    public class Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // var Attribute = (ConditionalHideInInspectorAttribute)attribute;
            if (CanDraw(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return CanDraw(property) ? EditorGUI.GetPropertyHeight(property, label, true) : 0.0f;
        }

        private bool CanDraw(SerializedProperty property)
        {
            var conditionalAttribute = (ConditionalHideInInspectorAttribute)attribute;


            // Debug.Log(property.propertyPath);
            // Debug.Log(Attribute.comparedProperty);
            // Debug.Log(Attribute.comparedPropertyValue);
            // Debug.Log(property.propertyPath.Contains("."));
            string path = property.propertyPath.Contains(".")
                ? global::System.IO.Path.ChangeExtension(property.propertyPath, conditionalAttribute.ComparedProperty)
                : conditionalAttribute.ComparedProperty;

            var ComparedField = property.serializedObject.FindProperty(path);
            // Debug.Log(ComparedField);
            if (ComparedField == null)
            {
                int LastIndex = property.propertyPath.LastIndexOf(".");
                // Debug.Log(Attribute.comparedEnum);
                if (LastIndex == -1)
                {
                    return true;
                }

                path = global::System.IO.Path.ChangeExtension(property.propertyPath.Substring(0, LastIndex),
                    conditionalAttribute.ComparedProperty);

                ComparedField = property.serializedObject.FindProperty(path);
                // Debug.Log(ComparedField);
                if (ComparedField == null)
                {
                    return true;
                }
            }

            switch (ComparedField.type)
            {
                case "bool":
                    return ComparedField.boolValue;
                case "Enum":
                {
                    return ComparedField.intValue.Equals((int)conditionalAttribute.ComparedPropertyValue);
                }
            }

            return true;
        }
    }
#endif
}