using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace Superfine.Unity
{
    [CustomPropertyDrawer(typeof(SuperfineSDKJSONData), true)]
    public class SuperfineSDKJSONDataDrawer : PropertyDrawer
    {
        public GUIContent iconToolbarPlus = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add child");
        public GUIContent iconToolbarPlusMore = EditorGUIUtility.TrIconContent("Toolbar Plus More", "Choose to add child");
        public GUIContent iconToolbarMinus = EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove");

        public readonly GUIStyle emptyHeaderBackground = "RL Empty Header";
        public readonly GUIStyle boxBackground = "RL Background";
        public readonly GUIStyle preButton = "RL FooterButton";

        private const float LIST_HEADER_PADDING = 2.0f;

        private const float LIST_BORDER_TOP = 2.0f;

        private const float LIST_OFFSET_TOP = 2.0f;
        private const float LIST_OFFSET_BOTTOM = 4.0f;
        private const float LIST_OFFSET_LEFT = 1.0f;
        private const float LIST_OFFSET_RIGHT = 1.0f;

        private const float CONTENT_OFFSET_LEFT = 6.0f;
        private const float CONTENT_OFFSET_RIGHT = 6.0f;

        private const float ARRAY_LABEL_SIZE = 50.0f;

        private const float ELEMENT_PADDING = 2.0f;

        private const float ELEMENT_SPLIT_WIDTH = 15.0f;

        private const float BUTTON_WIDTH = 25.0f;
        private const float BUTTON_HEIGHT = 16.0f;

        private const float BUTTON_PADDING = 6.0f;

        public GUIStyle buttonBackground = null;

        public readonly GUIStyle defaultLabel = new GUIStyle(EditorStyles.label);

        public readonly GUIStyle objectLabel = new GUIStyle(EditorStyles.boldLabel);
        public GUIStyle objectBackground = null;

        public GUIStyle numberFieldLabel = null;

        private ItemAddedData itemAddedData = null;

        private class ItemAddedData
        {
            public SerializedProperty property;
            public string path;
            public SimpleJSON.JSONNode node;
        }

        private void OnItemAdded(SerializedProperty property, string path, SimpleJSON.JSONNode node)
        {
            itemAddedData = null;

            if (node == null || property == null) return;

            itemAddedData = new ItemAddedData
            {
                property = property,
                path = path,
                node = node
            };
        }

        private SimpleJSON.JSONObject rootObject = null;

        private SerializedProperty property;

        private SerializedProperty rawTextProperty;

        private bool dirty = false;

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        private void InitStyles()
        {
            if (objectBackground == null)
            {
                objectBackground = new GUIStyle();
                objectBackground.normal.background = MakeTex(2, 2, new Color(0.0f, 0.5f, 0f, 0.5f));
            }

            if (buttonBackground == null)
            {
                buttonBackground = new GUIStyle();
                buttonBackground.normal.background = MakeTex(2, 2, new Color(0.5f, 0.5f, 0.5f, 0.5f));
            }

            if (numberFieldLabel == null)
            {
                numberFieldLabel = new GUIStyle(EditorStyles.textField);

                Color color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
                numberFieldLabel.normal.textColor = color;
                numberFieldLabel.focused.textColor = color;
                numberFieldLabel.active.textColor = color;
                numberFieldLabel.hover.textColor = color;
            }
        }

        private SimpleJSON.JSONNode FindJsonNode(SimpleJSON.JSONObject jsonObject, string path)
        {
            string propertyName;

            int pos = path.IndexOf('.');
            if (pos == -1)
            {
                propertyName = path;
                path = string.Empty;
            }
            else
            {
                propertyName = path.Substring(0, pos);
                path = path.Substring(pos + 1);
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            if (!jsonObject.ContainsKey(propertyName))
            {
                return null;
            }

            SimpleJSON.JSONNode node = jsonObject[propertyName];
            if (string.IsNullOrEmpty(path)) return node;

            if (node.IsObject)
            {
                return FindJsonNode((SimpleJSON.JSONObject)node, path);
            }
            else if (node.IsArray)
            {
                return FindJsonNode((SimpleJSON.JSONArray)node, path);
            }

            return null;
        }

        private SimpleJSON.JSONNode FindJsonNode(SimpleJSON.JSONArray jsonArray, string path)
        {
            string propertyName;

            int pos = path.IndexOf('.');
            if (pos == -1)
            {
                propertyName = path;
                path = string.Empty;
            }
            else
            {
                propertyName = path.Substring(0, pos);
                path = path.Substring(pos + 1);
            }

            if (string.IsNullOrEmpty(propertyName) || !int.TryParse(propertyName, out int index))
            {
                return null;
            }

            if (index < 0 || index >= jsonArray.Count)
            {
                return null;
            }

            SimpleJSON.JSONNode node = jsonArray[index];
            if (string.IsNullOrEmpty(path)) return node;

            if (node.IsObject)
            {
                return FindJsonNode((SimpleJSON.JSONObject)node, path);
            }
            else if (node.IsArray)
            {
                return FindJsonNode((SimpleJSON.JSONArray)node, path);
            }

            return null;
        }

        private void InitProperty(SerializedProperty value)
        {
            if (SerializedProperty.EqualContents(value, property)) return;

            property = value;
            rawTextProperty = property.FindPropertyRelative("rawText");
            rootObject = SuperfineSDKJSONData.GetJsonObject(rawTextProperty.stringValue);

            if (rootObject == null)
            {
                rootObject = new SimpleJSON.JSONObject();
                UpdateObject();
            }
        }

        private void UpdateObject()
        {
            if (rootObject == null)
            {
                rawTextProperty.stringValue = string.Empty;
            }
            else
            {
                rawTextProperty.stringValue = rootObject.ToString();
            }

            property.serializedObject.ApplyModifiedProperties();

            //AssetDatabase.SaveAssets();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            InitProperty(property);

            if (rootObject == null) return EditorGUIUtility.singleLineHeight;
            return GetJsonNodeHeight(rootObject);
        }

        private float GetJsonNodeHeight(SimpleJSON.JSONNode jsonNode)
        {
            if (jsonNode.IsObject)
            {
                return GetJsonObjectHeight((SimpleJSON.JSONObject)jsonNode);
            }
            else if (jsonNode.IsArray)
            {
                return GetJsonArrayHeight((SimpleJSON.JSONArray)jsonNode);
            }

            return EditorGUIUtility.singleLineHeight;
        }

        private float GetJsonObjectHeight(SimpleJSON.JSONObject jsonObject)
        {
            int count = jsonObject.Count;
            if (count == 0)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            float ret = EditorGUIUtility.singleLineHeight + LIST_BORDER_TOP + LIST_OFFSET_TOP + LIST_OFFSET_BOTTOM + LIST_HEADER_PADDING;
            foreach (KeyValuePair<string, SimpleJSON.JSONNode> pair in jsonObject)
            {
                ret += GetJsonNodeHeight(pair.Value) + ELEMENT_PADDING;
            }

            return ret;
        }

        private float GetJsonArrayHeight(SimpleJSON.JSONArray jsonArray)
        {
            int count = jsonArray.Count;
            if (count == 0)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            float ret = EditorGUIUtility.singleLineHeight + LIST_BORDER_TOP + LIST_OFFSET_TOP + LIST_OFFSET_BOTTOM + LIST_HEADER_PADDING;
            for (int i = 0; i < count; ++i)
            {
                ret += GetJsonNodeHeight(jsonArray[i]) + ELEMENT_PADDING;
            }

            return ret;
        }

        private string GetNewPropertyName(string baseName, SimpleJSON.JSONObject jsonObject)
        {
            if (!jsonObject.ContainsKey(baseName)) return baseName;

            int id = 1;

            do
            {
                string name = string.Format("{0}_{1}", baseName, id.ToString());
                if (!jsonObject.ContainsKey(name)) return name;

                id++;
            }
            while (id < 100);

            return null;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            InitStyles();

            InitProperty(property);

            if (rootObject == null) return;

            //EditorGUI.BeginProperty(rect, label, property);

            dirty = false;

            Rect initialRect = rect;
            DisplayJsonNode(initialRect, rootObject, string.Empty, property.displayName);

            if (itemAddedData != null)
            {
                if (SerializedProperty.EqualContents(itemAddedData.property, rawTextProperty))
                {
                    if (!dirty)
                    {
                        SimpleJSON.JSONNode node = string.IsNullOrEmpty(itemAddedData.path) ? rootObject : FindJsonNode(rootObject, itemAddedData.path);
                        if (node != null)
                        {
                            if (node.IsArray)
                            {
                                SimpleJSON.JSONArray jsonArray = (SimpleJSON.JSONArray)node;
                                jsonArray.Add(itemAddedData.node);
                                UpdateObject();

                            }
                            else if (node.IsObject)
                            {
                                SimpleJSON.JSONObject jsonObject = (SimpleJSON.JSONObject)node;

                                string propertyName = GetNewPropertyName("new", jsonObject);
                                if (!string.IsNullOrEmpty(propertyName))
                                {
                                    jsonObject.Add(propertyName, itemAddedData.node);
                                    UpdateObject();
                                }
                            }
                        }
                    }

                    itemAddedData = null;
                }
            }
                    
            //EditorGUI.EndProperty();
        }

        private bool ShowRemoveButton(Rect rect)
        {
            float buttonY = rect.y + (EditorGUIUtility.singleLineHeight - BUTTON_HEIGHT) * 0.5f;

            Rect buttonRect = new Rect(rect.xMax - BUTTON_WIDTH, buttonY, BUTTON_WIDTH, BUTTON_HEIGHT);

            if (Event.current.type == EventType.Repaint)
            {
                buttonBackground.Draw(buttonRect, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
            }

            return GUI.Button(buttonRect, iconToolbarMinus, preButton);
        }

        private void ShowAddButton(Rect rect, string path)
        {
            float buttonY = rect.y + (EditorGUIUtility.singleLineHeight - BUTTON_HEIGHT) * 0.5f;

            Rect buttonRect = new Rect(rect.xMax - BUTTON_WIDTH, buttonY, BUTTON_WIDTH, BUTTON_HEIGHT);

            if (Event.current.type == EventType.Repaint)
            {
                buttonBackground.Draw(buttonRect, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
            }

            if (GUI.Button(buttonRect, iconToolbarPlusMore, preButton))
            {
                GenericMenu menu = new GenericMenu();

                SerializedProperty property = rawTextProperty.Copy();

                menu.AddItem(new GUIContent("Object"), false, () =>
                {
                    OnItemAdded(property, path, new SimpleJSON.JSONObject());
                });

                menu.AddItem(new GUIContent("Array"), false, () =>
                {
                    OnItemAdded(property, path, new SimpleJSON.JSONArray());
                });

                menu.AddSeparator("");

                menu.AddItem(new GUIContent("String"), false, () =>
                {
                    OnItemAdded(property, path, new SimpleJSON.JSONString(string.Empty));
                });

                menu.AddItem(new GUIContent("Number"), false, () =>
                {
                    OnItemAdded(property, path, new SimpleJSON.JSONNumber(0.0));
                });

                menu.AddItem(new GUIContent("Bool"), false, () =>
                {
                    OnItemAdded(property, path, new SimpleJSON.JSONBool(false));
                });

                menu.ShowAsContext();
            }
        }

        private bool DisplayJsonNode(Rect rect, SimpleJSON.JSONNode jsonNode, string path, string overrideTitle = null)
        {
            if (jsonNode.IsObject)
            {
                return DisplayJsonObject(rect, (SimpleJSON.JSONObject)jsonNode, path, overrideTitle);
            }
            else if (jsonNode.IsArray)
            {
                return DisplayJsonArray(rect, (SimpleJSON.JSONArray)jsonNode, path, overrideTitle);
            }
            else if (jsonNode.IsString)
            {
                return DisplayJsonString(rect, (SimpleJSON.JSONString)jsonNode);
            }
            else if (jsonNode.IsBoolean)
            {
                return DisplayJsonBool(rect, (SimpleJSON.JSONBool)jsonNode);
            }
            else if (jsonNode.IsNumber)
            {
                return DisplayJsonNumber(rect, (SimpleJSON.JSONNumber)jsonNode);
            }
            else if (jsonNode.IsNull)
            {
                return DisplayJsonNull(rect);
            }

            return false;
        }

        private bool DisplayJsonString(Rect rect, SimpleJSON.JSONString jsonString)
        {
            Rect displayRect = new Rect(rect.x, rect.y, rect.width - BUTTON_WIDTH - BUTTON_PADDING, EditorGUIUtility.singleLineHeight);

            string value = jsonString.Value;

            string newValue = EditorGUI.DelayedTextField(displayRect, value);
            if (!dirty)
            {
                if (newValue != value)
                {
                    dirty = true;

                    jsonString.SetString(newValue);
                    UpdateObject();
                }
            }

            return ShowRemoveButton(rect);
        }

        private bool DisplayJsonBool(Rect rect, SimpleJSON.JSONBool jsonBool)
        {
            Rect displayRect = new Rect(rect.x, rect.y, rect.width - BUTTON_WIDTH - BUTTON_PADDING, EditorGUIUtility.singleLineHeight);

            bool value = jsonBool.AsBool;

            bool newValue = EditorGUI.Toggle(displayRect, value);
            if (!dirty)
            {
                if (newValue != value)
                {
                    dirty = true;

                    jsonBool.AsBool = newValue;
                    UpdateObject();
                }
            }

            return ShowRemoveButton(rect);
        }

        private bool DisplayJsonNumber(Rect rect, SimpleJSON.JSONNumber jsonNumber)
        {
            Rect displayRect = new Rect(rect.x, rect.y, rect.width - BUTTON_WIDTH - BUTTON_PADDING, EditorGUIUtility.singleLineHeight);

            double val = jsonNumber.AsDouble;

            double newVal = EditorGUI.DelayedDoubleField(displayRect, GUIContent.none, val, numberFieldLabel);
            if (!dirty)
            {
                if (Math.Abs(val - newVal) > 1e-7f)
                {
                    dirty = true;

                    jsonNumber.AsDouble = newVal;
                    UpdateObject();
                }
            }

            return ShowRemoveButton(rect);
        }

        private bool DisplayJsonNull(Rect rect)
        {
            Rect displayRect = new Rect(rect.x, rect.y, rect.width - BUTTON_WIDTH - BUTTON_PADDING, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(displayRect, "NULL");

            return ShowRemoveButton(rect);
        }

        private bool DisplayJsonObject(Rect rect, SimpleJSON.JSONObject jsonObject, string path, string overrideTitle = null)
        {
            bool deleted = false;

            Rect titleRect = new Rect(
                   rect.x,
                   rect.y,
                   rect.width,
                   EditorGUIUtility.singleLineHeight);

            if (Event.current.type == EventType.Repaint)
            {
                objectBackground.Draw(titleRect, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
            }

            if (!string.IsNullOrEmpty(overrideTitle))
            {
                EditorGUI.LabelField(titleRect, overrideTitle, objectLabel);
            }
            else
            {
                EditorGUI.LabelField(titleRect, "Object", objectLabel);
            }

            if (jsonObject != rootObject)
            {
                if (ShowRemoveButton(titleRect))
                {
                    deleted = true;
                }

                titleRect.xMax -= BUTTON_WIDTH + BUTTON_PADDING;
            }

            ShowAddButton(titleRect, path);

            int count = jsonObject.Count;
            if (count == 0) return deleted;

            rect.yMin += EditorGUIUtility.singleLineHeight + LIST_HEADER_PADDING;

            Rect itemsTopRect = new Rect(
                rect.x,
                rect.y,
                rect.width,
                LIST_BORDER_TOP);

            Rect itemsRect = new Rect(
                rect.x,
                rect.y + LIST_BORDER_TOP,
                rect.width,
                rect.height - LIST_BORDER_TOP);

            if (Event.current.type == EventType.Repaint)
            {
                emptyHeaderBackground.Draw(itemsTopRect, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
                boxBackground.Draw(itemsRect, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
            }

            itemsRect.yMin += LIST_OFFSET_TOP;
            itemsRect.yMax -= LIST_OFFSET_BOTTOM;

            itemsRect.xMin += LIST_OFFSET_LEFT + CONTENT_OFFSET_LEFT;
            itemsRect.xMax -= LIST_OFFSET_RIGHT + CONTENT_OFFSET_RIGHT;

            float labelWidth = EditorGUIUtility.labelWidth;
            float labelWidthRelative = labelWidth / rect.width;

            float offsetY = 0.0f;

            string propertyToRename = null;
            string propertyRenameKey = null;
            SimpleJSON.JSONNode propertyRenameValue = null;

            string propertyToDelete = null;

            ICollection<string> keys = jsonObject.Keys;

            foreach (KeyValuePair<string, SimpleJSON.JSONNode> pair in jsonObject)
            {
                string propertyName = pair.Key;
                SimpleJSON.JSONNode value = pair.Value;

                //float propertyNameWidth = GUI.skin.label.CalcSize(new GUIContent(propertyName)).x;
                float propertyNameWidth = labelWidth;

                float currentY = itemsRect.y + offsetY + ELEMENT_PADDING * 0.5f;

                Rect propertyNameRect = new Rect(
                    itemsRect.x,
                    currentY,
                    labelWidth - ELEMENT_SPLIT_WIDTH,
                    EditorGUIUtility.singleLineHeight);

                string newPropertyName = EditorGUI.DelayedTextField(propertyNameRect, propertyName);
                if (!dirty && !deleted)
                {
                    if (!string.IsNullOrEmpty(newPropertyName) && newPropertyName != propertyName && !keys.Contains(newPropertyName))
                    {
                        propertyToRename = propertyName;

                        propertyRenameKey = newPropertyName;
                        propertyRenameValue = value;

                        dirty = true;
                    }
                }

                float itemHeight = GetJsonNodeHeight(value);

                Rect nextRect = new Rect(
                    itemsRect.x + propertyNameWidth,
                    currentY,
                    itemsRect.width - propertyNameWidth,
                    itemHeight);

                EditorGUIUtility.labelWidth = nextRect.width * labelWidthRelative;
                if (DisplayJsonNode(nextRect, value, (!string.IsNullOrEmpty(path) ? (path + ".") : string.Empty) + propertyName))
                {
                    if (!dirty && !deleted)
                    {
                        propertyToDelete = propertyName;
                        dirty = true;
                    }
                }

                offsetY += itemHeight + ELEMENT_PADDING;
            }

            EditorGUIUtility.labelWidth = labelWidth;

            if (!string.IsNullOrEmpty(propertyToRename))
            {
                jsonObject.Remove(propertyToRename);
                jsonObject.Add(propertyRenameKey, propertyRenameValue);

                UpdateObject();
            }

            if (!string.IsNullOrEmpty(propertyToDelete))
            {
                jsonObject.Remove(propertyToDelete);
                UpdateObject();
            }

            return deleted;
        }

        private bool DisplayJsonArray(Rect rect, SimpleJSON.JSONArray jsonArray, string path, string overrideTitle = null)
        {
            bool deleted = false;

            Rect titleRect = new Rect(
                   rect.x,
                   rect.y,
                   rect.width,
                   EditorGUIUtility.singleLineHeight);

            if (Event.current.type == EventType.Repaint)
            {
                objectBackground.Draw(titleRect, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
            }

            if (!string.IsNullOrEmpty(overrideTitle))
            {
                EditorGUI.LabelField(titleRect, overrideTitle, objectLabel);
            }
            else
            {
                EditorGUI.LabelField(titleRect, "Array", objectLabel);
            }

            if (ShowRemoveButton(titleRect))
            {
                deleted = true;
            }

            titleRect.xMax -= BUTTON_WIDTH + BUTTON_PADDING;

            ShowAddButton(titleRect, path);

            int count = jsonArray.Count;
            if (count == 0) return deleted;

            rect.yMin += EditorGUIUtility.singleLineHeight + LIST_HEADER_PADDING;

            Rect itemsTopRect = new Rect(
                rect.x,
                rect.y,
                rect.width,
                LIST_BORDER_TOP);

            Rect itemsRect = new Rect(
                rect.x,
                rect.y + LIST_BORDER_TOP,
                rect.width,
                rect.height - LIST_BORDER_TOP);

            if (Event.current.type == EventType.Repaint)
            {
                emptyHeaderBackground.Draw(itemsTopRect, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
                boxBackground.Draw(itemsRect, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
            }

            itemsRect.yMin += LIST_OFFSET_TOP;
            itemsRect.yMax -= LIST_OFFSET_BOTTOM;

            itemsRect.xMin += LIST_OFFSET_LEFT + CONTENT_OFFSET_LEFT;
            itemsRect.xMax -= LIST_OFFSET_RIGHT + CONTENT_OFFSET_RIGHT;

            float labelWidth = EditorGUIUtility.labelWidth;
            float labelWidthRelative = labelWidth / rect.width;

            float offsetY = 0.0f;

            int indexToDelete = -1;

            for (int i = 0; i < count; ++i)
            {
                string propertyName = "[" + i.ToString() + "]";

                float propertyNameWidth = ARRAY_LABEL_SIZE;

                float currentY = itemsRect.y + offsetY + ELEMENT_PADDING * 0.5f;

                Rect propertyNameRect = new Rect(
                    itemsRect.x,
                    currentY,
                    propertyNameWidth,
                    EditorGUIUtility.singleLineHeight);

                EditorGUI.LabelField(propertyNameRect, propertyName);

                SimpleJSON.JSONNode value = jsonArray[i];

                float itemHeight = GetJsonNodeHeight(value);

                Rect nextRect = new Rect(
                    itemsRect.x + propertyNameWidth,
                    currentY,
                    itemsRect.width - propertyNameWidth,
                    itemHeight);

                EditorGUIUtility.labelWidth = nextRect.width * labelWidthRelative;

                if (DisplayJsonNode(nextRect, value, (!string.IsNullOrEmpty(path) ? (path + ".") : string.Empty) + i.ToString()))
                {
                    if (!dirty && !deleted)
                    {
                        indexToDelete = i;
                        dirty = true;
                    }
                }
                offsetY += itemHeight + ELEMENT_PADDING;
            }

            EditorGUIUtility.labelWidth = labelWidth;

            if (indexToDelete >= 0)
            {
                jsonArray.Remove(indexToDelete);
                UpdateObject();
            }

            return deleted;
        }
    }
}