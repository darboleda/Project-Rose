using UnityEngine;

using System.Collections.Generic;

using Canal.Rose.Unity.Engine;
using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class PathFindingPhysics : MovementPhysics
    {
        public MapNode TargetNode;
        public float TargetPosition;

        private MapTracer tracer;
        private Graph graph;
        private Stack<GraphNode> path;

        public void Awake()
        {
            tracer = this.Tracer as MapTracer;
            graph = GraphSearch.GenerateGraph(tracer.CurrentNode);

            if (TargetNode == null) return;
            path = graph.GeneratePath(tracer.CurrentNode, tracer.currentPosition, TargetNode, TargetPosition);
        }

        protected override void UpdatePosition(float distance)
        {
            if (path == null) return;
            transform.position = tracer.FollowPath(distance, path, TargetNode, TargetPosition);
        }
    }
}
