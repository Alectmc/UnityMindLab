using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PASAT : MonoBehaviour
{
    public Transform responses;
    public GameObject buttonPrefab;
    public TextMeshProUGUI stimuliText;
    public int maxSumValue;
    public float trialTime; // Used for time available for test trials in minutes
    public float stimulusInterval; // Used for the interval between stimuli in seconds
    private List<int> stimuliValues = new List<int>();  // Used for storing each stimuli value 
    private List<int> sumValues = new List<int>();      // Used for storing each sum value
    private int stimuliIndex = 0;
    private int currentSum = 0;
    private int currentStimuli = 0;
    private int userAnswer = -1;
    //private int correctAnswers = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateStimuli());
        GenerateButtons();
    }

    //Format for PASAT trials
    //11 practice stimuli
    //Lv1 Begin
    //Call up Likert Scale
    //Lv2 Begin
    //stimulusInterval = 2;
    //trialTIme = 5;
    //Call up Likert Scale
    //Lv3 Begin
    //stimulusInterval = 1.5;
    //trialTime = 10;
    //Call up Likert Scale

    // Function that generates the Stimuli 
    IEnumerator GenerateStimuli()
    {
        float startTime = Time.time;
        while (Time.time - startTime < trialTime * 60f) //while (true) [Original Code]
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
                yield return new WaitForSeconds(stimulusInterval);                                                 // Waits [stimulusInterval] seconds
                currentStimuli = UnityEngine.Random.Range(1, maxSumValue - stimuliValues[stimuliIndex - 1] + 1);   // Assigns currentStimuli to a random value: 1 - Max value possible for sum to be less than maxSumValue
                currentSum = currentStimuli + stimuliValues[stimuliIndex - 1];                                     // Calculates sum of currentStimuli and previous stimuli
                stimuliText.text = currentStimuli.ToString();                                                      // Sets stimuli text to the currentStimuli value
                stimuliValues.Add(currentStimuli);                                                                 // Adds stimuli to the stimuliValues list 
                stimuliIndex++;                                                                                    // Integrates stimuliIndex
            }
        }
    }

    // Function that generates the buttons based on the maxSumValue
    void GenerateButtons()
    {
        int buttonCount = 0, rowSize = 10;
        float buttonSize = 100f, buttonSpacing = 20f, rowX, rowY = 0f;
        GameObject currentRow = null;

        float totalWidth = rowSize * buttonSize + (rowSize - 1) * buttonSpacing;                            // Total width of a completely filled row
        int buttonsInFinalRow = maxSumValue % rowSize;                                                      // Number of buttons in final row
        float finalRowWidth = buttonsInFinalRow * buttonSize + (buttonsInFinalRow - 1) * buttonSpacing;     // Total width of final row

        // Deletes existing buttons if they exist
        foreach (Transform child in responses)
        {
            Destroy(child.gameObject);
        }

        // Creates formatted row of buttons from 1 - maxSumValue
        for (int i = 1; i <= maxSumValue; i++)
        {
            // If row is full, create a new row
            if (buttonCount % rowSize == 0)
            { 
                currentRow = new GameObject("Row", typeof(RectTransform));                          // Create row and assign to currentRow
                currentRow.transform.SetParent(responses);                                          // Set the parent of currentRow to Responses GameObject
                currentRow.GetComponent<RectTransform>().localScale = Vector3.one;                  // Set scale to 1
                currentRow.GetComponent<RectTransform>().localPosition = new Vector3(0, rowY, 0);   // Set position to center with offset y (rowY) depending on row
                buttonCount = 0;                                                                    // Reset button count to 0
                rowY -= buttonSize + buttonSpacing;                                                 // Decrement rowY by buttonSize + buttonSpacing: 120f
            }

            // Creates a button object child under responses object and sets text to i
            GameObject button = Instantiate(buttonPrefab.gameObject, currentRow.transform);
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            Button btn = button.GetComponentInChildren<Button>();
            buttonText.text = i.ToString();
            //btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { 
                userAnswer = int.Parse(buttonText.text); 
                //Debug.Log(userAnswer); 
            });

            // If button is in last row, adjust x values accordingly to properly center 
            if (i > maxSumValue - buttonsInFinalRow)
            {
                rowX = (buttonCount * (buttonSize + buttonSpacing)) - (finalRowWidth / 2f) + (buttonSize / 2f);   // Calculates last row button's x-position
            }
            else
            {
                rowX = (buttonCount * (buttonSize + buttonSpacing)) - (totalWidth / 2f) + (buttonSize / 2f);      // Calculates full row button's x-position
            }
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(rowX, 0);                         // Move button to calculated x-position

            buttonCount++;                                                                                        // Increment buttonCount
        }
    }
}