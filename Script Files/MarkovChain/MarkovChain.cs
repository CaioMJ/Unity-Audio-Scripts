using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkovChain : MonoBehaviour
{
    [SerializeField] private TwoDimensionalFloatArray probabilities;

    public int Run(int nextElement)
    {
        float probability = Random.Range(0f, 1f);
        int previousElement = nextElement;
        nextElement = 0;
        float accumulator = probabilities.input[previousElement].output[nextElement];

        while (accumulator < probability)
        {
            nextElement++;
            accumulator += probabilities.input[previousElement].output[nextElement];
        }

        return nextElement;
    }
}
