using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Timer))]
public class TimerUI : Editor

{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Timer timer = (Timer)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Start Timer"))
        {
            timer.StartTimer();
        }


        if(GUILayout.Button("Reset Timer")){
            timer.ResetTimer();
        }

        if(GUILayout.Button("Stop Timer")){
            timer.StopTimer();
        }
    }
}