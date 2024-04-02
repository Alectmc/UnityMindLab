using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PasatSettings", menuName = "Settings/PasatSettings")]
public class PasatSettings : ScriptableObject
{
    public int maxSumValue;
    public float[] trialTime;
    public float[] stimulusInterval;
    public bool practiceMode;
    public bool audioStimuli;
}
