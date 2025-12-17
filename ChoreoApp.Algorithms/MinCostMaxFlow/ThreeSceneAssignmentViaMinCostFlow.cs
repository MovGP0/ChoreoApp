using System.Numerics;

namespace ChoreoApp.Algorithms.MinCostMaxFlow
{
    public static class ThreeSceneAssignmentViaMinCostFlow
    {
        public static int[] Solve(
            IReadOnlyList<Vector2> sceneA,
            IReadOnlyList<Vector2> sceneB,
            IReadOnlyList<Vector2> sceneC,
            float sceneBFraction = 0.5f,
            Func<Vector2, Vector2, Vector2, float>? costFunc = null,
            Func<int, int, bool>? isAllowedPair = null)
        {
            ArgumentNullException.ThrowIfNull(sceneA);
            ArgumentNullException.ThrowIfNull(sceneB);
            ArgumentNullException.ThrowIfNull(sceneC);

            int count = sceneA.Count;
            if (count != sceneB.Count || count != sceneC.Count)
            {
                throw new ArgumentException("All scene position sets must have the same length.");
            }

            if (count == 0)
            {
                return [];
            }

            if (sceneBFraction < 0.0f || sceneBFraction > 1.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(sceneBFraction), "sceneBFraction must be in [0,1].");
            }

            costFunc ??= (start, mid, end) =>
            {
                Vector2 expectedMid = Vector2.Lerp(start, end, sceneBFraction);
                float pathEnergy = (mid - start).LengthSquared() + (end - mid).LengthSquared();
                float deviationEnergy = (mid - expectedMid).LengthSquared();
                return pathEnergy + deviationEnergy;
            };

            isAllowedPair ??= (_, _) => true;

            int sourceNode = 0;
            int firstStartNode = 1;
            int firstMidNode = firstStartNode + count;
            int sinkNode = firstMidNode + count;
            int nodeCount = sinkNode + 1;

            var solver = new MinCostMaxFlowSolver(nodeCount);

            for (int startIndex = 0; startIndex < count; startIndex++)
            {
                int startNode = firstStartNode + startIndex;
                solver.AddEdge(sourceNode, startNode, 1, 0.0f);
            }

            for (int startIndex = 0; startIndex < count; startIndex++)
            {
                int startNode = firstStartNode + startIndex;
                Vector2 start = sceneA[startIndex];
                Vector2 end = sceneC[startIndex];

                for (int midIndex = 0; midIndex < count; midIndex++)
                {
                    if (!isAllowedPair(startIndex, midIndex))
                    {
                        continue;
                    }

                    int midNode = firstMidNode + midIndex;
                    float cost = costFunc(start, sceneB[midIndex], end);
                    solver.AddEdge(startNode, midNode, 1, cost);
                }
            }

            for (int midIndex = 0; midIndex < count; midIndex++)
            {
                int midNode = firstMidNode + midIndex;
                solver.AddEdge(midNode, sinkNode, 1, 0.0f);
            }

            var (sentFlow, _) = solver.ComputeMinCostFlow(sourceNode, sinkNode, count);
            if (sentFlow != count)
            {
                throw new InvalidOperationException("No perfect assignment exists with the provided constraints.");
            }

            var assignment = new int[count];
            Array.Fill(assignment, -1);

            foreach (var (fromNode, toNode, _, _, reverseResidualCapacity) in solver.EnumerateForwardEdges())
            {
                bool fromIsStart = fromNode >= firstStartNode && fromNode < firstStartNode + count;
                bool toIsMid = toNode >= firstMidNode && toNode < firstMidNode + count;
                if (!fromIsStart || !toIsMid)
                {
                    continue;
                }

                if (reverseResidualCapacity == 1)
                {
                    int startIndex = fromNode - firstStartNode;
                    int midIndex = toNode - firstMidNode;
                    assignment[startIndex] = midIndex;
                }
            }

            for (int startIndex = 0; startIndex < count; startIndex++)
            {
                if (assignment[startIndex] < 0)
                {
                    throw new InvalidOperationException("Assignment decoding failed.");
                }
            }

            return assignment;
        }
    }
}
