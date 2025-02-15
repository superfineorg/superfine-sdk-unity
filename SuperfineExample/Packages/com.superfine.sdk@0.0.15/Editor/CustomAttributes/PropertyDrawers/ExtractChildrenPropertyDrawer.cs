using UnityEngine;
using UnityEditor;

namespace Superfine.Unity
{
    [CustomPropertyDrawer(typeof(ExtractChildrenAttribute))]
    public class ExtractChildrenPropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 0.0f;

            int depth = property.depth;

            foreach (SerializedProperty childProperty in property)
            {
                if (childProperty == null || childProperty.depth > depth + 1) continue;

                bool visible = PropertyUtility.IsVisible(childProperty);
                if (!visible)
                {
                    continue;
                }

                float height = GetPropertyHeight(childProperty);
                totalHeight += height;
            }

            return totalHeight;
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            DrawChildProperties(rect, property);
            EditorGUI.EndProperty();
        }

        private void DrawChildProperties(Rect rect, SerializedProperty property)
        {
            float totalHeight = 0.0f;

            int depth = property.depth;

            foreach (SerializedProperty childProperty in property)
            {
                if (childProperty == null || childProperty.depth > depth + 1) continue;

                bool visible = PropertyUtility.IsVisible(childProperty);
                if (!visible)
                {
                    continue;
                }

                float height = GetPropertyHeight(childProperty);

                Rect childRect = new Rect()
                {
                    x = rect.x,
                    y = rect.y + totalHeight,
                    width = rect.width,
                    height = height
                };

                SuperfineEditorGUI.PropertyField(childRect, childProperty, true);

                totalHeight += height;
            }
        }
    }
}
