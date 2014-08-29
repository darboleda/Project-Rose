using UnityEngine;

using Canal.Rose.Unity.Engine;
using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class RailTracer : Tracer {
        public Rail CurrentRail;
        public float FacingSampleDistance = 0.001f;

        public override Vector3 Move(float deltaDistance)
        {
            currentPosition += deltaDistance;
            currentPosition = Mathf.Clamp(currentPosition, 0, CurrentRail.GetWorldLength());
            return CurrentRail.SampleWorld(currentPosition + deltaDistance);
        }

        public override Vector3 GetFacingDirection(float direction)
        {
            direction = Mathf.Sign(direction);
            return (CurrentRail.SampleWorld(currentPosition + FacingSampleDistance * direction)
                 - CurrentRail.SampleWorld(currentPosition - FacingSampleDistance * direction)).normalized;
        }

        public override Trigger[] GetTriggers(float radius)
        {
            return CurrentRail.GetFiredTriggers(this.currentPosition - radius, this.currentPosition + radius);
        }
    }
}
