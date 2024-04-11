using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ChangePInstruction : MonoBehaviour
{
    public void changePInst() {  
        SceneManager.LoadScene("InstructionScene");  
    }  
}   