using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

namespace Yosf.UI
{
    [CustomPropertyDrawer(typeof(AnimationEffect))]
    public class AnimationEffectPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Get the properties
            var triggerNamesProperty = property.FindPropertyRelative("triggerNames");
            var selectedTriggerIndexProperty = property.FindPropertyRelative("selectedTriggerIndex");
            var generateAnimationProperty = property.FindPropertyRelative("generateAnimation");

            // Check if properties were found
            if (triggerNamesProperty == null || selectedTriggerIndexProperty == null || generateAnimationProperty == null)
            {
                EditorGUI.LabelField(position, label.text, "Property not found - check field names");
                EditorGUI.EndProperty();
                return;
            }

            // Calculate rects
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            
            Rect foldoutRect = new Rect(position.x, position.y, position.width, lineHeight);
            
            // Foldout
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);
            
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                
                float currentY = position.y + lineHeight + spacing;
                
                // Draw trigger names array manually to avoid recursion
                Rect arrayLabelRect = new Rect(position.x, currentY, position.width, lineHeight);
                EditorGUI.LabelField(arrayLabelRect, "Trigger Names");
                currentY += lineHeight + spacing;
                
                EditorGUI.indentLevel++;
                
                // Array size field
                Rect arraySizeRect = new Rect(position.x, currentY, position.width, lineHeight);
                int newSize = EditorGUI.IntField(arraySizeRect, "Size", triggerNamesProperty.arraySize);
                if (newSize != triggerNamesProperty.arraySize)
                {
                    triggerNamesProperty.arraySize = newSize;
                }
                currentY += lineHeight + spacing;
                
                // Draw array elements
                for (int i = 0; i < triggerNamesProperty.arraySize; i++)
                {
                    Rect elementRect = new Rect(position.x, currentY, position.width, lineHeight);
                    var element = triggerNamesProperty.GetArrayElementAtIndex(i);
                    element.stringValue = EditorGUI.TextField(elementRect, $"Element {i}", element.stringValue);
                    currentY += lineHeight + spacing;
                }
                
                EditorGUI.indentLevel--;
                
                // Selected Trigger Index Popup
                if (triggerNamesProperty.arraySize > 0)
                {
                    string[] triggerNames = new string[triggerNamesProperty.arraySize];
                    for (int i = 0; i < triggerNamesProperty.arraySize; i++)
                    {
                        var element = triggerNamesProperty.GetArrayElementAtIndex(i);
                        triggerNames[i] = string.IsNullOrEmpty(element.stringValue) ? $"Element {i}" : element.stringValue;
                    }
                    
                    Rect popupRect = new Rect(position.x, currentY, position.width, lineHeight);
                    
                    // Clamp the selected index to valid range
                    if (selectedTriggerIndexProperty.intValue >= triggerNames.Length)
                    {
                        selectedTriggerIndexProperty.intValue = 0;
                    }
                    
                    selectedTriggerIndexProperty.intValue = EditorGUI.Popup(
                        popupRect, 
                        "Selected Trigger", 
                        selectedTriggerIndexProperty.intValue, 
                        triggerNames
                    );
                    currentY += lineHeight + spacing;
                }
                
                // Generate Animation checkbox
                Rect generateRect = new Rect(position.x, currentY, position.width, lineHeight);
                generateAnimationProperty.boolValue = EditorGUI.Toggle(generateRect, "Generate Animation", generateAnimationProperty.boolValue);
                currentY += lineHeight + spacing;
                
                // Generate Button
                Rect buttonRect = new Rect(position.x, currentY, position.width, lineHeight * 1.5f);
                GUI.backgroundColor = Color.green;
                if (GUI.Button(buttonRect, "Generate Animator Controller"))
                {
                    GenerateAnimatorController(property);
                }
                GUI.backgroundColor = Color.white;
                
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var triggerNamesProperty = property.FindPropertyRelative("triggerNames");
            
            if (triggerNamesProperty == null || !property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            float height = EditorGUIUtility.singleLineHeight; // Foldout
            height += EditorGUIUtility.standardVerticalSpacing;
            
            // Trigger names array
            height += EditorGUIUtility.singleLineHeight; // Array label
            height += EditorGUIUtility.standardVerticalSpacing;
            height += EditorGUIUtility.singleLineHeight; // Size field
            height += EditorGUIUtility.standardVerticalSpacing;
            
            if (triggerNamesProperty != null)
            {
                // Array elements
                for (int i = 0; i < triggerNamesProperty.arraySize; i++)
                {
                    height += EditorGUIUtility.singleLineHeight;
                    height += EditorGUIUtility.standardVerticalSpacing;
                }
                
                // Selected trigger popup
                if (triggerNamesProperty.arraySize > 0)
                {
                    height += EditorGUIUtility.singleLineHeight;
                    height += EditorGUIUtility.standardVerticalSpacing;
                }
            }
            
            // Generate animation checkbox
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;
            
            // Generate button
            height += EditorGUIUtility.singleLineHeight * 1.5f;
            
            return height;
        }

        private void GenerateAnimatorController(SerializedProperty property)
        {
            // Get the component that contains this AnimationEffect
            var targetObject = property.serializedObject.targetObject;
            GameObject graphicTarget = null;
            
            // If the target object is a MonoBehaviour, use its GameObject
            if (targetObject is MonoBehaviour monoBehaviour)
            {
                graphicTarget = monoBehaviour.gameObject;
            }

            // Let user choose where to save the file
            string defaultFileName = $"{graphicTarget.name}_AnimatorController.controller";
            string fullPath = EditorUtility.SaveFilePanel(
                "Save Animator Controller",
                "Assets",
                defaultFileName,
                "controller"
            );

            if (string.IsNullOrEmpty(fullPath))
            {
                return; // User cancelled
            }

            // Convert absolute path to relative path for Unity
            if (fullPath.StartsWith(Application.dataPath))
            {
                fullPath = "Assets" + fullPath.Substring(Application.dataPath.Length);
            }

            try
            {
                // Generate the animator controller
                AnimatorController controller = AnimatorGenerator.GenerateAnimator(fullPath);

                if (controller != null)
                {
                    // Get or add Animator component
                    Animator animator = graphicTarget.GetComponent<Animator>();
                    if (animator == null)
                    {
                        animator = graphicTarget.AddComponent<Animator>();
                    }

                    // Assign the controller
                    animator.runtimeAnimatorController = controller;

                    // Mark the scene as dirty
                    EditorUtility.SetDirty(graphicTarget);
                    
                    // Show success message
                    EditorUtility.DisplayDialog(
                        "Success", 
                        $"Animator Controller generated successfully!\n\nPath: {fullPath}\n\nThe controller has been automatically assigned to the target GameObject.", 
                        "OK"
                    );

                    // Ping the created asset in the project window
                    EditorGUIUtility.PingObject(controller);
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Failed to generate Animator Controller.", "OK");
                }
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error", $"An error occurred while generating the Animator Controller:\n\n{e.Message}", "OK");
                Debug.LogError($"AnimatorGenerator Error: {e}");
            }
        }
    }
}