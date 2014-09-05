using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public static class GraphSearch
    {
        public static Graph GenerateGraph(this MapNode startingNode)
        {
            HashSet<MapNode> nodes = new HashSet<MapNode>();
            Queue<MapNode> nodesToAdd = new Queue<MapNode>();
            nodes.Add(startingNode);
            nodesToAdd.Enqueue(startingNode);

            while (nodesToAdd.Any())
            {
                MapNode node = nodesToAdd.Dequeue();
                foreach (MapNode child in node.Exits.Concat(node.Entrances))
                {
                    if (nodes.Add(child)) { nodesToAdd.Enqueue(child); }
                }
            }

            return new Graph(nodes);
        }
    }

    public class Graph
    {
        private class GraphNodeWrapper
        {
            public GraphNode Entrance;
            public GraphNode Exit;
        }

        private Dictionary<MapNode, GraphNodeWrapper> mappedNodes = new Dictionary<MapNode, GraphNodeWrapper>();
        public Graph(HashSet<MapNode> mapNodes)
        {
            foreach (MapNode node in mapNodes)
            {
                mappedNodes[node] = new GraphNodeWrapper()
                {
                    Entrance = new GraphNode(node),
                    Exit = new GraphNode(node)
                };
            }

            foreach (KeyValuePair<MapNode, GraphNodeWrapper> pair in mappedNodes)
            {
                GraphNode entrance = pair.Value.Entrance;
                GraphNode exit = pair.Value.Exit;
                float length = pair.Key.GetWorldLength();

                entrance.AddTransition(exit, length);
                exit.AddTransition(entrance, length);
                foreach (MapNode node in pair.Key.Entrances)
                {
                    entrance.AddTransition(mappedNodes[node].Exit, 0);
                }

                foreach (MapNode node in pair.Key.Exits)
                {
                    exit.AddTransition(mappedNodes[node].Entrance, 0);
                }
            }
        }
    }

    public class GraphNode
    {
        public MapNode Source;
        public List<Transition> Transitions = new List<Transition>();
        public GraphNode(MapNode source)
        {
            Source = source;
        }

        public void AddTransition(GraphNode destination, float cost)
        {
            this.Transitions.Add(new Transition() { Destination = destination, Cost = cost });
        }
    }

    public struct Transition
    {
        public GraphNode Destination;
        public float Cost;
    }
}
