using UnityEditor;
using UnityEngine;

namespace Yosf.UI
{
    [CustomEditor(typeof(YoButton))]
    public class YoButtonEditor : Editor
    {
        #region Private Fields

        private SerializedProperty m_graphicdTarget;
        private SerializedProperty m_buttonTheme;
        
        // List of effects
        private SerializedProperty m_onHoverEffectsProp;
        private SerializedProperty m_onClickEffectsProp;
        private SerializedProperty m_onSelectEffectsProp;

        // Selected effect index for adding new effects
        private int m_selectedEffectIndex;

        private bool m_onHoverFoldout;
        private bool m_onClickFoldout;
        private bool m_onSelectFoldout;
        private bool m_eventsFoldout;
        
        private YoButton m_button;
        
        #endregion
        
        // Find the serialized properties when the editor is enabled
        private void OnEnable()
        {
            m_graphicdTarget = serializedObject.FindProperty("GraphicTarget");
            m_buttonTheme = serializedObject.FindProperty("buttonThemeData");
            
            // Initialize the effect properties
            m_onHoverEffectsProp = serializedObject.FindProperty("m_onHoverEffects");
            m_onClickEffectsProp = serializedObject.FindProperty("m_onClickEffects");
            m_onSelectEffectsProp = serializedObject.FindProperty("m_onSelectEffects");
            
            // Get the target object 
            m_button = (YoButton)target;
        }

        // Override the OnInspectorGUI method to customize the inspector
        public override void OnInspectorGUI()
        {
            // Get the effect types from the button
            string[] effectType = m_button.effectType;
            
            // Update the changes in the inspector
            serializedObject.Update();

            #region Settings
            
            // Draw the settings section header
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_graphicdTarget, new GUIContent("Graphic Target", "The graphic component that this button will affect. Leave empty to use the default graphic of the button."));

            m_button.UpdateGraphicTarget();
            m_button.UpdateTheme();
            
            #endregion
            
            #region Theme
            
            int currentThemeIndex = m_button.selectedThemeIndex;
            
            EditorGUILayout.Space(10);
            // Draw the effects section header
            EditorGUILayout.LabelField("Theme", EditorStyles.boldLabel);
            
            m_button.selectedThemeIndex = EditorGUILayout.Popup("Type", m_button.selectedThemeIndex, new[] { "Default Theme", "Manager feed", "Custom Theme" });
            
            // Draw a custom theme field if the selected theme is "Custom Theme"
            if (m_button.selectedThemeIndex == 2)
            {
                EditorGUILayout.PropertyField(m_buttonTheme);
            }
            
            // Automatically apply the theme if the theme type has changed
            if (m_button.selectedThemeIndex != currentThemeIndex)
            {
                m_button.ApplyTheme();
            }
            
            #endregion

            #region Effect
                
                EditorGUILayout.Space(10);
                // Draw the effects section header
                EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);
                
                DrawEffectList("On Hover", ref m_onHoverFoldout, m_onHoverEffectsProp, effectType);
                DrawEffectList("On Click", ref m_onClickFoldout, m_onClickEffectsProp, effectType);
                DrawEffectList("On Select", ref m_onSelectFoldout, m_onSelectEffectsProp, effectType);
                
            #endregion
            
            EditorGUILayout.Space(10);
            
            // Draw the events section foldout
            m_eventsFoldout = EditorGUILayout.Foldout(m_eventsFoldout, "Events");
            if(m_eventsFoldout)
                // Draw the rest of the properties
                DrawPropertiesExcluding(serializedObject, "m_Script","GraphicTarget" , "buttonThemeData", "m_onHoverEffects", "m_onClickEffects", "m_onSelectEffects");
            
            // Apply the modified properties to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
        
        /// <summary>
        /// Draws a list of effects with a foldout for each effect group.
        /// </summary>
        /// <param name="label">The label for the foldout.</param>
        /// <param name="foldout">Reference to the foldout state.</param>
        /// <param name="effectsProp">SerializedProperty for the effects list.</param>
        /// <param name="effectType">Array of effect types to display in the popup.</param>
        private void DrawEffectList(string label, ref bool foldout, SerializedProperty effectsProp, string[] effectType)
        {
            // Foldout for this effect group
            foldout = EditorGUILayout.Foldout(foldout, label);

            // If the foldout is expanded, draw the effects
            if (foldout)
            {
                
                // Begin a box for the effect group
                EditorGUILayout.BeginVertical("helpBox");
                
                // Check if the effectsProp is not null, to avoid null effects list
                if (effectsProp == null)
                {
                    EditorGUILayout.HelpBox("Effects property is missing.", MessageType.Error);
                    EditorGUILayout.EndVertical();
                    return;
                }
                
                // Display the list of effects that was added.
                for (int i = 0; i < effectsProp.arraySize; i++)
                {
                    // Get the current effect property for the loop index
                    SerializedProperty effectProp = effectsProp.GetArrayElementAtIndex(i);

                    EditorGUILayout.BeginVertical("box");
                    {
                        // Display the effect property field with a label
                        EditorGUILayout.PropertyField(effectProp, new GUIContent($"Effect {i + 1}"), true);

                        // If the remove button is clicked, remove the effect from the list
                        if (GUILayout.Button("Remove Effect"))
                        {
                            effectsProp.DeleteArrayElementAtIndex(i);
                        }
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
                
                EditorGUILayout.Space(10);
                
                // Effect type selection and add button
                if (effectType?.Length > 0)
                {
                    // Display popup to select effect type
                    m_selectedEffectIndex = EditorGUILayout.Popup("Type", m_selectedEffectIndex, effectType);

                    // Button to add a new effect of the selected type
                    if (GUILayout.Button($"Add {effectType[m_selectedEffectIndex]}"))
                    {
                        effectsProp.arraySize++;
                        // Get the last element in the array to set the new effect
                        var newProp = effectsProp.GetArrayElementAtIndex(effectsProp.arraySize - 1);

                        switch (m_selectedEffectIndex)
                        {
                            case 0:
                                newProp.managedReferenceValue = new ScaleEffect();
                                break;
                            case 1:
                                newProp.managedReferenceValue = new ColorEffect();
                                break;
                            case 2:
                                newProp.managedReferenceValue = new AnimationEffect();
                                break;
                            default:
                                Debug.LogWarning("Unknown effect type selected.");
                                return;
                        }

                        GUI.FocusControl(null);
                        EditorUtility.SetDirty(target);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("No effect types defined. Please define effect types in the YoButton script.", MessageType.Warning);
                }
                
                EditorGUILayout.EndVertical();
            }
        }

    }
}
