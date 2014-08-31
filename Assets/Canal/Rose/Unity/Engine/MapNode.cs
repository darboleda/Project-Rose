using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public abstract class MapNode : Behavior
    {
        public Vector3 Sample(float distance)
        {
            float sampledDistance;
            return Sample(distance, out sampledDistance);
        }

        public Vector3 SampleWorld(float worldDistance)
        {
            float sampledDistance;
            return SampleWorld(worldDistance, out sampledDistance);
        }

        public abstract Vector3 Sample(float distance, out float sampledDistance);
        public abstract Vector3 SampleWorld(float worldDistance, out float sampledDistance);
        public abstract Trigger[] GetFiredTriggers(float positionMin, float positionMax);
    }
}
