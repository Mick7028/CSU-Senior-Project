using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class MultipleChoiceActivity : MonoBehaviour
{
    private int State = 0; // Current state of the MCA

    public GameObject NPC3; // Reference to the NPC GameObject
    public GameObject MCA_Dialog; // Reference to the Text GameObject for displaying quest information
    public GameObject PlayerDetector; // Reference to the player detection area
    public GameObject Hovered_Object_Names; // Reference to the object names displayed in HUD
    public GameObject Player; // Reference to the player GameObject
    public GameObject MCA_Text; // Reference to the NPC hover text

    float range = 14.8f;

    // Array of multiple choice options GameObjects
    public GameObject[] MCA_Choices; // Array of multiple choice options GameObjects

    // Array of spawn points for multiple choice GameObjects
    private Vector3[] SpawnPoints =
    {
        new Vector3(-37.00f, 22.09f, 6.27f),
        new Vector3(-40.18f, 22.09f, 2.47f),
        new Vector3(-41.54f, 22.09f, -1.77f),
        new Vector3(-37.97f, 22.09f, -4.78f),
        new Vector3(-35.75f, 23.02f, -7.38f),
        new Vector3(-32.71f, 22.09f, -9.66f),
        new Vector3(-28.52f, 22.09f, -8.75f),
        new Vector3(-25.52f, 22.09f, -6.07f)
    };

    private GameObject[] ObjectsAppearing; // Array to store the objects appearing in the game
    private int IOA; //Index of Objects Appearing

    public GameObject goSaveData;
    SaveDataManager Savedata;

    // Start and end time variables for overall activity
    private DateTime StartTime;
    private DateTime EndTime;

    // Start and end time variables for individual objects
    private DateTime ObjectStartTime;
    private DateTime ObjectEndTime;

    // Score and IncorrectClicks variables
    private double Score;
    private double IncorrectClicks;

    public GameObject GM; // Reference to GameManager GameObject
    public GameObject goEnterInitials; // Reference to EnterInitials GameObject
    public GameObject goTextScore; // Reference to TextScore GameObject


    // Start is called before the first frame update
    void Start()
    {
        // Initialize SaveDataManager
        Savedata = goSaveData.GetComponent<SaveDataManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks player distance and changes HUD if in the multiple choice area
        if (Vector3.Distance(PlayerDetector.transform.position, Player.transform.position) <= range)
        {
            if (Hovered_Object_Names.activeSelf == true)
            {
                Hovered_Object_Names.SetActive(false);
                MCA_Text.SetActive(true);
            }
            
        }
        else
        {
            if (!Hovered_Object_Names.activeSelf)
            {
                Hovered_Object_Names.SetActive(true);
                MCA_Text.SetActive(false);
            }
        }


    }

    // Starts the Multiple Choice Activity
    private void StartMCA()
    {
        // Initialize variables for the start of the activity
        StartTime = DateTime.Now;
        Score = 1000000;
        IncorrectClicks = 0;

        // Gets the next set of objects from SaveDataManager
        string[] nextObjects = Savedata.GetNextObjects();
        List<GameObject> PlacedObjects = new List<GameObject>();

        // Place objects at spawn points and activate them
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            GameObject nextObject = MCA_Choices.Where(go => go.name.ToLower() == nextObjects[i].ToLower()).FirstOrDefault();
            PlacedObjects.Add(nextObject);
            nextObject.transform.position = SpawnPoints[i];
            nextObject.SetActive(true);
        }

        // Shuffle and store placed objects
        ObjectsAppearing = PlacedObjects.OrderBy(o => UnityEngine.Random.value).ToArray();
        IOA = 0;

        // Set start time for the first object
        ObjectStartTime = DateTime.Now;

        // Display the name of the first object in HUD
        MCA_Text.GetComponent<TextMeshProUGUI>().text = ObjectsAppearing[IOA].name;

        // Deactivate NPC GameObject
        NPC3.SetActive(false);

    }

    // State enumeration to track the current state of the activity
    // State 0 is not interacted with yet
    // State 1 is when user hovers with ray interactor
    // State 2 is when user hovers and presses A button (displays instructions)
    // State 3 is when and presses B (starts activity) and user is playing the game

    // Method called when NPC is hovered
    internal void NPCHovered()
    {
        if (State == 0) 
        {
            State = 1;
            StartCoroutine(DisplayNPCText());
        }
      

        
    }

    // Coroutine to display NPC text based on the current state
    private IEnumerator DisplayNPCText ()
    {
         if (State == 1) //Activity not started
         {
            // Hover to show start game or see instructions
            MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "Press B to Start or Press A for Instructions";
            yield return new WaitForSeconds(4.0f);
            if (State != 1)
                {
                   yield break;

                }
            MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "";
            State = 0;
            yield break;
        }

        // Display instructions for the activity
        if (State == 2)
         {
            MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "In this activity, 8 objects will appear around you.";
            yield return new WaitForSeconds(4.0f);
            if (State == 3) yield break;
            MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "The name of one of the objects will appear on the HUD...";
            yield return new WaitForSeconds(4.0f);
            if (State == 3) yield break;
            MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "press the B button while hovering the correct object...";
            yield return new WaitForSeconds(4.0f);
            if (State == 3) yield break;
            MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "and a new word will appear on the HUD.";
            yield return new WaitForSeconds(4.0f);
            if (State == 3) yield break;
            MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "The goal is to pick the 8 objects as fast and as accurately as possible.";
            yield return new WaitForSeconds(5.0f);
            if (State == 3) yield break;
            MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "There will be three sets of 8 objects per game, try to get the fastest time!";
            yield return new WaitForSeconds(5.0f);
            if (State == 3) yield break;
            MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "";
            State = 0;
            yield break;
        }

        if (State == 3)
        {
            // So the instructions go away when the activity starts
        }
    }

    // When 'A' button is pressed while hovering NPC shows instructions
    internal void APressed ()
    {
        if (State == 0 || State == 1) 
        {
            State = 2;
            StartCoroutine(DisplayNPCText());
        }
    }

    // When 'B' button is pressed while hovering NPC start activity
    internal void BPressed()
    {

        if (State == 0 || State == 1 || State == 2) 
        {
            State = 3;
            MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "";
            StartMCA();
            return;
        }

        if (State == 3)
        {
            CheckSelection();
        }
    }

    // Checks if the player's selection is correct
    private void CheckSelection()
    {
        string cleanObjectName = Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text.ToLower();

        // Increment correct click count for the selected object
        if (cleanObjectName != "")
        {
            Savedata.IncrementClick(ObjectsAppearing[IOA].name.ToLower(), cleanObjectName);
        }

        // Increment incorrect click count for the selected object
        if (ObjectsAppearing[IOA].name.ToLower() != cleanObjectName)
        {
            IncorrectClicks++;
        }

        // Checks if the selected object is correct
        if (cleanObjectName == ObjectsAppearing[IOA].name.ToLower())
        {
            // Updates time for the current object
            ObjectEndTime =  DateTime.Now;
            Savedata.AddObjectTime(ObjectEndTime - ObjectStartTime, cleanObjectName);

            // Resets start time and deactivate current object
            ObjectStartTime = DateTime.Now;
            ObjectsAppearing[IOA].SetActive(false);
            IOA++;

            // Checks if all objects have been selected
            if (IOA == SpawnPoints.Length)
            {
                // Calculates score, finishes the activity and displays the score
                EndTime = DateTime.Now;
                Score -= (((EndTime - StartTime).TotalMilliseconds * 15) + (IncorrectClicks * 5000));
                Score = Math.Round(Score);
                Savedata.AddActivityTime(EndTime - StartTime);
                NPC3.SetActive(true);
                MCA_Dialog.GetComponent<TextMeshProUGUI>().text = "Your score is: " + Score.ToString();
                State = 0;
                MCA_Text.GetComponent<TextMeshProUGUI>().text = "";
                GM.GetComponent<GameManager>().HideMenus();
                goTextScore.GetComponent<TextMeshProUGUI>().text = Score.ToString();
                goEnterInitials.SetActive(true);
                return;
            }

            // Displays the name of the next object
            MCA_Text.GetComponent<TextMeshProUGUI>().text = ObjectsAppearing[IOA].name;
        }
    }

    // Checks if the player is currently playing the activity and is in activity area
    public bool CurrentlyPlaying()
    {
        return State == 3 && Vector3.Distance(PlayerDetector.transform.position, Player.transform.position) <= range;
    }

    // Gets information about player's interaction with multiple choice objects
    internal dynamic[] GetObjects()
    {
        return MCA_Choices.Select(o => new
        {
            name = o.name,
            Correct = 0,
            Incorrect = 0,
            AverageTime = 0,


        }).ToArray();
    }
}


