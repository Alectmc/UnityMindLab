using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TakeATestButtonScript : MonoBehaviour
{
   public void ManageScene(string level)
    {
        SceneManager.LoadScene(level);
    }
}
