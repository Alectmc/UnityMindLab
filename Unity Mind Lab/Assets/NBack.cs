using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBack : MonoBehaviour
{
    
    //Only contains a single variable, but can be modified later for flexibility
    //Stores correct and response time for later exporting list to .csv
    public class NBackEntry {
        
        public char letter;
        public float responseTime;
        public bool correct;
        
        public NBackEntry(char l) {
            letter = l;
        }
        
    };
    
    public int n; //How far back should the element match
    public float maxResponseTime; //How long user has to respond
    public float responseTime; //How long the user is taking to respond
    public float chanceSame; //Percent chance a new entry is the same as the Nth previous entry
    public List<NBackEntry> entries; //List of entries, 1 is appended each turn
    
    public int round; //Starts from 1
    
    private bool started;
    
    public int correct;
    public int incorrect;
    
    //List of possible characters for entries
    private string candidates = "abcdefghijklmnopqrstuvwxyz";
    
    
    //Returns true if the N-previous entry is the same as the current entry
    private bool EntryMatches() {
        if(round-1 >= n) {
            return entries[round - n - 1].letter == entries[round - 1].letter;
        }
        return false;
    }
    
    //Called preferably by other scripts
    public void EnterChoice(bool same) {
        
        if(EntryMatches() == same) {
            entries[round-1].correct = true;
            Debug.Log("Correct!");
            correct++;
        }
        else {
            incorrect++;
            Debug.Log("Incorrect!");
        }
        
        entries[round-1].responseTime = responseTime;
        responseTime = 0;
        MakeEntry();
    }
    
    //Skip round for user ran out of time
    public void SkipChoice() {
        incorrect++;
        entries[round-1].correct = false;
        entries[round-1].responseTime = maxResponseTime;
        responseTime = 0;
        MakeEntry();
        Debug.Log("Incorrect! (Ran out of time)");
    }
    
    //Generate a new entry with a random letter
    private void MakeEntry() {
        
        //Generate chance if to use the N-th previous letter
        int c = Random.Range(1,101);
        if(round-1 >= n && c <= chanceSame) {
            //Generate same letter as N-th previous letter
            NBackEntry sameNewEntry = new NBackEntry(entries[round - n].letter);
            entries.Add(sameNewEntry);
            round++;
            return;
        }
        
        int i = Random.Range(0,candidates.Length);
        NBackEntry newEntry = new NBackEntry(candidates[i]);
        entries.Add(newEntry);
        round++;
        
    }
    
    public char GetCurrentChar() {
        return entries[round-1].letter;
    }
    
    //Called function to begin the test
    public void Begin() {
        responseTime = 0;
        round = 0;
        entries = new List<NBackEntry>();
        MakeEntry();
        started = true;
        correct = incorrect = 0;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(started) {
            
            if(responseTime > maxResponseTime) {
                //User ran out of time
                SkipChoice();
            }
            else {
                responseTime += Time.deltaTime;
            }
        }
    }
}
