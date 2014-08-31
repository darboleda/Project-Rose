using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public abstract class MapNode : Behavior
    {
        public abstract MapNode GetNextNode(MapNode from);
        public abstract MapNode GetPreviousNode(MapNode from);

        public virtual Vector3 GetFacingDirection(float back, float front)
        {
            return (SampleWorld(front) - SampleWorld(back)).normalized;
        }

        public virtual Vector3 SampleMove(float start, float end, out float sampledEnd, out float leftoverDelta)
        {
            Vector3 result = Sample(end, out sampledEnd);
            leftoverDelta = end - sampledEnd;
            return result;
        }

        public virtual Vector3 SampleWorldMove(float start, float end, out float sampledEnd, out float leftoverDelta)
        {
            Vector3 result = SampleWorld(end, out sampledEnd);
            leftoverDelta = end - sampledEnd;
            return result;
        }

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

        public abstract float GetLength();
        public abstract float GetWorldLength();
    }
}
