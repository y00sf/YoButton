using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Yosf.UI
{
    [Serializable]
    public class ColorEffect : YoEffect
    {
        public ThemeData themeData;
        public Color customColor;
        public float Duration = 0.1f; // Duration for the color effect
        public int selectedColorIndex;
        private bool isClickEffect;
        
        private Color m_lastColor; // Store the last color to revert back after the effect
        YoButton button;
        
        [SerializeField] public String[] colors = 
        {
            "OnHoverEffects color",
            "OnClickEffects color",
            "OnSelectEffects color",
            "Other color"
        };
        
        public override void Initialize()
        {
            button = GraphicTarget.GetComponent<YoButton>();
            themeData = button.buttonThemeData;
            
        }

        protected override IEnumerator Apply()
        {
            ApplyColor();
            // Wait for the specified duration before reverting the color, to ensure the effect is visible
            yield return new WaitForSeconds(Duration);
        }

        protected override IEnumerator Cancel()
        {
            GraphicTarget.color = m_lastColor;
            
            // To ensure the effect is cancelled properly, we check if the button is still hovered
            if(isClickEffect && !button.isPointerOver) button.OnPointerExit(null);
            
            yield return null;
        }
        
        // Apply the color based on the index
        private void ApplyColor()
        {
            switch(selectedColorIndex)
            {
                case 0:
                    m_lastColor = themeData.normalColor;
                    GraphicTarget.color = themeData.hoverColor;
                    break;
                case 1:
                    isClickEffect = true;
                    m_lastColor = themeData.hoverColor;
                    GraphicTarget.color = themeData.clickColor;
                    break;
                case 2:
                    m_lastColor = themeData.normalColor;
                    GraphicTarget.color = themeData.selectedColor;
                    break;
                case 3:
                    m_lastColor = themeData.normalColor;
                    GraphicTarget.color = customColor;
                    break;
                default:
                    Debug.LogWarning("Invalid color index selected.");
                    break;
            }

        }

    }
}