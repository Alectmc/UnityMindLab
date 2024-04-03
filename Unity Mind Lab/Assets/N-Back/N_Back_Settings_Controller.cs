using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class N_Back_Settings_Controller : MonoBehaviour
{
    public TMP_InputField[] inputFields; // Reference to the InputField UI elements


    private string[] settingNames = { "movementInterval", "numRuns", "nthNumber", "totalStimuli", "nthProb", "intervalChange" };
    private string settingsFilePath = "Assets/N-Back/N_Back_Settings.txt"; // Path to the settings text file

 

    public void SaveSettingsToFile()
    {
        if (inputFields.Length != settingNames.Length)
        {
            Debug.LogError("Input fields array length does not match setting names length.");
            return;
        }

        // Create a StreamWriter to write to the text file
        using (StreamWriter writer = new StreamWriter(settingsFilePath))
        {
            for (int i = 0; i < inputFields.Length; i++)
            {
                // Write the setting name and its corresponding value to the file
                writer.WriteLine(settingNames[i]);
                writer.WriteLine(inputFields[i].text);
            }
        }

        Debug.Log("Settings saved to file: " + settingsFilePath);
    }
}
