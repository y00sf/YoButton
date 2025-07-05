using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yosf.UI
{
    [Serializable]
    public abstract class YoEffect
    {
        #region Fields
        
        [HideInInspector] public Graphic GraphicTarget;
        private Coroutine m_runningRoutine;

        #endregion
        
        /// <summary>
        /// Starts the effect on the specified MonoBehaviour runner.
        /// </summary>
        /// <param name="runner">The MonoBehaviour that will run the effect.</param>
        public void StartEffect(MonoBehaviour runner)
        {
            if(m_runningRoutine != null)
                runner.StopCoroutine(m_runningRoutine);
            
            m_runningRoutine = runner.StartCoroutine(Apply());
        }
        
        /// <summary>
        /// Stops the effect on the specified MonoBehaviour runner.
        /// (Stop the effect immediately, without waiting for the effect to finish)
        /// </summary>
        /// <param name="runner">The MonoBehaviour that will stop the effect.</param>
        public void StopEffect(MonoBehaviour runner)
        {
            if (m_runningRoutine != null)
                runner.StopCoroutine(m_runningRoutine);
            
            
            m_runningRoutine = runner.StartCoroutine(Cancel());
        }
        
        /// <summary>
        /// Ends the effect on the specified MonoBehaviour runner.
        /// (This method waits for the effect to finish applying before starting the cancellation process)
        /// </summary>
        /// <param name="runner">The MonoBehaviour that will end the effect.</param>
        public void EndEffect(MonoBehaviour runner)
        {
            runner.StartCoroutine(WaitForApplyThenCancel(runner));
        }
        
        /// <summary>
        /// Initializes the effect.
        /// </summary>
        public abstract void Initialize();
        
        /// <summary>
        /// Waits for the effect to apply before starting the cancellation process.
        /// </summary>
        /// <param name="runner">The MonoBehaviour that will run the effect.</param>
        /// <returns></returns>
        private IEnumerator WaitForApplyThenCancel(MonoBehaviour runner)
        {
            if (m_runningRoutine != null)
                yield return m_runningRoutine;

            m_runningRoutine = runner.StartCoroutine(Cancel());
        }
        
        /// <summary>
        /// The functionality that applies the effect.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator Apply();
        
        /// <summary>
        /// The functionality that cancels the effect.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator Cancel();
    }
}
