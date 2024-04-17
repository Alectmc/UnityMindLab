using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LikertScript : MonoBehaviour
{
    private int userAnswer;
    public static int[] answers;
    private static int currentRound = 1;                // Set to static in case of scene change for Instruction Scene
    public GameObject buttonPrefab;
    public TextMeshProUGUI questionText;
    public Transform responses;
    public PasatSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        answers = new int[settings.trialTime.Length];
        GenerateButtons();
    }

    void GenerateButtons() //To Do: can probably be simplified somewhat for the purposes of Likert Scale
    {
        int buttonCount = 0, rowSize = 10;
        float buttonSize = 150f, buttonSpacing = 20f, rowX, rowY = 0f;
        GameObject currentRow = null;

        float totalWidth = rowSize * buttonSize + (rowSize - 1) * buttonSpacing;                            // Total width of a completely filled row
        int buttonsInFinalRow = 5 % rowSize;                                                                // Number of buttons in final row
        float finalRowWidth = buttonsInFinalRow * buttonSize + (buttonsInFinalRow - 1) * buttonSpacing;     // Total width of final row

        // Deletes existing buttons if they exist
        foreach (Transform child in responses)
        {
            Destroy(child.gameObject);
        }

        // Creates formatted row of buttons from 1 - 5
        for (int i = 1; i <= 5; i++)
        {
            // If row is full, create a new row
            if (buttonCount % rowSize == 0)
            {
                currentRow = new GameObject("Row", typeof(RectTransform));                          // Create row and assign to currentRow
                currentRow.transform.SetParent(responses);                                          // Set the parent of currentRow to Responses GameObject
                currentRow.GetComponent<RectTransform>().localScale = Vector3.one;                  // Set scale to 1
                currentRow.GetComponent<RectTransform>().localPosition = new Vector3(0, rowY, 0);   // Set position to center with offset y (rowY) depending on row
                buttonCount = 0;                                                                    // Reset button count to 0
                rowY -= buttonSize + buttonSpacing;                                                 // Decrement rowY by buttonSize + buttonSpacing: 120f
            }

            // Creates a button object child under responses object and sets text to i
            GameObject button = Instantiate(buttonPrefab.gameObject, currentRow.transform);
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            Button btn = button.GetComponentInChildren<Button>();
            buttonText.text = i.ToString();
            btn.onClick.AddListener(() =>
            {
                userAnswer = int.Parse(buttonText.text);
                answers[currentRound - 1] = userAnswer;
                //Debug.Log(userAnswer); 
                //To Do: Print userAnswer to csv?
                if (currentRound < settings.trialTime.Length)
                {
                    currentRound++;
                    SceneManager.LoadScene("InstructionScene");
                }
                else
                {
                    foreach (Transform child in responses)
                    {
                        Destroy(child.gameObject);
                    }
                    questionText.text = "Test Complete";
                    StartCoroutine(ReturnToMenu());
                }
            });

            // If button is in last row, adjust x values accordingly to properly center 
            if (i > 5 - buttonsInFinalRow)
            {
                rowX = (buttonCount * (buttonSize + buttonSpacing)) - (finalRowWidth / 2f) + (buttonSize / 2f);   // Calculates last row button's x-position
            }
            else
            {
                rowX = (buttonCount * (buttonSize + buttonSpacing)) - (totalWidth / 2f) + (buttonSize / 2f);      // Calculates full row button's x-position
            }
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(rowX, 0);                         // Move button to calculated x-position

            buttonCount++;                                                                                        // Increment buttonCount
        }
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("SelectTest");
    }

}