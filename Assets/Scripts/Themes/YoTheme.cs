using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yosf.UI
{
    /// <summary>
    /// YoTheme is a class that provides methods to manage and apply themes to UI elements.
    /// </summary>
    [Serializable]
    public class YoTheme
    {
        /// <summary>
        /// Returns a default theme data.
        /// </summary>
        /// <returns></returns>
        public ThemeData DefaultTheme()
        {
            ThemeData themeData = ScriptableObject.CreateInstance<ThemeData>();
            
            themeData.normalColor = Color.white;
            themeData.hoverColor = new Color(0, 0.5f, 1f, 0.5f); // Semi-transparent blue
            themeData.clickColor = new Color(1f, 0.5f, 0f, 0.5f); // Semi-transparent orange
            themeData.selectedColor = new Color(0.5f, 1f, 0.5f, 0.5f); // Semi-transparent green
            return themeData;
        }
        
        /// <summary>
        /// Applies the button color from the theme data to the specified graphic.
        /// </summary>
        /// <param name="graphic">The graphic component to apply the color to.</param>
        /// <param name="themeData">The theme data containing the color information.</param>
        public void ApplyButtonColor(Graphic graphic, ThemeData themeData)
        {
            if (graphic == null || themeData == null) return;
            
            graphic.color = themeData.normalColor;
        }
        
        /// <summary>
        /// Applies the hover color from the theme data to the specified graphic.
        /// </summary>
        /// <param name="graphic">The graphic component to apply the hover color to.</param>
        /// <param name="themeData">The theme data containing the hover color information.</param>
        public void ApplyHoverColor(Graphic graphic, ThemeData themeData)
        {
            if (graphic == null || themeData == null) return;
            
            graphic.color = themeData.hoverColor;
        }
        
        /// <summary>
        /// Applies the click color from the theme data to the specified graphic.
        /// </summary>
        /// <param name="graphic">The graphic component to apply the click color to.</param>
        /// <param name="themeData">The theme data containing the click color information.</param>
        public void ApplyClickColor(Graphic graphic, ThemeData themeData)
        {
            if (graphic == null || themeData == null) return;
            
            graphic.color = themeData.clickColor;
        }
        
        /// <summary>
        /// Applies the selected color from the theme data to the specified graphic.
        /// </summary>
        /// <param name="graphic">The graphic component to apply the selected color to.</param>
        /// <param name="themeData">The theme data containing the selected color information.</param>
        public void ApplySelectedColor(Graphic graphic, ThemeData themeData)
        {
            if (graphic == null || themeData == null) return;
            
            graphic.color = themeData.selectedColor;
        }
    }
}