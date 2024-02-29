using UnityEngine;

public class TestObjectController : MonoBehaviour
{
    public Transform[] letterObjects; // Array of transforms for letters A through J
    public Timer timer; // Reference to the Timer script

    private int moveCount = 0; // Counter to track the number of movements
    private int totalMovements = 25; // Total number of movements
    private bool isMovingToFront = false; // Flag to track if a movement to the front is in progress
    private bool isMovingToBack = false; // Flag to track if a movement to the back is in progress
    private float movementInterval = 2000f; // Time interval between movements in milliseconds
    private float nextMovementTime = 0f; // Time of the next movement
    private int lastMovedLetterIndex = -1; // Index of the last letter moved to the front

    void Update()
    {
        // Initialize the next movement time when the timer starts
        if (timer.isRunning && nextMovementTime == 0f)
        {
            nextMovementTime = timer.GetElapsedTimeMilliseconds();
        }

        // Check if the timer is running and the total number of movements hasn't been reached
        if (timer.isRunning && moveCount < totalMovements)
        {
            // If it's time to move the object to the front
            if (isMovingToFront && timer.GetElapsedTimeMilliseconds() >= nextMovementTime)
            {
                MoveObjectToFront();
            }

            // If it's time to move the object to the back
            if (isMovingToBack && timer.GetElapsedTimeMilliseconds() >= nextMovementTime)
            {
                MoveObjectToBack();
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
    }

    void MoveObjectToFront()
    {
        // Randomly select one of the letter objects that is not the last moved letter
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, letterObjects.Length);
        } while (randomIndex == lastMovedLetterIndex);

        Transform selectedLetter = letterObjects[randomIndex];

        // Move the selected letter to the front
        selectedLetter.position = new Vector3(0f, 0f, 0f);

        // Update flags, move count, and last moved letter index
        isMovingToFront = false;
        lastMovedLetterIndex = randomIndex;
        moveCount++;
    }

    void MoveObjectToBack()
    {
        // Move the last moved letter to the back
        if (lastMovedLetterIndex != -1)
        {
            Transform selectedLetter = letterObjects[lastMovedLetterIndex];
            selectedLetter.position = new Vector3(0f, 0f, 1f);
        }

        // Update flags and move count
        isMovingToBack = false;
        moveCount++;
    }
}
