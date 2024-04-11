using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetPID : MonoBehaviour
{

    public string patientID;
    public TextMeshProUGUI pid;

    private void Awake(){
        patientID = PlayerPrefs.GetString("PatientID", "DefaultValue");
        pid.text = patientID;
    }
    
    public void setP_ID(){
        pid.text = patientID;
    }
}