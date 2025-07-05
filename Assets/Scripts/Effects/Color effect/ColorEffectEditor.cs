using UnityEditor;
using UnityEngine;
using Yosf.UI;

namespace Yosf.UI
{
    [CustomPropertyDrawer(typeof(ColorEffect))]
    public class ColorEffectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty selectedColorIndexProp = property.FindPropertyRelative("selectedColorIndex");
            SerializedProperty colorsProp = property.FindPropertyRelative("colors");
            SerializedProperty colorProp = property.FindPropertyRelative("customColor");
            SerializedProperty durationProp = property.FindPropertyRelative("Duration");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float padding = EditorGUIUtility.standardVerticalSpacing;
            Rect currentRect = new Rect(position.x, position.y, position.width, lineHeight);

            if (colorsProp != null && colorsProp.arraySize > 0)
            {
                string[] colorOptions = new string[colorsProp.arraySize];
                for (int i = 0; i < colorsProp.arraySize; i++)
                {
                    colorOptions[i] = colorsProp.GetArrayElementAtIndex(i).stringValue;
                }

                selectedColorIndexProp.intValue = EditorGUI.Popup(currentRect, "Select Color", selectedColorIndexProp.intValue, colorOptions);

                if (selectedColorIndexProp.intValue == 3)
                {
                    currentRect.y += lineHeight + padding;
                    EditorGUI.PropertyField(currentRect, colorProp, new GUIContent("Custom Color"));
                }

                currentRect.y += lineHeight + padding;
                EditorGUI.PropertyField(currentRect, durationProp);
            }
            else
            {
                EditorGUI.LabelField(currentRect, "No colors available. Please add colors to the ColorEffect.");
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var selectedColorIndexProp = property.FindPropertyRelative("selectedColorIndex");

            int lineCount = 2;

            if (selectedColorIndexProp != null && selectedColorIndexProp.intValue == 3)
            {
                lineCount++;
            }

            return lineCount * EditorGUIUtility.singleLineHeight + (lineCount - 1) * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
