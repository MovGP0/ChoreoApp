namespace ChoreoApp.Algorithms.MinCostMaxFlow;

public sealed class MinCostMaxFlowSolver
{
    private const float Epsilon = 1e-6f;
    private readonly List<Edge>[] _adjacency;

    public int NodeCount => _adjacency.Length;

    public MinCostMaxFlowSolver(int nodeCount)
    {
        if (nodeCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nodeCount));
        }

        _adjacency = new List<Edge>[nodeCount];
        for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
        {
            _adjacency[nodeIndex] = [];
        }
    }

    /// <summary>
    /// Adds a directed edge with given capacity and cost.
    /// A reverse edge is automatically added with 0 capacity and -cost.
    /// </summary>
    public void AddEdge(int fromNode, int toNode, int capacity, float cost)
    {
        if ((uint)fromNode >= (uint)NodeCount) throw new ArgumentOutOfRangeException(nameof(fromNode));
        if ((uint)toNode >= (uint)NodeCount) throw new ArgumentOutOfRangeException(nameof(toNode));
        if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        if (float.IsNaN(cost) || float.IsInfinity(cost)) throw new ArgumentOutOfRangeException(nameof(cost));

        var forwardEdge = new Edge(fromNode, toNode, capacity, cost);
        var reverseEdge = new Edge(toNode, fromNode, 0, -cost);

        forwardEdge.ReverseEdge = reverseEdge;
        reverseEdge.ReverseEdge = forwardEdge;

        _adjacency[fromNode].Add(forwardEdge);
        _adjacency[toNode].Add(reverseEdge);
    }

    /// <summary>
    /// Sends up to requestedFlow units from sourceNode to sinkNode minimizing total cost.
    /// Returns (sentFlow, minCost).
    ///
    /// Requirements for "production safe" use:
    /// - Works best when costs are non-negative on forward edges (typical for distance costs).
    /// - Supports negative costs internally due to reverse edges; potentials handle this.
    /// </summary>
    public (int sentFlow, float minCost) ComputeMinCostFlow(int sourceNode, int sinkNode, int requestedFlow)
    {
        if ((uint)sourceNode >= (uint)NodeCount) throw new ArgumentOutOfRangeException(nameof(sourceNode));
        if ((uint)sinkNode >= (uint)NodeCount) throw new ArgumentOutOfRangeException(nameof(sinkNode));
        if (requestedFlow < 0) throw new ArgumentOutOfRangeException(nameof(requestedFlow));
        if (sourceNode == sinkNode) throw new ArgumentException("sourceNode and sinkNode must be different.");

        // Potentials (Johnson reweighting). Ensures non-negative reduced costs for Dijkstra.
        var potentials = new float[NodeCount];

        // Standard Dijkstra state
        var distances = new float[NodeCount];
        var previousEdge = new Edge?[NodeCount];

        int totalSentFlow = 0;
        float totalCost = 0.0f;

        // We can initialize potentials to 0 because:
        // - all forward costs are typically >= 0 in assignment (distance-based)
        // - reduced costs start non-negative; reverse edges might be negative but have 0 capacity initially.
        while (totalSentFlow < requestedFlow)
        {
            Array.Fill(distances, float.PositiveInfinity);
            Array.Fill(previousEdge, null);

            distances[sourceNode] = 0.0f;

            var priorityQueue = new PriorityQueue<int, float>();
            priorityQueue.Enqueue(sourceNode, 0.0f);

            while (priorityQueue.TryDequeue(out int currentNode, out float currentDistance))
            {
                if (Math.Abs(currentDistance - distances[currentNode]) > Epsilon)
                {
                    continue;
                }

                // Early exit is safe for Dijkstra when we reach sink.
                if (currentNode == sinkNode)
                {
                    break;
                }

                foreach (var edge in _adjacency[currentNode])
                {
                    if (edge.Capacity <= 0)
                    {
                        continue;
                    }

                    // Reduced cost: c'(u,v) = c(u,v) + pot[u] - pot[v]
                    float reducedCost = edge.Cost + potentials[currentNode] - potentials[edge.ToNode];

                    // Dijkstra requires non-negative weights; reducedCost should be >= 0 if potentials are valid.
                    // With typical distance costs, this holds.
                    float candidateDistance = currentDistance + reducedCost;

                    if (candidateDistance < distances[edge.ToNode])
                    {
                        distances[edge.ToNode] = candidateDistance;
                        previousEdge[edge.ToNode] = edge;
                        priorityQueue.Enqueue(edge.ToNode, candidateDistance);
                    }
                }
            }

            if (previousEdge[sinkNode] is null)
            {
                // No augmenting path remaining -> cannot send more flow.
                break;
            }

            // Update potentials: pot[v] += dist[v] for all reachable v
            // This preserves reduced-cost non-negativity in future iterations.
            for (int nodeIndex = 0; nodeIndex < NodeCount; nodeIndex++)
            {
                if (!float.IsInfinity(distances[nodeIndex]))
                {
                    potentials[nodeIndex] += distances[nodeIndex];
                }
            }

            // Determine augmenting flow along found path (usually 1 for assignment)
            int augmentingFlow = requestedFlow - totalSentFlow;
            for (int nodeIndex = sinkNode; nodeIndex != sourceNode;)
            {
                var edge = previousEdge[nodeIndex]!;
                augmentingFlow = Math.Min(augmentingFlow, edge.Capacity);
                nodeIndex = edge.FromNode;
            }

            // Apply augmentation and accumulate true cost (original costs, not reduced costs)
            for (int nodeIndex = sinkNode; nodeIndex != sourceNode;)
            {
                var edge = previousEdge[nodeIndex]!;
                edge.Capacity -= augmentingFlow;
                edge.ReverseEdge?.Capacity += augmentingFlow;

                totalCost += augmentingFlow * edge.Cost;

                nodeIndex = edge.FromNode;
            }

            totalSentFlow += augmentingFlow;
        }

        return (totalSentFlow, totalCost);
    }

    /// <summary>
    /// Enumerates all original (forward) edges currently in the graph.
    /// Useful for decoding assignments from residual capacities.
    /// </summary>
    public IEnumerable<(int fromNode, int toNode, int residualCapacity, float cost, int reverseResidualCapacity)> EnumerateForwardEdges()
    {
        for (int fromNode = 0; fromNode < NodeCount; fromNode++)
        {
            foreach (var edge in _adjacency[fromNode])
            {
                // A forward edge is identified by having a reverse edge with negative cost.
                // (Because reverse edge cost = -forward cost)
                if (edge.ReverseEdge is not null && edge.Cost >= 0 && edge.ReverseEdge.Cost <= 0)
                {
                    yield return (edge.FromNode, edge.ToNode, edge.Capacity, edge.Cost, edge.ReverseEdge.Capacity);
                }
            }
        }
    }
}
