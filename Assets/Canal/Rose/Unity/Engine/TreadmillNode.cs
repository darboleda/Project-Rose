using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class TreadmillNode : MapNode
    {
        [SerializeField]
        private MapNode previousNode;

        [SerializeField]
        private MapNode nextNode;

        public float length;
        
        public override MapNode GetNextNode(MapNode from) { return nextNode; }
        public override MapNode GetPreviousNode(MapNode from) { return previousNode; }

        public override Vector3 GetFacingDirection(float back, float front)
        {
            float direction = Mathf.Sign(front - back);
            return (transform.TransformDirection(Vector3.right) * direction).normalized;
        }

        public override Vector3 Sample(float distance, out float sampledDistance)
        {  
            sampledDistance = Mathf.Clamp(distance, 0, length);
            return this.transform.localPosition;
        }
        
        public override Vector3 SampleWorld(float worldDistance, out float sampledDistance)
        {
            sampledDistance = Mathf.Clamp(worldDistance, 0, length);
            return this.transform.position;
        }

        public override Trigger[] GetFiredTriggers(float positionMin, float positionMax)
        {
            return new Trigger[0];
        }

        public override float GetLength() { return length; }
        public override float GetWorldLength() { return length; }
    }
}
