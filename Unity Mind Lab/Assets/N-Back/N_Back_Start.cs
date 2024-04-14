using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class N_Back_Start : MonoBehaviour
{
    // Start is called before the first frame update

    private string settingsPath;

    private int nthNumber=0;

    public TextMeshProUGUI NText;


    void Start()
    {
        settingsPath = Application.dataPath +"/N-Back/N_Back_Settings.txt";
        nthNumber = ReadSettingsFromFile(settingsPath);
        displayN();

        NText.text = (nthNumber)+" Stimuli Back";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void displayN()
    {

    }

    private int ReadSettingsFromFile(string path)
    {
        string[] lines = File.ReadAllLines(path);
        float movementInterval = float.Parse(lines[1]);
        int numRuns = int.Parse(lines[3])-1;
        int nthNumber = int.Parse(lines[5]);
        int totalStimuli = int.Parse(lines[7]);
        float nthProb = float.Parse(lines[9]);
        float intervalChange  = float.Parse(lines[11]);
        
        return nthNumber;

    }
}
