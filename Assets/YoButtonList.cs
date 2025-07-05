using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yosf.UI
{
    public class YoButtonList : MonoBehaviour
    {
        public int defaultButtonIndex; // Index of the default button to select at start

        [SerializeField] private List<YoButton> buttons;
        [HideInInspector] public int selectedIndex;


        private void Start()
        {
            if (buttons == null || buttons.Count == 0)
            {
                Debug.LogWarning("ButtonList: No buttons assigned.");
                return;
            }

            SubscribeToButtons();
            selectedIndex = defaultButtonIndex; // Default to the first button
            ButtonSelect(buttons[selectedIndex]);
        }

        /// <summary>
        /// Subscribes to the OnClick event of each button in the list.
        /// </summary>
        void SubscribeToButtons()
        {
            foreach (var button in buttons)
            {
                button.OnClick.AddListener(() => ButtonSelect(button));
            }
        }

        /// <summary>
        /// Selects a button and unselects the previously selected button.
        /// </summary>
        /// <param name="button">The button to select.</param>
        void ButtonSelect(YoButton button)
        {
            // Check if the button is already selected
            if (button.isSelected) return;

            // Unselect the currently selected button
            UnSelect(selectedIndex);
            selectedIndex = buttons.IndexOf(button);
            button.OnButtonSelected();
        }

        /// <summary>
        /// Unselects a button at the specified index.
        /// </summary>
        /// <param name="index">The index of the button to unselect.</param>
        private void UnSelect(int index)
        {
            if (index < 0 || index >= buttons.Count || !buttons[index].isSelected) return;

            buttons[index].OnButtonUnSelected();
        }
    }
}