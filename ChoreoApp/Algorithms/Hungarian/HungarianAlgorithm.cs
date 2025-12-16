namespace ChoreoApp.Algorithms.Hungarian;

public static class HungarianAlgorithm
{
    /// <summary>
    /// Solves the assignment problem for a square cost matrix using the Hungarian algorithm.
    /// Rows represent sources, columns represent targets; each row/column is matched exactly once.
    /// </summary>
    /// <param name="costMatrix">
    /// Square matrix where <c>costMatrix[i,j]</c> is the cost of assigning row <c>i</c> to column <c>j</c>.
    /// </param>
    /// <returns>
    /// An array <c>sigma</c> of length <c>n</c> where <c>sigma[row]</c> is the chosen column index.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided matrix is not square.
    /// </exception>
    public static int[] ComputeSigmaVector(float[,] costMatrix)
    {
        int nRows = costMatrix.GetLength(0);
        int nCols = costMatrix.GetLength(1);
        if (nRows != nCols)
        {
            throw new ArgumentException("HungarianAlgorithm requires a square matrix.");
        }

        int n = nRows;
        float[] u = new float[n + 1];
        float[] v = new float[n + 1];
        int[] p = new int[n + 1];
        int[] way = new int[n + 1];

        for (int i = 1; i <= n; i++)
        {
            p[0] = i;
            int j0 = 0;
            float[] minv = new float[n + 1];
            bool[] used = new bool[n + 1];

            for (int j = 0; j <= n; j++)
            {
                minv[j] = float.PositiveInfinity;
            }

            do
            {
                used[j0] = true;
                int i0 = p[j0];
                float delta = float.PositiveInfinity;
                int j1 = 0;

                for (int j = 1; j <= n; j++)
                {
                    if (used[j])
                    {
                        continue;
                    }

                    float cur = costMatrix[i0 - 1, j - 1] - u[i0] - v[j];
                    if (cur < minv[j])
                    {
                        minv[j] = cur;
                        way[j] = j0;
                    }

                    if (minv[j] < delta)
                    {
                        delta = minv[j];
                        j1 = j;
                    }
                }

                for (int j = 0; j <= n; j++)
                {
                    if (used[j])
                    {
                        u[p[j]] += delta;
                        v[j] -= delta;
                    }
                    else
                    {
                        minv[j] -= delta;
                    }
                }

                j0 = j1;
            }
            while (p[j0] != 0);

            // Augmenting
            do
            {
                int j1 = way[j0];
                p[j0] = p[j1];
                j0 = j1;
            }
            while (j0 != 0);
        }

        int[] result = new int[n];
        for (int j = 1; j <= n; j++)
        {
            int i = p[j];
            if (i > 0 && i <= n)
            {
                result[i - 1] = j - 1;
            }
        }

        return result;
    }
}
