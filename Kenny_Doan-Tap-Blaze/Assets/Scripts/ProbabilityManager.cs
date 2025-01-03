using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProbabilityManager
{
    /// <summary>
    /// Given the weights of the prizes, chose the indexes the weights based on the value of the weights
    /// </summary>
    /// <returns>Array of indexes that have been chosen</returns>
    public static int[] PickIndexes(float[] weights, int count)
    {
        int[] indexes = new int[count];

        float totalWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];
        }

        for (int i = 0; i < indexes.Length; i++)
        {
            float cumulative = 0;
            float randomValue = Random.value * totalWeight;
            for (int j = 0; j < weights.Length; j++)
            {
                cumulative += weights[j];
                if (randomValue <= cumulative)
                {
                    indexes[i] = j;
                    break;
                }
            }
        }

        return indexes;
    }
}
