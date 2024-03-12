//This is just a script to test if the NBACK test is working and is not intended to
//be in the final release

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NBackTester : MonoBehaviour
{

    public NBack nBack;
    public Text entryDisplay;

    // Start is called before the first frame update
    void Start()
    {
        nBack.Begin();
        Debug.Log(nBack.GetCurrentChar());
    }

    // Update is called once per frame
    void Update()
    {
        //entryDisplay.text = nBack.GetCurrentChar();
        
        
        if (Input.GetKeyDown("s")) {
            nBack.EnterChoice(true);
            Debug.Log(nBack.GetCurrentChar());
        }
        if (Input.GetKeyDown("d")) {
            nBack.EnterChoice(true);
            Debug.Log(nBack.GetCurrentChar());
        }
        
    }
}
