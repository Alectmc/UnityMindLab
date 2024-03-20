using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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
    public int totalMovements = 25; // Total number of movements
    private bool isMovingToFront = false; // Flag to track if a movement to the front is in progress
    private bool isMovingToBack = false; // Flag to track if a movement to the back is in progress
    private float movementInterval = 2000f; // Time interval between movements in milliseconds
    private float nextMovementTime = 0f; // Time of the next movement
    private int lastMovedLetterIndex = -1; // Index of the last letter moved to the front

    public int nthNumber = 2;

    private string nthLetter;//for holding string of nth letter back

    private List<string> stimuli = new List<string>(); // List to store the selected letters
    private List<string> answers = new List<string>(); // List to store user answers

    private bool answered;//flag for whether the user answered or not

    private float nthProb=0.2f;

    private Color goodColor = new Vector4(0f,1f,0f,1f);

    private Color badColor = new Vector4(1f,0f,0f,1f);

    private Color regularColor= new Vector4(1f,1f,1f,0f);

    private string selectedLetter ="";

    



    void Start()
    {
        
        //start letters
    }

    void Update()
    {
        // Initialize the next movement time when the timer starts
        if (timer.isRunning && nextMovementTime == 0f)
        {
            nextMovementTime = timer.GetElapsedTimeMilliseconds();
        }

        // Check if the timer is running and the total number of movements hasn't been reached
        if (timer.isRunning && stimulusCount < totalMovements)
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




        if(Input.GetMouseButtonDown(0)&& !isMovingToFront)//listener, with a guard to not go when the button is not shown.
        {
            GetAnswer();
        }
    }

    void ShowButton()
    {
        // Randomly select one of the letter buttons that is not the last moved letter
        int randomIndex= -1;
        
        
        // Check if it's time to select the nth letter
        if (stimulusCount > nthNumber && Random.value <= nthProb)
        {
            // Select the nth letter
            selectedLetter = nthLetter;
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

        nthLetter = stimuli[stimulusCount];

        // Update flags and move count
        isMovingToBack = false;
        moveCount++;
        stimulusCount++;
    }

    public void GetAnswer(){//to get answers from user based on mouseclick

        if(stimulusCount<nthNumber){
            
        }
        else{
            CanvasRenderer buttonRenderer = letterButton.GetComponent<CanvasRenderer>();
            if(selectedLetter ==nthLetter){
                answers[stimulusCount] ="Y";//adds a y to answers list
                buttonRenderer.SetColor(goodColor);//sets button to green

            }
            else{
                answers[stimulusCount]="N";//ands an n to answers list
                buttonRenderer.SetColor(badColor);//sets button to red
            }
            Debug.Log(answers.ToString());
        }
    }
}
