using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Yosf.UI
{
    public class YoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Fields

        [SerializeField] public Graphic GraphicTarget;
        //Used to detect any changes in the GraphicTarget 
        [HideInInspector] public Graphic currentGraphicTarget;
        
        [HideInInspector] public YoTheme m_theme;
        [HideInInspector] public int selectedThemeIndex;
        [HideInInspector] public ThemeData buttonThemeData;
        [HideInInspector] public ThemeData currentButtonThemeData;
        [HideInInspector] public bool isPointerOver;
        [HideInInspector] public bool isSelected;
        

        [HideInInspector] public string[] effectType = { "ScaleEffect", "ColorEffect", "AnimationEffect" };
        [SerializeField, SerializeReference] private List<YoEffect> m_onHoverEffects = new();
        [SerializeField, SerializeReference] private List<YoEffect> m_onClickEffects = new();
        [SerializeField, SerializeReference] private List<YoEffect> m_onSelectEffects = new();

        // Events
        public UnityEvent OnHover, OnUnHover, OnClick, OnSelect, OnUnSelect;

        #endregion

        #region Coroutines

        private Coroutine m_effectCoroutine;

        #endregion

        #region Pointer Events
        
        /// <summary>
        /// Called when the mouse pointer enters the button area.
        /// This method initializes and starts all hover effects associated with the button.
        /// It also invokes the OnHover event.
        /// </summary>
        /// <param name="eventData">The event data associated with the pointer event.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerOver = true;
            PlayEffects(m_onHoverEffects, true);
            OnHover?.Invoke();
        }

        /// <summary>
        /// Called when the mouse pointer exits the button area.
        /// This method stops all hover effects associated with the button.
        /// It also invokes the OnUnHover event.
        /// </summary>
        /// <param name="eventData">The event data associated with the pointer event.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerOver = false;
            StopEffects(m_onHoverEffects);
            OnUnHover?.Invoke();
        }

        /// <summary>
        /// Called when the button is clicked.
        /// This method stops all hover effects, plays the click effects, and invokes the OnClick event.
        /// </summary>
        /// <param name="eventData">The event data associated with the pointer event.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            StopEffects(m_onHoverEffects);
            PlayEffects(m_onClickEffects, true, true);
            OnClick?.Invoke();
        }

        #endregion
        
        /// <summary>
        /// Called when the button is selected.
        /// </summary>
        public void OnButtonSelected()
        {
            isSelected = true;
            PlayEffects(m_onSelectEffects, true);
            OnSelect?.Invoke();
        }
        
        /// <summary>
        /// Called when the button no longer selected.
        /// </summary>
        public void OnButtonUnSelected()
        {
            StopEffects(m_onSelectEffects);
            OnUnSelect?.Invoke();
            isSelected = false;
        }

        #region Settings Management

        /// <summary>
        /// Checks if the GraphicTarget has changed.
        /// If it has, updates the current graphic target and applies the theme.
        /// </summary>
        public void UpdateGraphicTarget()
        {
            if (!CheckGraphicTarget()) return; 
            if(GraphicTarget == currentGraphicTarget) return;
            currentGraphicTarget = GraphicTarget;
            ApplyTheme();
        }
        
        /// <summary>
        /// Checks if the GraphicTarget is set and assigns it if not.
        /// </summary>
        /// <returns>Returns true if the GraphicTarget is valid, false otherwise.</returns>
        public bool CheckGraphicTarget()
        {
            GraphicTarget ??= GetComponent<Graphic>();
            
            if(GraphicTarget == null)
            {
                Debug.LogWarning($"No Graphic component found on {gameObject.name}. " + "Please assign a Graphic component to the button.");
                return false;
            }
            return true;
        }

        #endregion
        
        #region Theme Management

        /// <summary>
        /// Updates the theme of the button if the buttonThemeData has changed.
        /// </summary>
        public void UpdateTheme()
        {
            if (buttonThemeData == currentButtonThemeData) return;
            
            ApplyTheme(); // Apply the new theme to the graphic target
            currentButtonThemeData = buttonThemeData; // Update the current theme data
        }

        /// <summary>
        /// Applies the selected theme to the button.
        /// </summary>
        public void ApplyTheme()
        {
            if (!CheckGraphicTarget()) return; 
            m_theme = new YoTheme();
            switch (selectedThemeIndex)
            {
                case 0: // Default Theme
                    buttonThemeData = m_theme.DefaultTheme();
                    m_theme.ApplyButtonColor(GraphicTarget, buttonThemeData);
                    break;
                case 1: // Manager feed
                    // Implement logic for manager feed theme if needed
                    break;
                case 2: // Custom Theme
                    if (buttonThemeData == null)
                    {
                        Debug.LogWarning($"No custom theme set on {gameObject.name}. " + "Please assign a ThemeData to the button.");
                        return;
                    }
                    m_theme.ApplyButtonColor(GraphicTarget, buttonThemeData);
                    break;
                default:
                    Debug.LogWarning($"Unknown theme index {selectedThemeIndex} on {gameObject.name}. " + "Please check the theme settings.");
                    break;
            }
        }
        
        #endregion
        
        # region Effect Management
        
        /// <summary>
        /// Checks if the effect target is set and assigns the GraphicTarget to the effect.
        /// </summary>
        /// <param name="effect">The effect to check.</param>
        private void EffectTarget(YoEffect effect)
        {
            if (effect == null) return;
            
            effect.GraphicTarget = GraphicTarget;
        }
        
        /// <summary>
        /// Plays the specified effects on the button.
        /// </summary>
        /// <param name="effects">The effects to play.</param>
        /// <param name="initialize">If true, initializes the effects before starting them.</param>
        /// <param name="endAfter">If true, ends the effects after starting them.</param>
        private void PlayEffects (IEnumerable<YoEffect> effects, bool initialize, bool endAfter = false)
        {
            foreach (var effect in effects)
            {
                EffectTarget(effect);
                if (initialize) effect.Initialize();
                effect.StartEffect(this);
                if (endAfter) effect.EndEffect(this);
            }
        }
        
        /// <summary>
        /// Stops the specified effects on the button.
        /// </summary>
        /// <param name="effects">The effects to stop.</param>
        private void StopEffects(IEnumerable<YoEffect> effects)
        {
            foreach (var effect in effects)
            {
                effect.StopEffect(this);
            }
        }

        #endregion
    }
}