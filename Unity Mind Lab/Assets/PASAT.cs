using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PASAT : MonoBehaviour
{
    public Transform responses;
    public GameObject buttonPrefab;
    public TextMeshProUGUI stimuliText;
    public int maxSumValue;
    public float stimulusInterval;
    private List<int> stimuliValues = new List<int>();
    private List<int> sumValues = new List<int>();
    private int stimuliIndex = 0;
    private int currentSum = 0;
    private int currentStimuli = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateStimuli());
        GenerateButtons();
    }

    // Function that generates the Stimuli 
    IEnumerator GenerateStimuli()
    {
        while (true)
        {
            if (stimuliValues.Count == 0)
            {
                // If list is empty, create initial stimuli value
                currentStimuli = UnityEngine.Random.Range(1, maxSumValue);  // Assigns currentStimuli to a random value: 1 - maxSumValue
                stimuliText.text = currentStimuli.ToString();               // Sets stimuli text to the currentStimuli value 
                stimuliValues.Add(currentStimuli);                          // Adds stimuli to the stimuliValues list 
                stimuliIndex++;                                             // Integrates stimuliIndex
            }
            else 
            {
                // Display new stimuli every [stimulusInterval] seconds
                yield return new WaitForSeconds(stimulusInterval);                                             // Waits [stimulusInterval] seconds
                currentStimuli = UnityEngine.Random.Range(1, maxSumValue - stimuliValues[stimuliIndex - 1]);   // Assigns currentStimuli to a random value: 1 - Max value possible for sum to be less than maxSumValue
                currentSum = currentStimuli + stimuliValues[stimuliIndex - 1];                                 // Calculates sum of currentStimuli and previous stimuli
                stimuliText.text = currentStimuli.ToString();                                                  // Sets stimuli text to the currentStimuli value
                stimuliValues.Add(currentStimuli);                                                             // Adds stimuli to the stimuliValues list 
                stimuliIndex++;                                                                                // Integrates stimuliIndex
            }
        }
    }

    // Function that generates the buttons based on the maxSumValue
    void GenerateButtons()
    {
        // Deletes existing buttons
        foreach (Transform child in responses)
        {
            Destroy(child.gameObject);
        }

        // Creates row of buttons from 1 - maxSumValue
        for (int i = 1; i <= maxSumValue; i++)
        {
            // Creates a button object child under responses object and sets text to i
            GameObject button = Instantiate(buttonPrefab.gameObject, responses);
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = i.ToString();
            // Creates a row of buttons
            // To Do: Make buttons centered and able to be divided into rows
            float xPos = i * button.GetComponent<RectTransform>().sizeDelta.x;
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0);
        }
    }
}
