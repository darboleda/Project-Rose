using UnityEngine;

using System.Collections.Generic;
using System.Linq;

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

        public Vector3 FollowPath(float deltaDistance, Stack<GraphNode> path, MapNode targetNode, float targetPosition)
        {
            float leftoverDelta = Mathf.Abs(deltaDistance);
            Vector3 sample = Vector3.zero;
            do
            {
                if (!path.Any())
                {
                    if (CurrentNode == targetNode)
                    {
                        if (leftoverDelta > Mathf.Abs(targetPosition - currentPosition))
                        {
                            sample = targetNode.SampleWorldMove(currentPosition, targetPosition, out currentPosition, out leftoverDelta);
                        }
                        else
                        {
                            float targetPos = Mathf.Sign(targetPosition - currentPosition) * leftoverDelta + currentPosition;
                            sample = targetNode.SampleWorldMove(currentPosition, targetPos, out currentPosition, out leftoverDelta);
                        }
                    }
                    break;
                }

                // If there's still a path to follow...
                GraphNode destination = path.Peek();
                float d = (destination.isExit ? leftoverDelta : -leftoverDelta);
                sample = destination.Source.SampleWorldMove(currentPosition, currentPosition + d, out currentPosition, out d);
                leftoverDelta = Mathf.Abs(d);
                if (leftoverDelta > 0.01f)
                {
                    GraphNode nextNode = path.Pop();

                    if (path.Any())
                    {
                        nextNode = path.Pop();
                    }
                    CurrentNode = nextNode.Source;
                    currentPosition = (nextNode.isExit ? CurrentNode.GetWorldLength() : 0);
                }

            } while (leftoverDelta > 0.01f);

            return sample;
        }

        public override Vector3 GetFacingDirection(float direction)
        {
            direction = Mathf.Sign(direction);
            Vector3 result = CurrentNode.GetFacingDirection(currentPosition - FacingSampleDistance * direction,
                                                  currentPosition + FacingSampleDistance * direction);
            return result;
        }

        public override Trigger[] GetTriggers(float radius)
        {
            return CurrentNode.GetFiredTriggers(this.currentPosition - radius, this.currentPosition + radius);
        }
    }
}
