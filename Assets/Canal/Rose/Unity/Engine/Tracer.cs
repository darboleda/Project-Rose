using UnityEngine;

using Canal.Rose.Unity.Engine;
using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public abstract class Tracer : Behavior {
        protected float currentPosition = 0;

        public abstract Vector3 Move(float distance);
        public abstract Vector3 GetFacingDirection(float direction);

        public abstract Trigger[] GetTriggers(float radius);
    }
}
