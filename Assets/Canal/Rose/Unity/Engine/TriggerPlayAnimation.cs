using UnityEngine;
using System.Collections;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class TriggerPlayAnimation : Behavior
    {
        public Animator Animator;
        public string AnimatorEnterTrigger;
        public string AnimatorExitTrigger;
        
        public void OnCanalTriggerEnter(Trigger trigger)
        {
            if (!string.IsNullOrEmpty(AnimatorEnterTrigger)) Animator.SetTrigger(AnimatorEnterTrigger);
        }
        
        public void OnCanalTriggerExit(Trigger trigger)
        {
            if (!string.IsNullOrEmpty(AnimatorEnterTrigger)) Animator.SetTrigger(AnimatorExitTrigger);
        }
    }
}