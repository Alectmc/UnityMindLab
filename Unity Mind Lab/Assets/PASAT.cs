using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;
using System;

public class PASAT : MonoBehaviour
{
    public Transform responses;
    public GameObject buttonPrefab;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI stimuliText;
    public TextMeshProUGUI notification;
    public TextMeshProUGUI message;
    public int maxSumValue;
    public float[] trialTime;                           // Used for time available for test trials in minutes
    public float[] stimulusInterval;                    // Used for the interval between stimuli in seconds
    private List<int> stimuliValues = new List<int>();  // Used for storing each stimuli value
    private int stimuliIndex = 0;
    private int currentSum;
    private int currentStimuli = 0;
    private int stimuliCount = 0;
    private float stimuliTime;
    private int userAnswer = 0;
    private int correctAnswers = 0;
    private float responseTime;
    private static int currentRound = 0;                // Set to static in case of scene change for likert scale
    private bool hasAnswered;
    private Coroutine timerCoroutine;
    public Slider progressBar;
    private static string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

    public AudioClip[] numberAudioClips; // audio for stimuli 
    public Logger logger;
    // Defining practice mode status, should be toggled on/off via settings text file
    public static bool practiceMode = true;

    // Start is called before the first frame update
    void Start()
    {
        logger.InitializeFile(timeStamp);
        StartCoroutine(GenerateStimuli());
        GenerateButtons();
        timerCoroutine = StartCoroutine(Timer());
    }

    // Function that generates the Stimuli 
    IEnumerator GenerateStimuli()
    {
        float startTime = Time.time;
        float roundTime = trialTime[currentRound] * 60f;
        float currentTime;
        int point = 0;


        while (Time.time - startTime < roundTime)
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
                float timeRemaining = roundTime - Time.time;
                progressBar.value = Mathf.Clamp01(timeRemaining / (trialTime[currentRound] * 60f));

                currentTime = (Time.time - startTime) * 1000f;
                // Display new stimuli every [stimulusInterval] seconds
                PlayNumberAudio(currentStimuli + 1);                              // playing stimuli audio 
                yield return new WaitForSeconds(stimulusInterval[currentRound]);  // Waits [stimulusInterval] seconds

                if (currentSum == userAnswer && hasAnswered == true)
                    point += 1;
                if (hasAnswered == false)
                {
                    userAnswer = 0;
                    responseTime = 0;
                    scoreText.text = correctAnswers.ToString() + " / " + stimuliCount.ToString();                 // updating scoreboard 
                }
                if (practiceMode == false)
                    logger.LogData(currentRound, stimuliCount, currentStimuli, currentSum, userAnswer, point, responseTime, currentTime);

                currentStimuli = UnityEngine.Random.Range(1, maxSumValue - stimuliValues[stimuliIndex - 1] + 1);   // Assigns currentStimuli to a random value: 1 - Max value possible for sum to be less than maxSumValue
                stimuliTime = Time.time;
                currentSum = currentStimuli + stimuliValues[stimuliIndex - 1];                                     // Calculates sum of currentStimuli and previous stimuli
                stimuliText.text = currentStimuli.ToString();                                                      // Sets stimuli text to the currentStimuli value
                stimuliValues.Add(currentStimuli);                                                                 // Adds stimuli to the stimuliValues list 
                hasAnswered = false;

                stimuliCount++;                                                                               // increasing stimuli count 
                stimuliIndex++;                                                                               // Integrates stimuliIndex

                // If practiceMode is true, check if sumValue is above 11, terminate current trial if true and do not save result. (check if extra sum was generated prior limiting stimuli sum bound)
                if ((practiceMode == true) && (stimuliCount > 11))                                            
                {
                    scoreText.text = correctAnswers.ToString() + " / " + stimuliCount.ToString();
                    practiceMode = false;
                    stimuliText.text = "";
                    message.text = "Practice Round Complete";

                    yield return new WaitForSeconds(5f);
                    SceneManager.LoadScene("InstructionScene");           // Returns to instruction scene, do not run likert scale.
                }
            }
        }

        currentRound++;
        stimuliText.text = "";
        message.text = "Round " + (currentRound) + " complete";
        if (currentRound > trialTime.Length)
        {
            logger.CloseFile();
        }
        StopCoroutine(timerCoroutine);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("LikertScale");
    }

    // Function that generates the buttons based on the maxSumValue
    void GenerateButtons()
    {
        int buttonCount = 0, rowSize = 10;
        float buttonSize = 150f, buttonSpacing = 20f, rowX, rowY = 0f;
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
                checkResponse();
                ResponseTime(); 
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

    void checkResponse()  //Checks the response to the currentSum IF not initial stimuli value and user has not answered to the sum
    {
        if (stimuliValues.Count > 1 && hasAnswered == false) //Allows only one response per sum, prevents the user from correcting their response after getting answer incorrect
        {
            string message = "";
            if (userAnswer == currentSum)
            {
                correctAnswers++;
                scoreText.text = correctAnswers.ToString() + " / " + stimuliCount.ToString();// Display current score
                message = "Correct!";                                                             // Display "correct" message to the stimuli text box
                hasAnswered = true;                                                               // Set current sum as answered by the user
            }
            else
            {
                scoreText.text = correctAnswers.ToString() + " / " + stimuliCount.ToString(); // Display current score
                message = "Incorrect!";                                                           // Display "correct" message to the stimuli text box
                hasAnswered = true;                                                               // Set current sum as answered by the user
            }
            StartCoroutine(ShowNotification(message));
        }
    }

    void ResponseTime()
    {
        float currentTime = Time.time;
        responseTime = (currentTime - stimuliTime) * 1000f;
    }

    // Displays whether user answer is correct or incorrect for 1 second
    IEnumerator ShowNotification(string message)
    {
        notification.text = message;
        yield return new WaitForSeconds(1f);
        notification.text = "";
    }
    
    // Function that controls timer slider
    IEnumerator Timer()
    {
        while (true)
        {
            float totalTime = stimulusInterval[currentRound];
            float timeRemaining = totalTime;
            while (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                progressBar.value = timeRemaining / totalTime;
                yield return null;
            }
            progressBar.value = 0f;
        }
    }

    void PlayNumberAudio(int number)
    {
        if (number >= 1 && number <= numberAudioClips.Length) 
        {
            // creating a temp GameObject to play the audio clip 
            GameObject audioObject = new GameObject("TempAudio");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>(); 
            audioSource.clip = numberAudioClips[number - 1]; // set the audio clip

            // play the stimulis audio clip
            audioSource.Play();

            // destroying the temp GameObject after the audio has finished 
            Destroy(audioObject, numberAudioClips[number - 1].length);
        }
      
    }
}
