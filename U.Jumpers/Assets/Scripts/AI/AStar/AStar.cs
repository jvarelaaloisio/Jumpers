using System;
using System.Collections.Generic;
using System.Linq;

namespace AI.AStar
{
    /// <summary>
    /// Pathfinding algorithm
    /// </summary>
    /// <typeparam name="TNode">the type of node to use</typeparam>
    public class AStar<TNode> where TNode : class
    {
        /// <summary>
        /// Runs Pathfinding algorithm.
        /// </summary>
        /// <param name="origin">Origin / Current state</param>
        /// <param name="destination">Destination / Goal state</param>
        /// <param name="getHeuristic">(Current, Goal) -> Heuristic cost
        ///     <para>Get cost (H) from current to goal.</para>
        ///     <para>Calculate how far is the current state from the destination.</para>
        /// </param>
        /// <param name="doesSatisfy">(Current) -> Satisfies
        ///     <para>Check if we are in destination</para>
        /// </param>
        /// <param name="getNeighbours">(Current) -> (Neighbour, G Cost)[]
        ///     <para>Gets the next nodes coming from the current one. Returns null if it has no neighbours.</para>
        ///     <para>Also known as Explode or Expand.</para>
        /// </param>
        /// <param name="maxIterationsPerCycle"></param>
        /// <returns>A path result</returns>
        public IEnumerable<PathResult> FindPath
        (
            TNode origin,
            TNode destination,
            Func<TNode, TNode, float> getHeuristic,
            Func<TNode, bool> doesSatisfy,
            Func<TNode, IEnumerable<Arc>> getNeighbours,
            int maxIterationsPerCycle = 50
        )
        {
            var initialPath = new Path();
            initialPath.OpenNodes.Add(origin);
            initialPath.GCostByNode[origin] = 0;
            initialPath.FCostByNode[origin] = getHeuristic(origin, destination);
            initialPath.PreviousNodeTo[origin] = null;
            initialPath.Current = origin;

            var path = initialPath;
            while (path.HasOpenNodes && !path.IsComplete)
            {
                for (int i = 0; i < maxIterationsPerCycle; i++)
                {
                    path = path.Clone();

                    var candidate = GetCandidateWithLeastFCost(path);
                    path.Current = candidate;

                    if (doesSatisfy(candidate))
                    {
                        path.IsComplete = true;
                        break;
                    }

                    path.CloseNode(candidate);
                    
                    var neighbours = getNeighbours(candidate);
                    
                    if (DoesntHave(neighbours))
                        continue;

                    var gCostCandidate = path.GCostByNode[candidate];

                    foreach (var arcToNeighbour in neighbours)
                    {
                        if (arcToNeighbour.IsClosed(path))
                            continue;

                        var arcGCost = gCostCandidate + arcToNeighbour.GCost;
                        path.OpenNodes.Add(arcToNeighbour.Endpoint);

                        if (arcToNeighbour.IsMoreEfficient(path, arcGCost))
                        {
                            var arcFCost = arcGCost + getHeuristic(arcToNeighbour.Endpoint, destination);
                            path.ReplacePrevious(arcToNeighbour.Endpoint,
                                                 candidate,
                                                 arcGCost,
                                                 arcFCost);
                        }
                    }
                }

                var tempPath = path;
                yield return new PathResult(new Lazy<IEnumerable<TNode>>(() => tempPath.GetSteps()),
                                            path.IsComplete);
            }

            yield return new PathResult(new Lazy<IEnumerable<TNode>>(() => path.GetSteps()),
                                        path.IsComplete);
            yield break;

            static TNode GetCandidateWithLeastFCost(Path path)
                => path.OpenNodes.OrderBy(x => path.FCostByNode[x]).First();
            
            static bool DoesntHave(IEnumerable<Arc> collection)
                => collection == null || !collection.Any();
        }

        /// <summary>
        /// Arc or edge between two nodes/vertices/points
        /// </summary>
        public readonly struct Arc
        {
            /// <summary>
            /// Connection node.
            /// </summary>
            public readonly TNode Endpoint;
            /// <summary>
            /// Cost to endpoint
            /// </summary>
            public readonly float GCost;

            public Arc(TNode ep, float gCost)
            {
                Endpoint = ep;
                GCost = gCost;
            }

            /// <summary>
            /// Is <see cref="Endpoint"/> in the <see cref="Path.ClosedNodes"/> collection?
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            internal bool IsClosed(Path path) => path.ClosedNodes.Contains(Endpoint);

            /// <summary>
            /// Is the new g cost less or equal the current one?
            /// </summary>
            /// <param name="path"></param>
            /// <param name="newGCost"></param>
            /// <returns></returns>
            internal bool IsMoreEfficient(Path path, float newGCost)
            {
                var myCurrentGCost = path.GCostByNode.GetValueOrDefault(Endpoint, float.MaxValue);
                return newGCost <= myCurrentGCost;
            }
        }

        /// <summary>
        /// Represents the state of an A* path
        /// </summary>
        public class Path
        {
            /// <summary>
            /// Discovered nodes that have not yet been evaluated
            /// </summary>
            public readonly HashSet<TNode> OpenNodes;
            /// <summary>
            /// Nodes that have been evaluated already
            /// </summary>
            public readonly HashSet<TNode> ClosedNodes;
            /// <summary>
            /// Cost from current to [neighbour]
            /// </summary>
            public readonly Dictionary<TNode, float> GCostByNode;
            /// <summary>
            /// Total cost from current to goal using [neighbour] (F = G + H)
            /// </summary>
            public readonly Dictionary<TNode, float> FCostByNode;
            /// <summary>
            /// The previous node to each one.
            /// </summary>
            public readonly Dictionary<TNode, TNode> PreviousNodeTo;
            /// <summary>
            /// Used for iteration
            /// </summary>
            internal TNode Current;
            
            /// <summary>
            /// Is this path complete from origin to Destination?
            /// </summary>
            public bool IsComplete { get; internal set; }

            internal bool HasOpenNodes => OpenNodes.Any();

            public Path()
            {
                OpenNodes = new HashSet<TNode>();
                ClosedNodes = new HashSet<TNode>();
                GCostByNode = new Dictionary<TNode, float>();
                FCostByNode = new Dictionary<TNode, float>();
                PreviousNodeTo = new Dictionary<TNode, TNode>();
                Current = default;
                IsComplete = false;
            }

            public Path(Path original)
            {
                OpenNodes = new HashSet<TNode>(original.OpenNodes);
                ClosedNodes = new HashSet<TNode>(original.ClosedNodes);
                GCostByNode = new Dictionary<TNode, float>(original.GCostByNode);
                FCostByNode = new Dictionary<TNode, float>(original.FCostByNode);
                PreviousNodeTo = new Dictionary<TNode, TNode>(original.PreviousNodeTo);
                Current = original.Current;
                IsComplete = original.IsComplete;
            }

            public Path Clone() => new(this);

            /// <summary>
            /// Get this path's nodes/steps
            /// </summary>
            /// <returns>Path nodes in order</returns>
            public IEnumerable<TNode> GetSteps()
            {
                return Generate(Current, node => PreviousNodeTo[node])
                    .Reverse();
            }

            /// <summary>
            /// Removes <see cref="node"/> from <see cref="OpenNodes"/> and adds it to <see cref="ClosedNodes"/>
            /// </summary>
            /// <param name="node"></param>
            internal void CloseNode(TNode node)
            {
                OpenNodes.Remove(node);
                ClosedNodes.Add(node);
            }

            /// <summary>
            /// Replaces the previous node to a given one. Use this when finding more efficient arcs between nodes
            /// </summary>
            /// <param name="node"></param>
            /// <param name="newPrevious"></param>
            /// <param name="gCost"></param>
            /// <param name="fCost"></param>
            internal void ReplacePrevious(TNode node, TNode newPrevious, float gCost, float fCost)
            {
                PreviousNodeTo[node] = newPrevious;
                GCostByNode[node] = gCost;
                FCostByNode[node] = fCost;
            }
            
            /// <summary>
            /// Helper method for accum with IEnumerable
            /// </summary>
            /// <param name="seed"></param>
            /// <param name="mutate"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            private static IEnumerable<T> Generate<T>(T seed, Func<T, T> mutate)
            {
                var accum = seed;
                while (accum != null)
                {
                    yield return accum;
                    accum = mutate(accum);
                }
            }
        }
        public readonly struct PathResult
        {
            /// <summary>
            /// Lazy collection of the steps to follow
            /// </summary>
            public Lazy<IEnumerable<TNode>> Steps { get; }
            /// <summary>
            /// Is this path valid and complete between origin and destination?
            /// </summary>
            public bool IsValid { get; }

            public PathResult(Lazy<IEnumerable<TNode>> steps, bool isValid)
            {
                Steps = steps;
                IsValid = isValid;
            }
        }
    }
    
}