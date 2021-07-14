using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkovChain : MonoBehaviour
{
    //Scriptable Object class that holds a 2-D array of floats
    [SerializeField] private TwoDimensionalFloatArray probabilities;

    public int Run(int previousElement)
    {
        float probability = Random.Range(0f, 1f);
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
