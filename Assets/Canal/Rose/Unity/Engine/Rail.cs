using UnityEngine;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public abstract class Rail : Behavior
    {
        [System.NonSerialized]
        public float CurrentSample;

        public abstract float GetLength();
        public abstract float GetWorldLength();

        public abstract Vector3 Sample(float distance);
        public abstract Vector3 SampleWorld(float worldDistance);
    }
}
