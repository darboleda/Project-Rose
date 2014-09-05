using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class RailJunction : MapNode
    {
        [System.Serializable]
        public struct RailJunctionPair
        {
            public MapNode entrance;
            public MapNode exit;
        }

        public RailJunctionPair[] junctions;
        
        public override MapNode GetDefaultExit(MapNode entranceNode)
        {
            return junctions.FirstOrDefault(x => x.entrance == entranceNode).exit;
        }
        public override MapNode GetDefaultEntrance(MapNode exitNode)
        {
            return junctions.FirstOrDefault(x => x.exit == exitNode).entrance;
        }

        public override IEnumerable<MapNode> Entrances { get { return junctions.Select(x => x.entrance); } }
        public override IEnumerable<MapNode> Exits { get { return junctions.Select(x => x.exit); } }

        public override Vector3 SampleMove(float start, float end, out float sampledEnd, out float leftoverDelta)
        {
            Vector3 result = Sample(end, out sampledEnd);
            leftoverDelta = end - start;
            return result;
        }

        public override Vector3 SampleWorldMove(float start, float end, out float sampledEnd, out float leftoverDelta)
        {
            Vector3 result = SampleWorld(end, out sampledEnd);
            leftoverDelta = end - start;
            return result;
        }

        public override Vector3 Sample(float distance, out float sampledDistance)
        {  
            sampledDistance = 0;
            return this.transform.localPosition;
        }
        
        public override Vector3 SampleWorld(float worldDistance, out float sampledDistance)
        {
            sampledDistance = 0;
            return this.transform.position;
        }

        public override Trigger[] GetFiredTriggers(float positionMin, float positionMax)
        {
            return new Trigger[0];
        }

        public override float GetLength() { return 0 ; }
        public override float GetWorldLength() { return 0 ; }
    }
}
