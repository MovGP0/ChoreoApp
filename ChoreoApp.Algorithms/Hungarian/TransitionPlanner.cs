using System.Numerics;

namespace ChoreoApp.Algorithms.Hungarian;

public static class TransitionPlanner
{
    /// <summary>
    /// Computes a bijection sigma where each <c>initial[i]</c> moves to <c>target[sigma[i]]</c>.
    /// <list type="bullet">
    /// <item>Primary objective: minimize the maximum travel distance.</item>
    /// <item>Secondary objective: among those, minimize total energy.</item>
    /// </list>
    /// </summary>
    /// <param name="initial">
    /// Initial positions of the moving points (one entry per point).
    /// </param>
    /// <param name="target">
    /// Target positions for the same points, in arbitrary order.
    /// </param>
    /// <param name="energyFunc">
    /// The distance dependent energy function
    /// <c>energyFunc(distance)</c> can be e.g.
    /// <list type="bullet">
    /// <item>the squared distance d² (<c>d => d * d</c>)</item>
    /// <item>an exponential discounting of the distance exp(α•d) (<c>d => Math.Exp(alpha * d)</c>)</item>
    /// </list>
    /// </param>
    /// <returns>
    /// An array sigma where sigma[i] is the assigned target index for initial[i].
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the point sets differ in size.
    /// </exception>
    public static int[] ComputeAssignment(
        IReadOnlyList<Vector2> initial,
        IReadOnlyList<Vector2> target,
        Func<float, float>? energyFunc = null)
    {
        ArgumentNullException.ThrowIfNull(initial);
        ArgumentNullException.ThrowIfNull(target);

        int n = initial.Count;
        if (n != target.Count)
        {
            throw new ArgumentException("Point sets must have equal size.");
        }

        if (n == 0)
        {
            return [];
        }

        energyFunc ??= d => d * d; // default: squared distance

        // Precompute distances using shared helper
        Vector2[] initialArray = initial as Vector2[] ?? initial.ToArray();
        Vector2[] targetArray = target as Vector2[] ?? target.ToArray();

        float[,] squaredDistances = DistanceHelpers.FillSquaredDistances(initialArray, targetArray);

        float[,] distances = new float[n, n];
        var allDistances = new List<float>(n * n);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                float d = MathF.Sqrt(squaredDistances[i, j]);
                distances[i, j] = d;
                allDistances.Add(d);
            }
        }

        allDistances.Sort();

        // Bottleneck assignment: find minimal possible max distance
        float minMaxDistance = FindMinimalMaxDistance(distances, allDistances);

        // Within that radius, minimize total energy using Hungarian algorithm.
        // We enforce that edges with d > minMaxDistance are forbidden (cost = +∞).
        float[,] costMatrix = new float[n, n];
        float bigM = 1e12f; // sufficiently large "infinite" cost

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                float d = distances[i, j];
                costMatrix[i, j] = d <= minMaxDistance ? energyFunc(d) : bigM;
            }
        }

        return HungarianAlgorithm.ComputeSigmaVector(costMatrix);
    }

    /// <summary>
    /// Finds the smallest radius R that still allows a perfect matching so that
    /// every matched pair (i,j) satisfies distances[i,j] &lt;= R.
    /// </summary>
    private static float FindMinimalMaxDistance(float[,] distances, List<float> sortedDistances)
    {
        int left = 0;
        int right = sortedDistances.Count - 1;
        float best = sortedDistances[right];

        while (left <= right)
        {
            int mid = (left + right) / 2;
            float threshold = sortedDistances[mid];

            if (TryFindPerfectMatching(distances, threshold))
            {
                best = threshold;
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
        }

        return best;
    }

    /// <summary>
    /// Tests whether the bipartite graph limited to edges with distances[i,j] &lt;= threshold
    /// admits a perfect matching.
    /// </summary>
    private static bool TryFindPerfectMatching(float[,] distances, float threshold)
    {
        int n = distances.GetLength(0);

        int[] matchToLeft = new int[n]; // right j -> left i
        for (int j = 0; j < n; j++)
        {
            matchToLeft[j] = -1;
        }

        for (int i = 0; i < n; i++)
        {
            bool[] visited = new bool[n];
            if (!Augment(i, distances, threshold, matchToLeft, visited))
            {
                return false; // cannot match this left node under current threshold
            }
        }

        return true; // matched all left nodes -> perfect matching exists
    }

    /// <summary>
    /// Depth-first search style augmentation step used by <see cref="TryFindPerfectMatching"/>
    /// to grow the current matching within the allowed threshold.
    /// </summary>
    private static bool Augment(
        int leftNode,
        float[,] distances,
        float threshold,
        int[] matchToLeft,
        bool[] visited)
    {
        int n = distances.GetLength(0);

        for (int j = 0; j < n; j++)
        {
            if (visited[j])
            {
                continue;
            }

            if (distances[leftNode, j] > threshold)
            {
                continue;
            }

            visited[j] = true;

            // If right node j is free, or we can reassign its current partner
            // recursively, then match leftNode to j.
            if (matchToLeft[j] == -1 ||
                Augment(matchToLeft[j], distances, threshold, matchToLeft, visited))
            {
                matchToLeft[j] = leftNode;
                return true;
            }
        }

        return false;
    }
}
