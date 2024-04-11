using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class p_id_management : MonoBehaviour
{
    public TMP_InputField pid_input;
    
    public void readIn(){
        string pid = pid_input.text;
        Debug.Log(pid);
        savePatientID(pid);
    }
    
    public void savePatientID(string pid){
        PlayerPrefs.SetString("PatientID", pid);
        PlayerPrefs.Save();
    }
}