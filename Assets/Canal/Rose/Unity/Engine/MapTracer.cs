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
            float leftoverDelta = deltaDistance;
            MapNode nodeToSample = CurrentNode;
            MapNode previousNode = CurrentNode;
            Vector3 sample = Vector3.zero;
            do
            {
                float newPos = currentPosition + leftoverDelta;
                sample = nodeToSample.SampleWorldMove(currentPosition, newPos, out currentPosition, out leftoverDelta);
                if (leftoverDelta > 0.01f)
                {
                    MapNode newNode = nodeToSample.GetDefaultExit(previousNode);
                    if (newNode == null) break;
                    previousNode = nodeToSample;
                    nodeToSample = newNode;
                    currentPosition = 0;
                }
                else if (leftoverDelta < -0.01f)
                {
                    MapNode newNode = nodeToSample.GetDefaultEntrance(previousNode);
                    if (newNode == null) break;
                    previousNode = nodeToSample;
                    nodeToSample = newNode;
                    currentPosition = nodeToSample.GetWorldLength();
                }
                else break;

            } while (true);

            CurrentNode = nodeToSample;

            return sample;
        }

        public override Vector3 GetFacingDirection(float direction)
        {
            direction = Mathf.Sign(direction);
            return CurrentNode.GetFacingDirection(currentPosition - FacingSampleDistance * direction,
                                                  currentPosition + FacingSampleDistance * direction);
        }

        public override Trigger[] GetTriggers(float radius)
        {
            return CurrentNode.GetFiredTriggers(this.currentPosition - radius, this.currentPosition + radius);
        }
    }
}
