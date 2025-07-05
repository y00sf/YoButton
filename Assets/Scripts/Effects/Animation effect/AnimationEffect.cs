using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yosf.UI
{
    [Serializable]
    public class AnimationEffect : YoEffect
    {
        private Animator animator;
        [SerializeField] public String[] triggerNames = { "Normal", "Hovered", "Pressed", "Selected" };
        [SerializeField] public int selectedTriggerIndex;
        [SerializeField] public bool generateAnimation;
        public override void Initialize()
        {
            animator = GraphicTarget.GetComponent<Animator>();
        }
        
        protected override IEnumerator Apply()
        {
            animator.SetTrigger(triggerNames[selectedTriggerIndex]);
            yield return null;
        }

        protected override IEnumerator Cancel()
        {
            yield return null;
        }
        
        public void GenerateAnimation()
        {
            
        }
    }
}