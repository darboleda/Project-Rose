using UnityEngine;
using System.Linq;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class FusedRail : Rail
    {
        public Rail[] Rails;
        public bool loop;

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
                for (int i = 0, length = Rails.Length; i < length; ++i)
                {
                    float railLength = Rails[i].GetLength();
                    if(railLength >= distance)
                    {
                        return Rails[i].Sample(distance);
                    }
                    distance -= railLength;
                }
            } while(loop);
            return Rails[Rails.Length - 1].Sample(1);
        }

        public override Vector3 SampleWorld(float worldDistance)
        {
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
                for (int i = 0, length = Rails.Length; i < length; ++i)
                {
                    float railLength = Rails[i].GetWorldLength();
                    if(railLength >= worldDistance)
                    {
                        return Rails[i].SampleWorld(worldDistance);
                    }
                    worldDistance -= railLength;
                }
            } while (loop);
            return Rails[Rails.Length - 1].SampleWorld(1);
        }

        public override RailTrigger[] GetFiredTriggers(float positionMin, float positionMax)
        {
            if (positionMin < 0
                || positionMax < 0)
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
                } while (loop);
                return false;
            }).ToArray();
        }
    }
}
