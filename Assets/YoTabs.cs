using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yosf.UI
{
    public class YoTabs : MonoBehaviour
    {
        public int defaultButtonIndex; // Index of the default button to select at start
        [SerializeField] private List<Tab> tabs;
        [HideInInspector] public int selectedTabIndex;

        private void Start()
        {
            if (tabs == null || tabs.Count == 0)
            {
                Debug.LogWarning("Tabs: No tabs assigned.");
                return;
            }

            SubscribeToButtons();
            selectedTabIndex = defaultButtonIndex; // Default to the first tab
            HideAllContents();
            SelectTab(tabs[selectedTabIndex]);
        }
        
        private void Update()
        {
            CheckIsSelected();
        }

        /// <summary>
        /// Subscribes to the OnClick event of each tab button.
        /// </summary>
        void SubscribeToButtons()
        {
            foreach (var tab in tabs)
            {
                tab.button.OnClick.AddListener(() => SelectTab(tab));
            }
        }

        /// <summary>
        /// Selects a tab and unselects the previously selected tab.
        /// </summary>
        /// <param name="tab">The tab to select.</param>
        void SelectTab(Tab tab)
        {
            // Check if the tab is already selected
            if (tab.button.isSelected) return;

            // Unselect the currently selected tab
            UnSelect(selectedTabIndex);
            selectedTabIndex = tabs.IndexOf(tab);
            tab.button.OnButtonSelected();
            ShowContent(tab);
        }

        /// <summary>
        /// Unselects a tab at the specified index.
        /// </summary>
        /// <param name="index">The index of the tab to unselect.</param>
        private void UnSelect(int index)
        {
            if (index < 0 || index >= tabs.Count || !tabs[index].button.isSelected) return;
            
            HideContent(tabs[index]);
            tabs[index].button.OnButtonUnSelected();
        }

        /// <summary>
        /// Shows the content of the selected tab.
        /// </summary>
        /// <param name="tab">The tab whose content should be shown.</param>
        private void ShowContent(Tab tab)
        {
            // Show the content of the selected tab
            foreach (var content in tab.content)
            {
                content.SetActive(true);
            }
        }
        
        /// <summary>
        /// Hides the content of the unselected tab.
        /// </summary>
        /// <param name="tab">The tab whose content should be hidden.</param>
        private void HideContent(Tab tab)
        {
            // Hide the content of the unselected tab
            foreach (var content in tab.content)
            {
                content.SetActive(false);
            }
        }
        
        /// <summary>
        /// Hides all tab contents.
        /// </summary>
        private void HideAllContents()
        {
            // Hide all tab contents
            foreach (var tab in tabs)
            {
                HideContent(tab);
            }
        }
        
        private void CheckIsSelected()
        {
            if (tabs[selectedTabIndex].button.GraphicTarget.color !=
                tabs[selectedTabIndex].button.buttonThemeData.selectedColor)
            {
                tabs[selectedTabIndex].button.GraphicTarget.color = tabs[selectedTabIndex].button.buttonThemeData.selectedColor;
            }
        }
    }

    [Serializable]
    public class Tab
    {
        public YoButton button;
        public GameObject[] content;
    }
}