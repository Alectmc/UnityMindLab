using UnityEngine;
using UnityEditor;

public class Timer : MonoBehaviour
{
    public float startTime;
    public bool isRunning = false;

    public bool start;

    public float ElapsedTimeMilliseconds;


    private void Update()
    {
        if (isRunning)
        {
            UpdateElapsedTime();
        }
    }

    private void UpdateElapsedTime()
    {
        ElapsedTimeMilliseconds = (Time.time - startTime) * 1000f;
    }
    
    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void StopTimer()
    {

        isRunning = false;
    }

    public void ResetTimer()
    {
        startTime = 0f;
        ElapsedTimeMilliseconds = 0f;
        isRunning = false;
    }

    // Method to get the elapsed time
    public float GetElapsedTimeMilliseconds()
    {
        return ElapsedTimeMilliseconds;
    }
}