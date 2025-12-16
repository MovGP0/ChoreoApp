using System.Numerics;

namespace ChoreoApp.Algorithms.MinCostMaxFlow;

public static class AssignmentViaMinCostFlow
{
    /// <summary>
    /// Returns sigma where sigma[initialIndex] = targetIndex.
    /// Minimizes sum of costFunc(distance(initial, target)).
    /// </summary>
    public static int[] Solve(
        IReadOnlyList<Vector2> initialPoints,
        IReadOnlyList<Vector2> targetPoints,
        Func<float, float>? costFunc = null,
        Func<int, int, bool>? isAllowedPair = null)
    {
        if (initialPoints is null) throw new ArgumentNullException(nameof(initialPoints));
        if (targetPoints is null) throw new ArgumentNullException(nameof(targetPoints));
        if (initialPoints.Count != targetPoints.Count) throw new ArgumentException("Point sets must have equal size.");

        int pointCount = initialPoints.Count;
        if (pointCount == 0) return Array.Empty<int>();

        costFunc ??= distance => distance * distance; // default: squared distance
        isAllowedPair ??= (_, _) => true;

        int sourceNode = 0;
        int firstInitialNode = 1;
        int firstTargetNode = firstInitialNode + pointCount;
        int sinkNode = firstTargetNode + pointCount;
        int nodeCount = sinkNode + 1;

        var minCostFlow = new MinCostMaxFlowSolver(nodeCount);

        // Source -> initial nodes
        for (int initialIndex = 0; initialIndex < pointCount; initialIndex++)
        {
            int initialNode = firstInitialNode + initialIndex;
            minCostFlow.AddEdge(sourceNode, initialNode, capacity: 1, cost: 0.0f);
        }

        // Initial -> target nodes with cost
        for (int initialIndex = 0; initialIndex < pointCount; initialIndex++)
        {
            int initialNode = firstInitialNode + initialIndex;

            for (int targetIndex = 0; targetIndex < pointCount; targetIndex++)
            {
                if (!isAllowedPair(initialIndex, targetIndex))
                    continue; // forbidden pair: simply omit this edge

                int targetNode = firstTargetNode + targetIndex;
                float distance = initialPoints[initialIndex].DistanceTo(targetPoints[targetIndex]);
                float cost = costFunc(distance);

                // Cap 1 => assignment
                minCostFlow.AddEdge(initialNode, targetNode, capacity: 1, cost: cost);
            }
        }

        // Target -> sink
        for (int targetIndex = 0; targetIndex < pointCount; targetIndex++)
        {
            int targetNode = firstTargetNode + targetIndex;
            minCostFlow.AddEdge(targetNode, sinkNode, capacity: 1, cost: 0.0f);
        }

        var (sentFlow, minCost) = minCostFlow.ComputeMinCostFlow(sourceNode, sinkNode, requestedFlow: pointCount);
        if (sentFlow != pointCount)
        {
            throw new InvalidOperationException("No perfect assignment exists under the given constraints.");
        }

        // Decode assignment:
        // An initial->target edge is used if its reverse edge residual capacity is 1 (flow pushed).
        var assignment = new int[pointCount];
        Array.Fill(assignment, -1);

        foreach (var (fromNode, toNode, residualCapacity, cost, reverseResidualCapacity) in minCostFlow.EnumerateForwardEdges())
        {
            bool fromIsInitial = fromNode >= firstInitialNode && fromNode < firstInitialNode + pointCount;
            bool toIsTarget = toNode >= firstTargetNode && toNode < firstTargetNode + pointCount;
            if (!fromIsInitial || !toIsTarget)
                continue;

            // If we sent 1 unit along forward edge, reverse edge capacity becomes 1.
            if (reverseResidualCapacity == 1)
            {
                int initialIndex = fromNode - firstInitialNode;
                int targetIndex = toNode - firstTargetNode;
                assignment[initialIndex] = targetIndex;
            }
        }

        // Sanity check: all assigned
        for (int initialIndex = 0; initialIndex < pointCount; initialIndex++)
        {
            if (assignment[initialIndex] < 0)
                throw new InvalidOperationException("Assignment decoding failed.");
        }

        return assignment;
    }
}
