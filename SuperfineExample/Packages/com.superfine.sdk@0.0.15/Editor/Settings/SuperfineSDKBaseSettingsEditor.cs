using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Superfine.Unity
{
    public class SuperfineSDKBaseSettingsEditor : Editor
    {
        private const float FOLDOUT_PADDING = 10.0f;
        private const float BOXGROUP_PADDING = 10.0f;

        private List<SerializedProperty> _serializedProperties = new List<SerializedProperty>();

        private Dictionary<string, SavedBool> _foldouts = new Dictionary<string, SavedBool>();

        private Object backupObject = null;
        private SerializedObject backupSerializedObject = null;

        protected virtual void OnEnable()
        {
        #if UNITY_2022_2_OR_NEWER
            saveChangesMessage = "This editor has unsaved changes. Would you like to save?";
        #endif
            DestroyBackupObject();
        }

        protected virtual void OnDisable()
        {
            DestroyBackupObject();
        }

        private void DestroyBackupObject()
        {
            if (backupObject != null)
            {
                DestroyImmediate(backupObject);
                backupObject = null;
            }

            if (backupSerializedObject != null)
            {
                backupSerializedObject.Dispose();
                backupSerializedObject = null;
            }
        }

        private bool SerializedObjectEquals(SerializedObject obj1, SerializedObject obj2)
        {
            HashSet<string> pathSet = new HashSet<string>();

            using (var iterator = obj1.GetIterator())
            {
                if (iterator.NextVisible(true))
                {
                    do
                    {
                        if (iterator.name.Equals("m_Script", System.StringComparison.Ordinal)) continue;

                        string path = iterator.propertyPath;
                        pathSet.Add(path);

                        SerializedProperty property = obj2.FindProperty(iterator.propertyPath);
                        if (property == null)
                        { 
                            return false;
                        }

                        if (!SerializedProperty.DataEquals(property, iterator))
                        {
                            return false;
                        }
                    }
                    while (iterator.NextVisible(false));
                }
            }

            using (var iterator = obj2.GetIterator())
            {
                if (iterator.NextVisible(true))
                {
                    do
                    {
                        if (iterator.name.Equals("m_Script", System.StringComparison.Ordinal)) continue;

                        if (!pathSet.Contains(iterator.propertyPath))
                        {
                            return false;
                        }
                    }
                    while (iterator.NextVisible(false));
                }
            }

            return true;
        }

        public override void OnInspectorGUI()
        {
            if (backupObject == null)
            {
                backupObject = Instantiate(serializedObject.targetObject);
                backupObject.name = serializedObject.targetObject.name;

                backupSerializedObject = new SerializedObject(backupObject);
            }

            GetSerializedProperties(ref _serializedProperties);

            bool anyCustomAttribute = _serializedProperties.Any(p => PropertyUtility.GetAttribute<ICustomAttribute>(p) != null);
            if (!anyCustomAttribute)
            {
                DrawDefaultInspector();
            }
            else
            {
                DrawSerializedProperties();
            }

            bool dirty = !SerializedObjectEquals(serializedObject, backupSerializedObject);

#if UNITY_2022_2_OR_NEWER
            hasUnsavedChanges = dirty;

            using (new EditorGUI.DisabledScope(!hasUnsavedChanges))
            {
                GUILayout.Space(20);

                if (GUILayout.Button("Save"))
                {
                    SaveChanges();
                }

                if (GUILayout.Button("Discard"))
                {
                    DiscardChanges();
                }
            }
#else
            if (dirty)
            {
#if UNITY_2021_3_OR_NEWER
                AssetDatabase.SaveAssetIfDirty(serializedObject.targetObject);
#else
                EditorUtility.SetDirty(serializedObject.targetObject);
                AssetDatabase.SaveAssets();
#endif
                AssetDatabase.Refresh();

                DestroyBackupObject();

                Debug.Log("Saved changes!!!");
            }
#endif
        }

#if UNITY_2022_2_OR_NEWER
        public override void SaveChanges()
        {
            AssetDatabase.SaveAssetIfDirty(serializedObject.targetObject);
            AssetDatabase.Refresh();

            DestroyBackupObject();

            Debug.Log("Saved changes!!!");
            base.SaveChanges();
        }

        public override void DiscardChanges()
        {
            if (backupObject == null) return;

            EditorUtility.CopySerialized(backupObject, serializedObject.targetObject);

            //AssetDatabase.SaveAssetIfDirty(serializedObject.targetObject);
            //AssetDatabase.Refresh();

            DestroyBackupObject();

            Debug.Log("Discarded changes!!!");
            base.DiscardChanges();
        }  
#endif

        protected void GetSerializedProperties(ref List<SerializedProperty> outSerializedProperties)
        {
            outSerializedProperties.Clear();
            using (var iterator = serializedObject.GetIterator())
            {
                if (iterator.NextVisible(true))
                {
                    do
                    {
                        outSerializedProperties.Add(serializedObject.FindProperty(iterator.name));
                    }
                    while (iterator.NextVisible(false));
                }
            }
        }

        protected bool DrawSerializedProperties()
        {
            EditorGUI.BeginChangeCheck();

            serializedObject.Update();

            // Draw non-grouped serialized properties
            foreach (var property in GetNonGroupedProperties(_serializedProperties))
            {
                if (property.name.Equals("m_Script", System.StringComparison.Ordinal))
                {
                    using (new EditorGUI.DisabledScope(disabled: true))
                    {
                        EditorGUILayout.PropertyField(property);
                    }
                }
                else
                {
                    SuperfineEditorGUI.PropertyField_Layout(property, includeChildren: true);
                }
            }

            // Draw grouped serialized properties
            foreach (var group in GetBoxGroupProperties(_serializedProperties))
            {
                IEnumerable<SerializedProperty> visibleProperties = group.Where(p => PropertyUtility.IsVisible(p));
                if (!visibleProperties.Any())
                {
                    continue;
                }

                if (BOXGROUP_PADDING > 0)
                {
                    EditorGUILayout.Space(BOXGROUP_PADDING);
                }

                SuperfineEditorGUI.BeginBoxGroup_Layout(group.Key);

                foreach (var property in visibleProperties)
                {
                    int indent = 0;
                    BoxGroupAttribute boxGroupAttribute = PropertyUtility.GetAttribute<BoxGroupAttribute>(property);
                    if (boxGroupAttribute != null) indent = boxGroupAttribute.Indent;

                    if (indent > 0) EditorGUI.indentLevel += indent;
                    SuperfineEditorGUI.PropertyField_Layout(property, includeChildren: true);
                    if (indent > 0) EditorGUI.indentLevel -= indent;
                }

                SuperfineEditorGUI.EndBoxGroup_Layout();
            }

            // Draw foldout serialized properties
            foreach (var group in GetFoldoutProperties(_serializedProperties))
            {
                IEnumerable<SerializedProperty> visibleProperties = group.Where(p => PropertyUtility.IsVisible(p));
                if (!visibleProperties.Any())
                {
                    continue;
                }

                if (FOLDOUT_PADDING > 0)
                {
                    EditorGUILayout.Space(FOLDOUT_PADDING);
                }

                if (!_foldouts.ContainsKey(group.Key))
                {
                    _foldouts[group.Key] = new SavedBool($"{target.GetInstanceID()}.{group.Key}", false);
                }

                bool lastChanged = GUI.changed;
                _foldouts[group.Key].Value = EditorGUILayout.Foldout(_foldouts[group.Key].Value, group.Key, true, GetFoldoutGUIStyle());
                GUI.changed = lastChanged;

                if (_foldouts[group.Key].Value)
                {
                    EditorGUI.indentLevel++;

                    foreach (var property in GetNonBoxGroupsProperties(visibleProperties))
                    {
                        SuperfineEditorGUI.PropertyField_Layout(property, true);
                    }

                    foreach (var foldoutGroup in GetBoxGroupProperties(visibleProperties, false))
                    {
                        IEnumerable<SerializedProperty> foldoutVisibleProperties = foldoutGroup.Where(p => PropertyUtility.IsVisible(p));
                        if (!foldoutVisibleProperties.Any())
                        {
                            continue;
                        }

                        if (BOXGROUP_PADDING > 0)
                        {
                            EditorGUILayout.Space(BOXGROUP_PADDING);
                        }

                        SuperfineEditorGUI.BeginBoxGroup_Layout(foldoutGroup.Key, GetInnerBoxGroupGUIStyle());

                        foreach (var property in foldoutVisibleProperties)
                        {
                            int indent = 0;
                            BoxGroupAttribute boxGroupAttribute = PropertyUtility.GetAttribute<BoxGroupAttribute>(property);
                            if (boxGroupAttribute != null) indent = boxGroupAttribute.Indent;

                            if (indent > 0) EditorGUI.indentLevel += indent;
                            SuperfineEditorGUI.PropertyField_Layout(property, includeChildren: true);
                            if (indent > 0) EditorGUI.indentLevel -= indent;
                        }

                        SuperfineEditorGUI.EndBoxGroup_Layout();
                    }

                    /*
                    foreach (var property in visibleProperties)
                    {
                        SuperfineEditorGUI.PropertyField_Layout(property, true);
                    }
                    */

                    EditorGUI.indentLevel--;
                }
            }

            serializedObject.ApplyModifiedProperties();

            return EditorGUI.EndChangeCheck();
        }

        private static IEnumerable<SerializedProperty> GetNonGroupedProperties(IEnumerable<SerializedProperty> properties)
        {
            return properties.Where(p => PropertyUtility.GetAttribute<IGroupAttribute>(p) == null);
        }

        private static IEnumerable<SerializedProperty> GetNonBoxGroupsProperties(IEnumerable<SerializedProperty> properties)
        {
            return properties.Where(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p) == null);
        }

        private static IEnumerable<IGrouping<string, SerializedProperty>> GetBoxGroupProperties(IEnumerable<SerializedProperty> properties, bool nonFoldout = true)
        {
            return properties
                .Where(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p) != null && (!nonFoldout || PropertyUtility.GetAttribute<FoldoutAttribute>(p) == null))
                .GroupBy(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p).Name);
        }

        private static IEnumerable<IGrouping<string, SerializedProperty>> GetFoldoutProperties(IEnumerable<SerializedProperty> properties)
        {
            return properties
                .Where(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p) != null)
                .GroupBy(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p).Name);
        }

        private static GUIStyle GetHeaderGUIStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.UpperCenter;

            return style;
        }

        private static GUIStyle GetInnerBoxGroupGUIStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);

            Color col = new Color(0f / 255f, 190f / 255f, 190f / 255f);
            style.normal.textColor = col;
            return style;
        }

        private static GUIStyle GetFoldoutGUIStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.foldout);

            Color normalCol = new Color(255f / 255f, 255f / 255f, 190f / 255f);
            style.normal.textColor = normalCol;

            Color expandedCol = new Color(255f / 255f, 255f / 255f, 64f / 255f);
            style.onNormal.textColor = expandedCol;

            style.fontStyle = FontStyle.Bold;

            return style;
        }
    }
}
