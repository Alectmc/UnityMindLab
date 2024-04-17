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
    // Public Variables
    public Transform responses;
    public GameObject buttonPrefab;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI stimuliText;
    public TextMeshProUGUI notification;
    public TextMeshProUGUI message;
    public PasatSettings settings;
    public Slider progressBar;
    public AudioClip[] numberAudioClips;
    public Logger logger;
    // Private Variables
    private static string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
    private Coroutine timerCoroutine;
    private int previousStimuli;
    private int currentStimuli;
    private int currentSum;
    private int userAnswer;
    private float stimuliTime;
    private float responseTime;
    private bool answered;
    // Counter Variables
    private static int currentRound = 0;
    private int correctAnswers = 0;
    private int stimuliCount = 0;
    // Test Settings
    private int maxSumValue;
    private float[] trialTime;
    private float[] stimulusInterval;
    private bool audioStimuli;

    void Start()
    {
        // Updating test settings
        maxSumValue = settings.maxSumValue;
        trialTime = settings.trialTime;
        stimulusInterval = settings.stimulusInterval;
        audioStimuli = settings.audioStimuli;
        // Initializing csv file
        logger.InitializeFile(timeStamp);
        // Starting Test
        if (settings.practiceMode)
        {
            StartCoroutine(PracticeRound());
            GenerateButtons();
            timerCoroutine = StartCoroutine(Timer());
        }
        else
        {
            StartCoroutine(GenerateStimuli());
            GenerateButtons();
            timerCoroutine = StartCoroutine(Timer());
        }
    }


    // Function that generates the Stimuli 
    IEnumerator GenerateStimuli()
    {
        float startTime = Time.time;
        float roundTime = trialTime[currentRound] * 60f;
        float currentTime;
        int point = 0;

        currentStimuli = UnityEngine.Random.Range(1, maxSumValue);  // Assigns currentStimuli to a random value: 1 - maxSumValue
        stimuliText.text = currentStimuli.ToString();               // Sets stimuli text to the currentStimuli value 

        while (Time.time - startTime < roundTime)
        {
            // Update timer bar
            float timeRemaining = roundTime - Time.time;
            progressBar.value = Mathf.Clamp01(timeRemaining / (trialTime[currentRound] * 60f));

            currentTime = (Time.time - startTime) * 1000f;                    // Current time in ms

            // Display new stimuli every [stimulusInterval] seconds
            if (audioStimuli)
                PlayNumberAudio(currentStimuli + 1);                          // playing stimuli audio 
            yield return new WaitForSeconds(stimulusInterval[currentRound]);  // Waits [stimulusInterval] seconds
            // If answer is correct, add point
            if (currentSum == userAnswer && answered)
                point += 1;
            // If no response given in time, set responseTime and UserAnswer to 0
            if (!answered)
            {
                userAnswer = 0;
                responseTime = 0;
                scoreText.text = correctAnswers.ToString() + " / " + stimuliCount.ToString();                  
            }
            // Record data
            logger.LogData(currentRound, stimuliCount, currentStimuli, currentSum, userAnswer, point, responseTime, currentTime);

            // Display new stimuli
            previousStimuli = currentStimuli;
            currentStimuli = UnityEngine.Random.Range(1, maxSumValue - previousStimuli + 1);   // Assigns currentStimuli to a random value: 1 - Max value possible for sum to be less than maxSumValue
            stimuliTime = Time.time;
            currentSum = currentStimuli + previousStimuli;                                     // Calculates sum of currentStimuli and previous stimuli
            stimuliText.text = currentStimuli.ToString();                                      // Sets stimuli text to the currentStimuli value
            stimuliCount++;
            answered = false;
        }

        currentRound++;
        stimuliText.text = "";
        message.text = "Round " + (currentRound) + " complete"; 
        StopCoroutine(timerCoroutine);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("LikertScale");
    }

    // Function that runs the practice round
    IEnumerator PracticeRound()
    {
        currentStimuli = UnityEngine.Random.Range(1, maxSumValue);  // Assigns currentStimuli to a random value: 1 - maxSumValue
        stimuliText.text = currentStimuli.ToString();               // Sets stimuli text to the currentStimuli value 

        while (stimuliCount < 11)
        {
            float timeRemaining = 180 - Time.time;
            progressBar.value = Mathf.Clamp01(timeRemaining / 180f);

            if (audioStimuli)
                PlayNumberAudio(currentStimuli + 1);
            yield return new WaitForSeconds(3f);

            previousStimuli = currentStimuli;
            currentStimuli = UnityEngine.Random.Range(1, maxSumValue - previousStimuli + 1);

            currentSum = currentStimuli + previousStimuli;                                     // Calculates sum of currentStimuli and previous stimuli
            stimuliText.text = currentStimuli.ToString();                                      // Sets stimuli text to the currentStimuli value
            stimuliCount++;
            answered = false;
        }

        settings.practiceMode = false;
        stimuliText.text = "";
        message.text = "Practice Round Complete";
        StopCoroutine(timerCoroutine);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("InstructionScene");           // Returns to instruction scene, do not run likert scale.
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
            btn.onClick.AddListener(() =>
            {
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

    // Checks the response to the currentSum IF not initial stimuli value and user has not answered to the sum
    void checkResponse()
    {
        if (stimuliCount > 0 && !answered) // Allows only one response per sum, prevents the user from correcting their response after getting answer incorrect
        {
            string message = "";
            if (userAnswer == currentSum)
            {
                correctAnswers++;
                scoreText.text = correctAnswers.ToString() + " / " + stimuliCount.ToString();     // Display current score
                message = "Correct!";                                                             // Display "correct" message to the stimuli text box
                answered = true;                                                                  // Set current sum as answered by the user
            }
            else
            {
                scoreText.text = correctAnswers.ToString() + " / " + stimuliCount.ToString();     // Display current score
                message = "Incorrect!";                                                           // Display "correct" message to the stimuli text box
                answered = true;                                                                  // Set current sum as answered by the user
            }
            StartCoroutine(ShowNotification(message));
        }
    }

    // Calculates patient response time
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

    // Plays stimuli audio
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