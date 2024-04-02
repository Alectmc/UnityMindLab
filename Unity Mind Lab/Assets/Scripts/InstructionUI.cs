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


    public TextMeshProUGUI welcomeText;
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
        //checkStatus();

    }

    void displayInstruct()
    {
        if (settings.practiceMode == true) //Display practice mode instruction
        {
            welcomeText.text = "Welcome to the PASAT test!";
            instructText.text = "Practice round is active, the system will display eleven practice stimuli before official test begins.";
            timeIntervalInfo.text = "Stimuli value interval time: 3 seconds";
        }
        else if (settings.practiceMode == false && currentRound == 0) //Display round 1 instruction
        {
            instructText.text = "Practice round complete.";
            timeIntervalInfo.text = "Round " + (currentRound + 1) + " trial time: " + (trialTime[currentRound]) + " minutes / Stimuli value interval time: " + (stimulusInterval[currentRound]) + " seconds";
            currentRound++;
        }
        else if (currentRound < trialTime.Length) //Display rest of the rounds' instructions
        {
            instructText.text = "Round " + (currentRound) + " complete.";
            timeIntervalInfo.text = "Round " + (currentRound + 1) + " trial time: " + (trialTime[currentRound]) + " minutes / Stimuli value interval time: " + (stimulusInterval[currentRound]) + " seconds";
            currentRound++;
        }
        else //Confirm test is complete, have not error checked.
        {
            instructText.text = "PASAT test complete.";
        }
    }
    
    public void return_to_test() //Placeholder, can be replaced by generated button
    {
        SceneManager.LoadScene("PASAT");
    }
}
