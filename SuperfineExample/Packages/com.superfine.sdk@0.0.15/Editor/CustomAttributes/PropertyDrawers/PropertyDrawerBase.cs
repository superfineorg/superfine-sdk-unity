using UnityEditor;
using UnityEngine;

namespace Superfine.Unity
{
    public abstract class PropertyDrawerBase : PropertyDrawer
    {
        public sealed override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            // Check if visible
            bool visible = PropertyUtility.IsVisible(property);
            if (!visible)
            {
                return;
            }

            // Check if enabled and draw
            EditorGUI.BeginChangeCheck();

            using (new EditorGUI.DisabledScope(disabled: false))
            {
                (GUIContent finalLabel, bool bold) = PropertyUtility.GetLabel(property);

                FontStyle origFontStyle = FontStyle.Normal;

                if (bold)
                {
                    origFontStyle = EditorStyles.label.fontStyle;
                    EditorStyles.label.fontStyle = FontStyle.Bold;
                }

                OnGUI_Internal(rect, property, finalLabel);

                if (bold)
                {
                    EditorStyles.label.fontStyle = origFontStyle;
                }
            }

            EditorGUI.EndChangeCheck();
        }

        protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);

        sealed override public float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool visible = PropertyUtility.IsVisible(property);
            if (!visible)
            {
                return 0.0f;
            }

            return GetPropertyHeight_Internal(property, label);
        }

        protected virtual float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, includeChildren: true);
        }

        protected float GetPropertyHeight(SerializedProperty property)
        {
            return EditorGUI.GetPropertyHeight(property, includeChildren: true);
        }

        public virtual float GetHelpBoxHeight()
        {
            return EditorGUIUtility.singleLineHeight * 2.0f;
        }

        public void DrawDefaultPropertyAndHelpBox(Rect rect, SerializedProperty property, string message, MessageType messageType)
        {
            float indentLength = SuperfineEditorGUI.GetIndentLength(rect);
            Rect helpBoxRect = new Rect(
                rect.x + indentLength,
                rect.y,
                rect.width - indentLength,
                GetHelpBoxHeight());

            SuperfineEditorGUI.HelpBox(helpBoxRect, message, MessageType.Warning, context: property.serializedObject.targetObject);

            Rect propertyRect = new Rect(
                rect.x,
                rect.y + GetHelpBoxHeight(),
                rect.width,
                GetPropertyHeight(property));

            EditorGUI.PropertyField(propertyRect, property, true);
        }
    }
}
