using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class RailJunction : MapNode
    {
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
    }
}
