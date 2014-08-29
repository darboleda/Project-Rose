using UnityEngine;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class RailTrigger : Trigger
    {
        public Rail Parent;

        public void Awake()
        {
            Parent.RegisterTrigger(this);
        }

        public void OnDestroy()
        {
            Parent.UnregisterTrigger(this);
        }
    }
}
