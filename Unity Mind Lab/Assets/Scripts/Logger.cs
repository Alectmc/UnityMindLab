using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private string folderPath;
    private string filePath;
    private StreamWriter writer;
    private string participantId;
    public PasatSettings settings;

    private void Awake()
    {
        participantId = PlayerPrefs.GetString("PatientID", "DefaultValue");
    }

    private void Start()
    {
        folderPath = Path.Combine(Application.dataPath, "ParticipantLogs");
        if (!Directory.Exists(folderPath))
        {
            try
            {
                Directory.CreateDirectory(folderPath);
            }
            catch(System.Exception e)
            {
                Debug.Log("Error creating directory: " + e.Message);
                return;
            }
        }
    }

    public void InitializeFile(string timeStamp)
    {
        filePath = Path.Combine(folderPath, participantId + "_" + timeStamp + "_pasat.csv");
        try
        {
            if (!File.Exists(filePath))
            {
                writer = new StreamWriter(filePath, true);
                string header = "Round,Trial,Stimuli,Correct Sum,Selected Sum,Points,Response Time,Time Log";
                writer.WriteLine(header);
                writer.Flush();
            }
            else 
            {
                writer = new StreamWriter(filePath, true);
            }
        }
        catch(System.Exception e) 
        {
            Debug.LogError("Error initializing file: " + e.Message);
        }
    }

    public void LogData(int round, int trial, int stimuli, int correctSum, int selectedSum, int point, float responseTime, float timeLog)
    {
        if (writer == null)
        {
            Debug.LogError("Writer is null. File not initialized.");
            return;
        }

        try
        {
            string line = $"{round},{trial},{stimuli},{correctSum},{selectedSum},{point},{responseTime},{timeLog}";
            writer.WriteLine(line);
            writer.Flush();
        }
        catch (System.Exception e) 
        {
            Debug.LogError("Error writing data to file: " + e.Message);
        }
    }

    public void CloseFile()
    {
        writer.Close();
    }
}
