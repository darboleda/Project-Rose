using UnityEngine;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class FusedRail : Rail
    {
        public Rail[] Rails;
        private Vector3[] railConnectors;

        public override float GetLength()
        {
            float total = 0;
            for (int i = 0, length = Rails.Length; i < length; ++i)
            {
                total += Rails[i].GetLength();
            }
            return total;
        }

        public override float GetWorldLength()
        {
            float total = 0;
            for (int i = 0, length = Rails.Length; i < length; ++i)
            {
                total += Rails[i].GetWorldLength();
            }
            return total;
        }

        public override Vector3 Sample(float distance)
        {
            if (Rails.Length == 0) return Vector3.zero;

            for (int i = 0, length = Rails.Length; i < length; ++i)
            {
                float railLength = Rails[i].GetLength();
                if(railLength >= distance)
                {
                    return Rails[i].Sample(distance);
                }
                distance -= railLength;
            }
            return Rails[Rails.Length - 1].Sample(1);
        }

        public override Vector3 SampleWorld(float worldDistance)
        {
            if (Rails.Length == 0) return Vector3.zero;
            
            for (int i = 0, length = Rails.Length; i < length; ++i)
            {
                float railLength = Rails[i].GetWorldLength();
                if(railLength >= worldDistance)
                {
                    return Rails[i].SampleWorld(worldDistance);
                }
                worldDistance -= railLength;
            }
            return Rails[Rails.Length - 1].SampleWorld(1);
        }
    }
}
