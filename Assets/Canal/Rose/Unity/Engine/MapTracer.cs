using UnityEngine;

using Canal.Rose.Unity.Engine;
using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class MapTracer : Tracer {
        public MapNode CurrentNode;
        public float FacingSampleDistance = 0.001f;

        public override Vector3 Move(float deltaDistance)
        {
            Vector3 sample = CurrentNode.SampleWorld(currentPosition + deltaDistance, out currentPosition);
            return sample;
        }

        public override Vector3 GetFacingDirection(float direction)
        {
            direction = Mathf.Sign(direction);
            return (CurrentNode.SampleWorld(currentPosition + FacingSampleDistance * direction)
                 - CurrentNode.SampleWorld(currentPosition - FacingSampleDistance * direction)).normalized;
        }

        public override Trigger[] GetTriggers(float radius)
        {
            return CurrentNode.GetFiredTriggers(this.currentPosition - radius, this.currentPosition + radius);
        }
    }
}
