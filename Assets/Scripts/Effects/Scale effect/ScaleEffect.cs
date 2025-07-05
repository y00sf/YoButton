using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Yosf.UI
{
    [Serializable]
    public class ScaleEffect : YoEffect
    {
        #region Fields
        
        [SerializeField] private Vector3 to;
        [SerializeField] float duration = 0.1f;

        private Vector3 m_originalScale;
        
        #endregion
        public override void Initialize()
        {
            m_originalScale = GraphicTarget.transform.localScale;
            
        }

        protected override IEnumerator Apply()
        {
            Vector3 from = GraphicTarget.transform.localScale;
            float time = 0f;

            while (time < duration)
            {
                GraphicTarget.transform.localScale = Vector3.Lerp(from, to, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            GraphicTarget.transform.localScale = to;
        }
        
        protected override IEnumerator Cancel()
        {
            float time = 0f;
            var from = GraphicTarget.transform.localScale;
            
            // Calculate the back duration based on the distance from the original scale
            float totalDistance = Vector3.Distance(to, m_originalScale);
            float currentDistance = Vector3.Distance(from, m_originalScale);
            float backDuration = (totalDistance > 0f) ? duration * currentDistance / totalDistance : 0f;

            while (time < backDuration)
            {
                GraphicTarget.transform.localScale = Vector3.Lerp(from, m_originalScale, time / backDuration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            GraphicTarget.transform.localScale = m_originalScale;
        }
        
    }
}