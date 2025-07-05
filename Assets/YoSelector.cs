using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yosf.UI
{
    public class YoSelector : MonoBehaviour
    {
        [SerializeField] private YoButton leftButton;
        [SerializeField] private YoButton rightButton;
        [SerializeField] private TMP_Text text;
        [SerializeField] private bool loop;
        
        private int m_index;
        [SerializeField] private string[] options = {"Option 1", "Option 2", "Option 3"};
        
        private void Start()
        {
            leftButton.OnClick.AddListener(OnLeftButtonClicked);
            rightButton.OnClick.AddListener(OnRightButtonClicked);
            UpdateText();
        }
        
        /// <summary>
        /// Handles the left button click event to decrease the index and update the text.
        /// </summary>
        private void OnLeftButtonClicked()
        {
            m_index--;
            if (m_index < 0)
            {
                m_index = loop ? options.Length - 1 : 0;
            }
            UpdateText();
        }
        
        /// <summary>
        /// Handles the right button click event to increase the index and update the text.
        /// </summary>
        private void OnRightButtonClicked()
        {
            m_index++;
            if (m_index >= options.Length)
            {
                m_index = loop ? 0 : options.Length - 1;
            }
            UpdateText();
        }

        /// <summary>
        /// Updates the text component to display the currently selected option.
        /// </summary>
        private void UpdateText()
        {
            if (text != null)
            {
                text.text = options[m_index];
            }
            else
            {
                Debug.LogWarning("YoSelector: Text component is not assigned. Please assign a TMP_Text component to display the selected option.");
            }
        }
        
        #region Setters and Getters
        
        public void SetOptions(string[] options)
        {
            this.options = options;
            m_index = 0;
            UpdateText();
        }
        
        public void SetIndex(int index)
        {
            m_index = index;
            UpdateText();
        }
        
        public void SetIndex(string option)
        {
            m_index = System.Array.IndexOf(options, option);
            UpdateText();
        }
        
        public string GetSelectedOption()
        {
            return options[m_index];
        }
        
        public int GetSelectedIndex()
        {
            return m_index;
        }
        
        #endregion
    }
}