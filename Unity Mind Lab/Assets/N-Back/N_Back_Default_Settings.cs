using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class N_Back_Default_Settings : MonoBehaviour//sets default settings in text file
{


    private string[] settingNames = { "movementInterval", "numRuns", "nthNumber", "totalStimuli", "nthProb", "intervalChange" };

    private string[] defaultSettings = {"2000","3","2","25","0.15","500"};
    private string settingsFilePath = "Assets/N-Back/N_Back_Settings.txt"; // Path to the settings text file

 

    public void SaveSettingsToFile()
    {
        // Create a StreamWriter to write to the text file
        using (StreamWriter writer = new StreamWriter(settingsFilePath))
        {
            for (int i = 0; i < defaultSettings.Length; i++)
            {
                // Write the setting name and its corresponding value to the file
                writer.WriteLine(settingNames[i]);
                writer.WriteLine(defaultSettings[i]);
            }
        }

        Debug.Log("Settings saved to file: " + settingsFilePath);
    }
}
