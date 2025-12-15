using System.Numerics;
using System.Runtime.CompilerServices;

namespace ChoreoApp.Algorithms;

public static class Vector2Extensions
{
    extension (Vector2 vector)
    {
        /// <summary>
        /// Computes the squared Euclidean distance to <paramref name="other"/>.
        /// Useful when the square root is not needed and performance matters.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float SquaredDistanceTo(in Vector2 other)
        {
            var diff = other - vector;
            return diff.LengthSquared();
        }

        /// <summary>
        /// Computes the Euclidean distance to <paramref name="other"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float EuclideanDistanceTo(in Vector2 other)
        {
            var diff = other - vector;
            return diff.Length();
        }
    }
}
