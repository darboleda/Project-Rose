using UnityEngine;
using System.Linq;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class FusedRail : Rail
    {
        public Rail[] Rails = new Rail[0];
        public bool loop;
        public bool wrapTriggers = false;

        private Vector3[] railConnectors;

        public override Vector3 SampleMove(float start, float end, out float sampledEnd, out float leftoverDelta)
        {
            Vector3 result = Sample(end, out sampledEnd);
            leftoverDelta = end - sampledEnd;
            if (!loop) return result;

            leftoverDelta = 0;
            return result;
        }

        public override Vector3 SampleWorldMove(float start, float end, out float sampledEnd, out float leftoverDelta)
        {
            Vector3 result = SampleWorld(end, out sampledEnd);
            leftoverDelta = end - sampledEnd;
            if (!loop) return result;

            leftoverDelta = 0;
            return result;
        }

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

        public override Vector3 Sample(float distance, out float sampledDistance)
        {
            sampledDistance = 0;
            if (Rails.Length == 0) return Vector3.zero;
            if (distance < 0)
            {
                if (loop)
                {
                    float fullLength = this.GetLength();
                    while (distance < 0) distance += fullLength;
                }
                else distance = 0;
            }
            do
            {
                sampledDistance = 0;
                for (int i = 0, length = Rails.Length; i < length; ++i)
                {
                    float railLength = Rails[i].GetLength();
                    if(railLength >= distance)
                    {
                        float sampled;
                        Vector3 point = Rails[i].Sample(distance, out sampled);
                        sampledDistance += sampled;
                        return point;
                    }
                    sampledDistance += railLength;
                    distance -= railLength;
                }
            } while(loop);
            sampledDistance = this.GetLength();
            return Rails[Rails.Length - 1].Sample(Rails[Rails.Length - 1].GetLength());
        }

        public override Vector3 SampleWorld(float worldDistance, out float sampledDistance)
        {
            sampledDistance = 0;
            if (Rails.Length == 0) return Vector3.zero;
            if (worldDistance < 0)
            {
                if (loop)
                {
                    float worldLength = this.GetWorldLength();
                    while (worldDistance < 0) worldDistance += worldLength;
                }
                else worldDistance = 0;
            }
            do
            {
                sampledDistance = 0;
                for (int i = 0, length = Rails.Length; i < length; ++i)
                {
                    float railLength = Rails[i].GetWorldLength();
                    if(railLength >= worldDistance)
                    {

                        float sampled;
                        Vector3 point = Rails[i].SampleWorld(worldDistance, out sampled);
                        sampledDistance += sampled;
                        return point;
                    }
                    sampledDistance += railLength;
                    worldDistance -= railLength;
                }
            } while (loop);
            sampledDistance = this.GetWorldLength();
            return Rails[Rails.Length - 1].SampleWorld(Rails[Rails.Length - 1].GetWorldLength());
        }

        public override Trigger[] GetFiredTriggers(float positionMin, float positionMax)
        {
            if (wrapTriggers &&
                (positionMin < 0 || positionMax < 0))
            {
                if (loop)
                {
                    float worldLength = this.GetWorldLength();
                    while (positionMin < 0 || positionMax < 0)
                    {
                        positionMin += worldLength;
                        positionMax += worldLength;
                    }
                }
                else 
                {
                    positionMax = positionMax - positionMin;
                    positionMin = 0;
                }
            }

            return Rails.SelectMany(x => x.registeredTriggers).Where((trigger, i) => {
                float min = positionMin;
                float max = positionMax;
                if (trigger.Parent == this) return IsTriggerContained(trigger, min, max);
                do
                {
                    if (!wrapTriggers)
                    {
                        if (min < 0) min = 0;
                        float worldLength = GetWorldLength();
                        if (max > worldLength) max = worldLength;
                    }
                    foreach (Rail rail in Rails)
                    {
                        if (min < 0 && max < 0) return false;

                        float railLength = rail.GetWorldLength();
                        if (rail != trigger.Parent)
                        {
                            min -= railLength;
                            max -= railLength;
                            continue;
                        }
                        else if(!IsTriggerContained(trigger, min, max))
                        {
                            min -= railLength;
                            max -= railLength;
                            continue;
                        }
                        return true;
                    }
                    if (min < 0 && max < 0) return false;
                } while (loop);
                return false;
            }).ToArray();
        }
    }
}
