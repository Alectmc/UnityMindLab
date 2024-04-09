using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class InstructionUI : MonoBehaviour
{
    //Incomplete
    //Set likert scale to here or somewhere in PASAT script actual

    public TMP_Text buttonText;
    public TextMeshProUGUI instructText;
    public TextMeshProUGUI timeIntervalInfo;
    public PasatSettings settings;

    //Taken directly from PASAT.cs, saving number of correct answers when scene switch yet to be implemented.
    private float[] trialTime;                           // Used for time available for test trials in minutes
    private float[] stimulusInterval;                    // Used for the interval between stimuli in seconds
    private static int currentRound = 0;

    // Start is called before the first frame update
    void Start()
    {
        trialTime = settings.trialTime;
        stimulusInterval = settings.stimulusInterval;
        displayInstruct();
    }

    void displayInstruct()
    {
        if (settings.practiceMode == true) //Display practice mode instruction
        {
            instructText.text = "Practice round is active, the system will display eleven practice stimuli before official test begins.";
            timeIntervalInfo.text = "Response Time:\n3 seconds";
            buttonText.text = "Begin";
        }
        else if (settings.practiceMode == false && currentRound == 0) //Display round 1 instruction
        {
            instructText.text = "";
            timeIntervalInfo.text = "Round " + (currentRound + 1) + "\nTrial Time: " + (trialTime[currentRound]) + " minutes\nResponse Time: " + (stimulusInterval[currentRound]) + " seconds";
            buttonText.text = "Begin";
            currentRound++;
        }
        else if (currentRound < trialTime.Length) //Display rest of the rounds' instructions
        {
            instructText.text = "Round " + (currentRound) + " Complete.";
            timeIntervalInfo.text = "Round " + (currentRound + 1) + "\nTrial Time: " + (trialTime[currentRound]) + " minutes\nResponse Time: " + (stimulusInterval[currentRound]) + " seconds";
            buttonText.text = "Begin";
            currentRound++;
        }
        else //Confirm test is complete, have not error checked.
        {
            instructText.text = "PASAT test complete.";
            buttonText.text = "Return";
        }
    }
    
    public void return_to_test() //Placeholder, can be replaced by generated button
    {
        SceneManager.LoadScene("PASAT");
    }

}
