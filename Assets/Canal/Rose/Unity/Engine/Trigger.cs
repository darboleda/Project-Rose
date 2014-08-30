using UnityEngine;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class Trigger : Behavior
    {
        public float CenterPoint;
        public float Radius;

        public float Minimum
        {
            get { return CenterPoint - Radius; }
            set
            {
                float max = Maximum;
                float min = value;

                Radius = Mathf.Abs(max - min) * 0.5f;
                CenterPoint = min + Radius;
            }
        }

        public float Maximum
        {
            get { return CenterPoint + Radius; }
            set
            {
                float max = value;
                float min = Minimum;

                Radius = Mathf.Abs(max - min) * 0.5f;
                CenterPoint = min + Radius;
            }
        }

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
