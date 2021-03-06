using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//What this script does?
//This script uses a Markov chain (which is basically an aleatoric generator where the output is dependent on its input) to play audio clips randomly form an array.
//The original intent was to make it so the clips in the array are samples of individual notes and have this script generate a melody aleatorically through the Markov chain.
//It works by defining probabilities for each output that are dependent on the input. What this means is that you can make it so (for example) the next note after a G4 
//has a 50% chance of being D4, 25% of being a F#4, 25% of being an A4 and 0% of being a C4

//HOW TO USE THIS:
//1) Fill up the Audio Clips array with the desired files in the inspector

//2) Declare the values for the probabilities array in the inspector:
    //a) The number of Input Note elements should be the equal to how many audio clips you're using
    //b) The number of Output Note elements inside each Input Note element should also be equal to how many audio clips you're using
    //c) The value set for each Output Note element is the probability (between 0 and 1) that element's index will be generated for each Input Note element
    //d) The sum of all elements for each Output Note array should be equal to 1

//3) Call RunMarkovChain() to play the next audio clip from the array according to the defined probabilities
    //a) I set it to call the function when you press 'm' just for testing purposes

[RequireComponent(typeof(AudioSource))]
public class AudioClipMarkovChain : MonoBehaviour
{
    private AudioSource aSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private ProbabilitiesArray probabilities;
    private int previousElement, nextElement;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
        nextElement = Random.Range(0, audioClips.Length);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            RunMarkovChain();
        }
    }

    public void RunMarkovChain()
    {
        aSource.PlayOneShot(audioClips[nextElement]);
        print("MARKOV INDEX: " + nextElement);

        float probability = Random.Range(0f, 1f);
        previousElement = nextElement;
        nextElement = 0;
        float accumulator = probabilities.inputNote[previousElement].outputNote[nextElement];

        while (accumulator < probability)
        {
            nextElement++;
            accumulator += probabilities.inputNote[previousElement].outputNote[nextElement];
        }
    }
}

[System.Serializable]
public class ProbabilitiesArray
{
    [System.Serializable]
    public struct rowData
    {
        public float[] outputNote;
    }
    public rowData[] inputNote = new rowData[9]; //Grid of 9x9
}
