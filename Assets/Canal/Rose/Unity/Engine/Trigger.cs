using UnityEngine;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class Trigger : Behavior
    {
        public float CenterPoint;
        public float Radius;

        public void NotifyTriggerStay()
        {
            this.SendMessage("OnCanalTriggerStay", this, SendMessageOptions.DontRequireReceiver);
        }

        public void NotifyTriggerEnter()
        {
            this.SendMessage("OnCanalTriggerEnter", this, SendMessageOptions.DontRequireReceiver);
        }

        public void NotifyTriggerExit()
        {
            this.SendMessage("OnCanalTriggerExit", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}
