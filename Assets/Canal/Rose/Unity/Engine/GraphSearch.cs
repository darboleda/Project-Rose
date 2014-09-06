using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    if (child != null && nodes.Add(child)) { nodesToAdd.Enqueue(child); }
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

            public IEnumerable<GraphNode> GetNodes()
            {
                yield return Entrance;
                yield return Exit;
            }
        }

        private Dictionary<MapNode, GraphNodeWrapper> mappedNodes = new Dictionary<MapNode, GraphNodeWrapper>();
        public Graph(HashSet<MapNode> mapNodes)
        {
            foreach (MapNode node in mapNodes)
            {
                mappedNodes[node] = new GraphNodeWrapper()
                {
                    Entrance = new GraphNode(node, false),
                    Exit = new GraphNode(node, true)
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
                    if (node == null) continue;
                    entrance.AddTransition(mappedNodes[node].Exit, 0);
                }

                foreach (MapNode node in pair.Key.Exits)
                {
                    if (node == null) continue;
                    exit.AddTransition(mappedNodes[node].Entrance, 0);
                }
            }
        }

        public float GetShortestDistance(MapNode start, float startPosition, MapNode end, float endPosition)
        {
            Dictionary<GraphNode, float> distanceFromSource;
            Dictionary<GraphNode, GraphNode> optimalPrevious;

            GraphNode final = this.GenerateShortestDistanceMap(
                start, startPosition, end, endPosition,
                out distanceFromSource, out optimalPrevious);

            return distanceFromSource[final];
        }

        public Stack<GraphNode> GeneratePath(MapNode start, float startPosition, MapNode end, float endPosition)
        {
            Stack<GraphNode> path = new Stack<GraphNode>();
            Dictionary<GraphNode, float> distanceFromSource;
            Dictionary<GraphNode, GraphNode> optimalPrevious;

            GraphNode final = this.GenerateShortestDistanceMap(
                start, startPosition, end, endPosition,
                out distanceFromSource, out optimalPrevious);

            GraphNode current = final;
            while (optimalPrevious.ContainsKey(current))
            {
                current = optimalPrevious[current];
                path.Push(current);
            }
            path.Pop();
            return path;
        }

        private GraphNode GenerateShortestDistanceMap(
            MapNode startNode, float startPosition, 
            MapNode endNode, float endPosition,
            out Dictionary<GraphNode, float> distanceFromSource,
            out Dictionary<GraphNode, GraphNode> optimalPrevious)
        {
            distanceFromSource = new Dictionary<GraphNode, float>();
            optimalPrevious = new Dictionary<GraphNode, GraphNode>();

            Queue<GraphNode> nodesToVisit = new Queue<GraphNode>();
            foreach (GraphNode node in mappedNodes.Values.SelectMany(x => x.GetNodes()))
            {
                distanceFromSource[node] = float.PositiveInfinity;
            }

            GraphNode start = new GraphNode(startNode, false);
            GraphNode end = new GraphNode(endNode, false);

            distanceFromSource[start] = 0;
            distanceFromSource[end] = float.PositiveInfinity;

            start.AddTransition(mappedNodes[startNode].Entrance, startPosition);
            start.AddTransition(mappedNodes[startNode].Exit, startNode.GetWorldLength() - startPosition);

            if (start.Source == end.Source)
                start.AddTransition(end, Mathf.Abs(endPosition - startPosition));

            mappedNodes[endNode].Entrance.AddTransition(end, endPosition);
            mappedNodes[endNode].Exit.AddTransition(end, endNode.GetWorldLength() - endPosition);

            distanceFromSource[start] = 0;
            nodesToVisit.Enqueue(start);

            while (nodesToVisit.Any())
            {
                GraphNode node = nodesToVisit.Dequeue();
                foreach (Transition transition in node.Transitions)
                {
                    if (optimalPrevious.ContainsKey(transition.Destination))
                        continue;
                    nodesToVisit.Enqueue(transition.Destination);
                    float newDistance = distanceFromSource[node] + transition.Cost;
                    if (newDistance < distanceFromSource[transition.Destination])
                    {
                        distanceFromSource[transition.Destination] = newDistance;
                        optimalPrevious[transition.Destination] = node;
                    }
                }
            }

            mappedNodes[endNode].Entrance.RemoveTransition(end);
            mappedNodes[endNode].Exit.RemoveTransition(end);
            return end;
        }
    }

    public class GraphNode
    {
        public MapNode Source;
        public List<Transition> Transitions = new List<Transition>();
        public bool isExit;

        public GraphNode(MapNode source, bool isExit)
        {
            Source = source;
            this.isExit = isExit;
        }

        public void AddTransition(GraphNode destination, float cost)
        {
            this.Transitions.Add(new Transition() { Destination = destination, Cost = cost });
        }

        public void RemoveTransition(GraphNode destination)
        {
            for (int i = Transitions.Count - 1; i >= 0; --i)
            {
                if (Transitions[i].Destination == destination)
                    Transitions.RemoveAt(i);
            }
        }

        public float GetCost(GraphNode destination)
        {
            foreach (Transition transition in Transitions)
                if (transition.Destination == destination)
                    return transition.Cost;
            return float.PositiveInfinity;
        }
    }

    public struct Transition
    {
        public GraphNode Destination;
        public float Cost;
    }
}
