using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public PasatSettings settings;
    private string patientFolderPath;
    private string settingsFolderPath;
    private string patientFilePath;
    private string settingsFilePath;
    private StreamWriter patientWriter;
    private StreamWriter settingsWriter;

    private void Start()
    {
        patientFolderPath = Path.Combine(Application.dataPath, "PatientLogs");
        settingsFolderPath = Path.Combine(Application.dataPath, "SettingsLogs");
        if (!Directory.Exists(patientFolderPath))
        {
            try
            {
                Directory.CreateDirectory(patientFolderPath);
            }
            catch(System.Exception e)
            {
                Debug.Log("Error creating directory: " + e.Message);
                return;
            }
        }
        if (!Directory.Exists(settingsFolderPath))
        {
            try
            {
                Directory.CreateDirectory(settingsFolderPath);
            }
            catch (System.Exception e)
            {
                Debug.Log("Error creating directory: " + e.Message);
                return;
            }
        }
    }

    public void InitializeFile(string timeStamp)
    {
        patientFilePath = Path.Combine(patientFolderPath, timeStamp + "_pasat.csv");
        settingsFilePath = Path.Combine(settingsFolderPath, timeStamp + "_pasat.csv");
        try
        {
            if (!File.Exists(patientFilePath))
            {
                patientWriter = new StreamWriter(patientFilePath, true);
                string header = "Round,Trial,Stimuli,Correct Sum,Selected Sum,Points,Response Time,Time Log";
                patientWriter.WriteLine(header);
                patientWriter.Flush();
            }
            else 
            {
                patientWriter = new StreamWriter(patientFilePath, true);
            }
        }
        catch(System.Exception e) 
        {
            Debug.LogError("Error initializing file: " + e.Message);
        }
        try
        {
            if (!File.Exists(settingsFilePath))
            {
                settingsWriter = new StreamWriter(settingsFilePath, true);
                string pasatSettings = "Max Sum Value = " + settings.maxSumValue + 
                                       "\nTrial Times = " + string.Join(", ", settings.trialTime) + 
                                       "\nStimulus Intervals = " + string.Join(", ", settings.stimulusInterval) + 
                                       "\nPractice Mode = " + settings.practiceMode + 
                                       "\nAudio Stimuli = " + settings.audioStimuli;
                settingsWriter.WriteLine(pasatSettings);
                settingsWriter.Flush();
                settingsWriter.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error initializing file: " + e.Message);
        }
    }

    public void LogData(int round, int trial, int stimuli, int correctSum, int selectedSum, int point, float responseTime, float timeLog)
    {
        if (patientWriter == null)
        {
            Debug.LogError("Writer is null. File not initialized.");
            return;
        }
        try
        {
            string line = $"{round},{trial},{stimuli},{correctSum},{selectedSum},{point},{responseTime},{timeLog}";
            patientWriter.WriteLine(line);
            patientWriter.Flush();
        }
        catch (System.Exception e) 
        {
            Debug.LogError("Error writing data to file: " + e.Message);
        }
    }

    public void CloseFile()
    {
        patientWriter.Close();
    }
}
