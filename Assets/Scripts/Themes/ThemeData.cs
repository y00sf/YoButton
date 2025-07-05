using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yosf.UI
{
    /// <summary>
    /// ThemeData is a ScriptableObject that holds the color themes for a button.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "YoButtonTheme", menuName = "Yosf/YoButtonTheme", order = 1)]
    public class ThemeData : ScriptableObject
    {
        [SerializeField] public Color normalColor;
        [SerializeField] public Color hoverColor;
        [SerializeField] public Color clickColor;
        [SerializeField] public Color selectedColor;
    }
    
}