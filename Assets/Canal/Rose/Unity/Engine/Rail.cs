using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public abstract class Rail : Behavior
    {
        [System.NonSerialized]
        public float CurrentSample;

        [System.NonSerialized]
        public List<RailTrigger> registeredTriggers = new List<RailTrigger>();

        public abstract float GetLength();
        public abstract float GetWorldLength();

        public abstract Vector3 Sample(float distance);
        public abstract Vector3 SampleWorld(float worldDistance);

        public void RegisterTrigger(RailTrigger trigger)
        {
            registeredTriggers.Add(trigger);
        }

        public void UnregisterTrigger(RailTrigger trigger)
        {
            registeredTriggers.Remove(trigger);
        }

        public virtual RailTrigger[] GetFiredTriggers(float positionMin, float positionMax)
        {
            return registeredTriggers.Where((trigger, i) => {
                return IsTriggerContained(trigger, positionMin, positionMax);
            }).ToArray();
        }

        protected static bool IsTriggerContained(Trigger trigger, float positionMin, float positionMax)
        {
            if (trigger == null) return false;
                float min = trigger.CenterPoint - trigger.Radius;
                float max = trigger.CenterPoint + trigger.Radius;

                return (max > positionMax && min < positionMax)
                    || (max > positionMin && min < positionMin)
                    || (max < positionMax && min > positionMin)
                    || (min < positionMin && max > positionMax);
        }
    }
}
