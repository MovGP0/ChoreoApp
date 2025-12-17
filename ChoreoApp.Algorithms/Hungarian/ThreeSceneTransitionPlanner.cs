using System.Numerics;

namespace ChoreoApp.Algorithms.Hungarian
{
    public static class ThreeSceneTransitionPlanner
    {
        public static int[] ComputeMidSceneAssignment(
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

            const float bigM = 1e12f;
            float[,] costMatrix = new float[count, count];

            for (int dancerIndex = 0; dancerIndex < count; dancerIndex++)
            {
                Vector2 start = sceneA[dancerIndex];
                Vector2 end = sceneC[dancerIndex];

                for (int candidateIndex = 0; candidateIndex < count; candidateIndex++)
                {
                    float cost = isAllowedPair(dancerIndex, candidateIndex)
                        ? costFunc(start, sceneB[candidateIndex], end)
                        : bigM;

                    costMatrix[dancerIndex, candidateIndex] = cost;
                }
            }

            return HungarianAlgorithm.ComputeSigmaVector(costMatrix);
        }
    }
}
