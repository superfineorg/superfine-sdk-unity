using UnityEditor;
using UnityEngine;

namespace Superfine
{
    [CustomPropertyDrawer(typeof(SuperfineNullable<>))]
    public class SuperfineNullableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var setRect = new Rect(position.x, position.y, 15, position.height);
            var consumed = setRect.width + 5;
            var valueRect = new Rect(position.x + consumed, position.y, position.width - consumed, position.height);

            var hasValueProp = property.FindPropertyRelative("hasValue");
            EditorGUI.PropertyField(setRect, hasValueProp, GUIContent.none);
            bool guiEnabled = GUI.enabled;
            GUI.enabled = guiEnabled && hasValueProp.boolValue;
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);
            GUI.enabled = guiEnabled;

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
