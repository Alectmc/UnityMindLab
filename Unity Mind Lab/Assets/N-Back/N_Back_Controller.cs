using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class N_Back_Controller : MonoBehaviour
{
    public string[] letters ={"A","B","C","D","E","F","G","H","I",
                              "J","K","L","M","N","O","P","Q","R",
                              "S","T","U","V","W","X","Y","Z"}; // Array of letters A through Z
    public Timer timer; // Reference to the Timer script

    public Button letterButton;// Reference to the button

    public TextMeshProUGUI buttonText;//Reference to the text of the button

    private int moveCount = 0; // Counter to track the number of movements
    private int stimulusCount=0;//Counts total stimuli
    private int outputCount=0;//holds index for output to csv
    public int totalStimuli = 25; // Total number of movements

    public int numRuns = 1;

    private int runCount=0;
    private bool isMovingToFront = false; // Flag to track if a movement to the front is in progress
    private bool isMovingToBack = false; // Flag to track if a movement to the back is in progress
    private float movementInterval = 2000f; // Time interval between movements in milliseconds
    private float intervalChange=0f;
    private float nextMovementTime = 0f; // Time of the next movement
    private int lastMovedLetterIndex = -1; // Index of the last letter moved to the front

    public int nthNumber = 2;

    private string nthLetter;//for holding string of nth letter back

    //private bool buttonsBool =false;//if true, use mouseclick on buttons for input. if false, use keys "y" and "n"

    private List<string> stimuli = new List<string>(); // List to store the selected letters
    private List<string> answers = new List<string>(); // List to store user answers

    private List<float> timeStampAnswer = new List<float>();

    private List<float> timeStampStimuli = new List<float>();

    private List<int> runList = new List<int>();

    private bool answered=false;//flag for whether the user answered or not

    private float nthProb=0.15f;

    private Color goodColor = new Vector4(0f,1f,0f,1f);

    private Color badColor = new Vector4(1f,0f,0f,1f);

    private Color regularColor= new Vector4(1f,1f,1f,0f);

    private string selectedLetter ="";

    private string settingsPath;

    private string outFile;
    



    void Start()
    {

        // Path to the text file in the Assets folder
        settingsPath = Application.dataPath +"/N-Back/N_Back_Settings.txt";
        outFile = Application.dataPath + "/N-Back/test.csv";
        

        Debug.Log("File path: " + settingsPath); // Debug log the file path
        
        ReadSettingsFromFile(settingsPath);

        timer.StartTimer();
    }

    void Update()
    {
        test();

    }

    void ShowButton()
    {
        // Randomly select one of the letter buttons that is not the last moved letter
        int randomIndex= -1;
        
        
        // Check if it's time to select the nth letter
        if (stimulusCount > nthNumber && Random.value <= nthProb)
        {
            // Select the nth letter
            selectedLetter = stimuli[outputCount-nthNumber];
            nthProb=0.2f;
            
        }
        else
        {
            // Choose a random letter
            randomIndex = Random.Range(0, letters.Length);
            selectedLetter = letters[randomIndex];
            nthProb+=0.2f;
        }

        

        buttonText.text = selectedLetter;

        stimuli.Add(selectedLetter);

        answers.Add("None");

        runList.Add(runCount);

        timeStampStimuli.Add(timer.GetElapsedTimeMilliseconds());
        timeStampAnswer.Add(0f);

        

        

        // Set alpha to 1 (fully visible)
        CanvasRenderer buttonRenderer = letterButton.GetComponent<CanvasRenderer>();
        buttonRenderer.SetAlpha(1f);

        

        // Update flags, move count, and last moved letter index
        isMovingToFront = false;
        lastMovedLetterIndex = randomIndex;
        moveCount++;
        
    }

    void HideButton()
    {
        
        CanvasRenderer buttonRenderer = letterButton.GetComponent<CanvasRenderer>();
        buttonRenderer.SetColor(regularColor);
        

        buttonText.text = "";

        if(stimulusCount<nthNumber){

        }
        else{
            nthLetter = stimuli[stimulusCount-nthNumber];
        }
        // Update flags and move count
        isMovingToBack = false;
        moveCount++;
        stimulusCount++;
        outputCount++;

        answered=false;
    }

    public void GetAnswer(string str){//to get answers from user based on mouseclick

        if(stimulusCount<nthNumber||answered){
            
        }
        else{
            
            answers[outputCount]= str;//adds choice to list

            CanvasRenderer buttonRenderer = letterButton.GetComponent<CanvasRenderer>();
            if((selectedLetter ==stimuli[outputCount-nthNumber]&& str=="Y")||selectedLetter !=stimuli[outputCount-nthNumber]&& str=="N"){//if choice was correct
                //answers[outputCount] ="Y";//adds a y to answers list
                buttonRenderer.SetColor(goodColor);//sets button to green
                

            }
            else{
                //answers[outputCount]="N";//ands an n to answers list
                buttonRenderer.SetColor(badColor);//sets button to red
            }
            answered = true;
            timeStampAnswer[outputCount]=timer.GetElapsedTimeMilliseconds();
        }
    }

    // Function to read settings from the file
    private void ReadSettingsFromFile(string path)
    {
        string[] lines = File.ReadAllLines(path);
        movementInterval = float.Parse(lines[1]);
        numRuns = int.Parse(lines[3])-1;
        nthNumber = int.Parse(lines[5]);
        totalStimuli = int.Parse(lines[7]);
        nthProb = float.Parse(lines[9]);
        intervalChange  = float.Parse(lines[11]);
        


    }

    private void WriteToFile(string outFile){
        string header = "run,stimuli,answer,StimulusTime,AnswerTime"; 

        // Open the file for writing
        using (StreamWriter writer = new StreamWriter(outFile))
        {
            // Write the header to the file
            writer.WriteLine(header);
            
            // Write each line of data
            for (int i = 0; i < stimuli.Count; i++)
            {
                Debug.Log("writing");
                string line = $"{runList[i]},{stimuli[i]},{answers[i]},{timeStampStimuli[i]},{timeStampAnswer[i]}";
                writer.WriteLine(line);
            }
        }
    }


    private void test(){
                // Initialize the next movement time when the timer starts
        if (timer.isRunning && nextMovementTime == 0f)
        {
            nextMovementTime = timer.GetElapsedTimeMilliseconds();
        }

        // Check if the timer is running and the total number of movements hasn't been reached
        if (timer.isRunning && stimulusCount < totalStimuli)
        {
            // If it's time to move the object to the front
            if (isMovingToFront && timer.GetElapsedTimeMilliseconds() >= nextMovementTime)
            {
                ShowButton();
            }

            // If it's time to move the object to the back
            if (isMovingToBack && timer.GetElapsedTimeMilliseconds() >= nextMovementTime)
            {
                HideButton();
            }

            // If neither movement is in progress, determine the next movement
            if (!isMovingToFront && !isMovingToBack)
            {
                if (moveCount % 2 == 0)
                {
                    isMovingToFront = true;
                }
                else
                {
                    isMovingToBack = true;
                }

                nextMovementTime = timer.GetElapsedTimeMilliseconds() + movementInterval;
            }
        }

        if(timer.isRunning&& stimulusCount>=totalStimuli&&runCount<numRuns){
            runCount++;
            stimulusCount=0;
            moveCount=0;
            isMovingToBack = false;
            nextMovementTime = timer.GetElapsedTimeMilliseconds() +3000f;
            movementInterval= movementInterval - intervalChange;
            buttonText.text = "Run " + (runCount+1).ToString();
            





        }

        if(runCount==numRuns && stimulusCount>=totalStimuli && timer.isRunning){//ending test and writing to csv
            timer.StopTimer();
            Debug.Log("end test");
            WriteToFile(outFile);


        }

        if(Input.GetKeyDown(KeyCode.Y)&& !isMovingToFront)//listener, with a guard to not go when the button is not shown.
        {
            GetAnswer("Y");
        }

        if(Input.GetKeyDown(KeyCode.N)&& !isMovingToFront){
            GetAnswer("N");
        }

    }







}


