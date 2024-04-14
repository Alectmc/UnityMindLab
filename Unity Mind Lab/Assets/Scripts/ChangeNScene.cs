using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeNScene : MonoBehaviour
{
    public void changeNBack() {  
        SceneManager.LoadScene("N-back_start-screen");  
    }  

    public void changeNBackSettings() {  
        SceneManager.LoadScene("N-BACK-SETTINGS");  
    }  
}   