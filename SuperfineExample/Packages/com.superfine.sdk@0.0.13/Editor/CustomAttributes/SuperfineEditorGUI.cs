using UnityEditor;
using UnityEngine;

namespace Superfine.Unity
{
    public static class SuperfineEditorGUI
    {
        public const float IndentLength = 15.0f;
        public const float HorizontalSpacing = 2.0f;

        private delegate void PropertyFieldFunction(Rect rect, SerializedProperty property, GUIContent label, bool includeChildren);

        public static float GetIndentLength(Rect sourceRect)
        {
            Rect indentRect = EditorGUI.IndentedRect(sourceRect);
            float indentLength = indentRect.x - sourceRect.x;

            return indentLength;
        }

        public static void BeginBoxGroup_Layout(string label = "", GUIStyle style = null)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            if (!string.IsNullOrEmpty(label) && !label.StartsWith("#"))
            {
                EditorGUILayout.LabelField(label, style == null ? EditorStyles.boldLabel : style);
            }
        }

        public static void EndBoxGroup_Layout()
        {
            EditorGUILayout.EndVertical();
        }

        public static void HorizontalLine(Rect rect, float height, Color color)
        {
            rect.height = height;
            EditorGUI.DrawRect(rect, color);
        }

        public static void HelpBox(Rect rect, string message, MessageType type, UnityEngine.Object context = null, bool logToConsole = false)
        {
            EditorGUI.HelpBox(rect, message, type);

            if (logToConsole)
            {
                DebugLogMessage(message, type, context);
            }
        }

        public static void HelpBox_Layout(string message, MessageType type, UnityEngine.Object context = null, bool logToConsole = false)
        {
            EditorGUILayout.HelpBox(message, type);

            if (logToConsole)
            {
                DebugLogMessage(message, type, context);
            }
        }

        private static void DebugLogMessage(string message, MessageType type, UnityEngine.Object context)
        {
            switch (type)
            {
                case MessageType.None:
                case MessageType.Info:
                    Debug.Log(message, context);
                    break;
                case MessageType.Warning:
                    Debug.LogWarning(message, context);
                    break;
                case MessageType.Error:
                    Debug.LogError(message, context);
                    break;
            }
        }

        public static void PropertyField(Rect rect, SerializedProperty property, bool includeChildren)
        {
            PropertyField_Implementation(rect, property, includeChildren, DrawPropertyField);
        }

        public static void PropertyField_Layout(SerializedProperty property, bool includeChildren)
        {
            Rect dummyRect = new Rect();
            PropertyField_Implementation(dummyRect, property, includeChildren, DrawPropertyField_Layout);
        }

        private static void DrawPropertyField(Rect rect, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            EditorGUI.PropertyField(rect, property, label, includeChildren);
        }

        private static void DrawPropertyField_Layout(Rect rect, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            EditorGUILayout.PropertyField(property, label, includeChildren);
        }

        private static void PropertyField_Implementation(Rect rect, SerializedProperty property, bool includeChildren, PropertyFieldFunction propertyFieldFunction)        {
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

                propertyFieldFunction.Invoke(rect, property, finalLabel, includeChildren);

                if (bold)
                {
                    EditorStyles.label.fontStyle = origFontStyle;
                }
            }

            EditorGUI.EndChangeCheck();
        }
    }
}
