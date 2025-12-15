using System.Numerics;

namespace ChoreoApp.Algorithms;

public static class DistanceHelpers
{
    /// <summary>
    /// Builds a dense n x n matrix of squared Euclidean distances where entry (i,j)
    /// corresponds to the squared distance between <paramref name="initial"/>[i] and
    /// <paramref name="target"/>[j]. The two point sets must have identical length.
    /// </summary>
    /// <param name="initial">
    /// Positions of moving points before the transition.
    /// </param>
    /// <param name="target">
    /// Desired positions of the same points after the transition.
    /// </param>
    /// <returns>
    /// A newly allocated float matrix sized <c>n x n</c> containing squared distances.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the two spans do not have equal length.
    /// </exception>
    public static float[,] FillSquaredDistances(
        ReadOnlySpan<Vector2> initial,
        ReadOnlySpan<Vector2> target)
    {
        if (target.Length != initial.Length)
        {
            throw new ArgumentException("Point sets must have equal size.");
        }

        int n = initial.Length;
        float[,] distances = new float[n, n];

        for (int i = 0; i < n; i++)
        {
            ref readonly Vector2 a = ref initial[i];
            for (int j = 0; j < n; j++)
            {
                distances[i, j] = a.SquaredDistanceTo(in target[j]);
            }
        }

        return distances;
    }
}
