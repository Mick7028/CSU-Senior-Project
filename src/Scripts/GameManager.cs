using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Windows;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.DebugUI;
using InputDevice = UnityEngine.XR.InputDevice;

public class GameManager : MonoBehaviour
{
    bool devicesInitialized = false; // Flag to track if devices are initialized
    List<InputDevice> devices = new List<InputDevice>(); // List to hold input devices

    // Flags to track button presses
    private bool APressed = false; 
    private bool BPressed = false; 
    private bool MenuPressed = false; 

    InputDevice RightHand; // Reference to the right hand input device
    InputDevice LeftHand; // Reference to the left hand input device

    public GameObject Hovered_Object_Names; // Reference to a GameObject for displaying hovered object names
    public GameObject FlowerQuest; // Reference to a GameObject for handling flower quest
    public GameObject Inventory; // Reference to a GameObject for managing inventory
    internal GameObject CurrentlyHovering; // Reference to the currently hovered object

    // Reference to the SaveData GameObject, used for managing save data
    public GameObject SaveData;

    // Reference to the MainMenu GameObject, representing the main menu in the game
    public GameObject MainMenu;

    // TouchScreenKeyboard instance, used for on-screen keyboard input
    public TouchScreenKeyboard keyboard;

    // Reference to the GameObject for handling player initials input
    public GameObject goInitials;
    // TMP_InputField for capturing and displaying player initials
    private TMP_InputField Initials;

    // Reference to the GameObject for displaying player score
    public GameObject goScore;
    // TextMeshProUGUI for displaying player score
    private TextMeshProUGUI Score;

    // Reference to the last active menu GameObject, used for toggling menu visibility
    private GameObject lastMenuActive;

    // Start is called before the first frame update
    void Start()
    {
        // Initializes references to UI elements
        Initials = goInitials.GetComponent<TMP_InputField>();
        Score = goScore.GetComponent<TextMeshProUGUI>();

        // Sets the last active menu to the main menu at the start
        lastMenuActive = MainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        // Update currently hovered object name display
        if (CurrentlyHovering != null)
        {
            // Extracts and displays the name of the currently hovered object, removing unwanted characters
            Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text = Regex.Replace(CurrentlyHovering.name, @"[^a-zA-Z \\']", "").Trim();
        }
        else
        {
            // If no object is currently hovered, clear the display text
            Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text = "";
        }

        // Initialize input devices if not already done
        if (!devicesInitialized)
        {
            TryInitDevices();
        }

        // Check for A button press and trigger actions
        if (TryGetAPressed() && !APressed)
        {
            APressed = true;

            // Checks if the currently hovered object has a valid name
            if (Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text != "")
            {
                // Retrieves the AudioManager and SaveData objects
                GameObject goAudioManager = GameObject.FindGameObjectWithTag("AudioManager");
                GameObject goSaveData = GameObject.FindGameObjectWithTag("SaveData");

                // Plays sound if AudioManager is available and not currently playing a sound
                if (goAudioManager != null && Hovered_Object_Names.activeSelf == true)
                {
                    if (goAudioManager.TryGetComponent<AudioManager>(out var audioManager))
                    {
                        if (audioManager.isPlayingSound == false)
                        {
                            audioManager.StartSoundCoroutine(Regex.Replace(Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text, @"[^a-zA-Z \\']", "").Trim());
                        }
                    }
                }
                // Adds the interacted object to the SaveData if available
                if (goSaveData != null)
                {
                    if (goSaveData.TryGetComponent<SaveDataManager>(out var SaveData))
                    {
                        SaveData.TryAddInteractedObject(Regex.Replace(Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text, @"[^a-zA-Z \\']", "").Trim());
                    }

                }

                // Trigger specific actions based on the hovered object's name
                if (Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text.ToLower() == "mcanpc")
                {
                    GameObject goMCA = GameObject.FindGameObjectWithTag("MCA");

                    if (goMCA != null)
                    {
                        MultipleChoiceActivity MCA = goMCA.GetComponent<MultipleChoiceActivity>();
                        MCA.APressed();
                    }
                }

                // Checks if the currently hovered object's name is "peasant nolant"
                if (Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text.ToLower() == "peasant nolant")
                {
                    // Finds the FlowerQuest GameObject using its tag
                    GameObject goFQ = GameObject.FindGameObjectWithTag("FlowerQuest");

                    // If FlowerQuest GameObject is found, this triggers the APressed method in the FlowerQuest script
                    if (goFQ != null)
                    {
                        FlowerQuest FQ = goFQ.GetComponent<FlowerQuest>();
                        FQ.APressed();
                    }
                }
            }

        }

        // Check for B button press and trigger actions
        if (TryGetBPressed() && !BPressed)
        {
            string HudText = Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text;
            string CleanHudText = HudText.ToLower();
            BPressed = true;

            // Checks if the currently hovered object has a valid name
            if (CleanHudText != "")
            {
                // Triggers actions based on the cleaned name
                if (CleanHudText == "peasant nolant")
                {
                    FlowerQuest.GetComponent<FlowerQuest>().Interact();
                }

                // Checks if the cleaned HUD text contains the word "flower"
                if (CleanHudText.Contains("flower"))
                {
                    // Checks if the flower type is valid for the FlowerQuest
                    if (FlowerQuest.GetComponent<FlowerQuest>().isValidFlower(CleanHudText))
                    {
                        // Adds the flower to your inventory
                        Inventory.GetComponent<InventoryManager>().AddItem(HudText);

                        // Disables the mesh renderer and collider of the currently hovered object
                        CurrentlyHovering.GetComponent<MeshRenderer>().enabled = false;
                        CurrentlyHovering.GetComponent<MeshCollider>().enabled = false;

                        // Sets the interacted flag in the Hoverable component to true
                        CurrentlyHovering.GetComponent<Hoverable>().interacted = true;

                        // Increments the count for the corresponding menu item in the FlowerQuest
                        FlowerQuest.GetComponent<FlowerQuest>().IncrementMenuItem(HudText);

                        // Starts a coroutine to respawn the currently hovered object after a delay
                        StartCoroutine(Respawn(CurrentlyHovering));
                    }
                }

                // Adds the interacted object to the SaveData if available
                GameObject goSaveData = GameObject.FindGameObjectWithTag("SaveData");
                if (goSaveData != null)
                {
                    if (goSaveData.TryGetComponent<SaveDataManager>(out var SaveData))
                    {
                        // Removes special characters and trims the name before adding to the SaveData
                        SaveData.TryAddInteractedObject(Regex.Replace(Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text, @"[^a-zA-Z \\']", "").Trim());
                    }

                }

                // Triggers actions for the MultipleChoiceActivity based on the hovered object's name
                GameObject goMCA = GameObject.FindGameObjectWithTag("MCA");
                if (goMCA != null)
                {
                    MultipleChoiceActivity MCA = goMCA.GetComponent<MultipleChoiceActivity>();

                    // If the hovered object's name is "mcanpc," trigger BPressed in the MultipleChoiceActivity
                    if (Hovered_Object_Names.GetComponent<TextMeshProUGUI>().text.ToLower() == "mcanpc")
                    {
                        MCA.BPressed();
                    }
                    // If MultipleChoiceActivity is in progress, trigger BPressed in the MultipleChoiceActivity
                    if (GameObject.FindGameObjectWithTag("MCA").GetComponent<MultipleChoiceActivity>().CurrentlyPlaying())
                    {
                        MCA.BPressed();
                    }
                }
            }
        }

        if (TryGetMenuPressed() && !MenuPressed)
        {
            MenuPressed = true;

            bool currentLastMenuState = lastMenuActive.activeSelf;

            HideMenus();
            lastMenuActive.SetActive(!currentLastMenuState);
            SaveData.GetComponent<SaveDataManager>().Save();
        }
    }

    // Attempt to initialize input devices
    void TryInitDevices()
    {
        InputDevices.GetDevices(devices);

        // If there are 3 devices, identify the right hand device
        if (devices.Count == 3)
        {
            foreach (InputDevice device in devices)
            {
                if ((device.characteristics & InputDeviceCharacteristics.Right) != 0)
                {
                    RightHand = device;
                }

                if ((device.characteristics & InputDeviceCharacteristics.Left) != 0)
                {
                    LeftHand = device;
                }
            }

            // Mark devices as initialized
            devicesInitialized = true;
        }
    }

    // Check if A button is pressed
    bool TryGetAPressed()
    {
        // If devices are not initialized, return false
        if (!devicesInitialized)
        {
            return false;
        }

        // Check if primary button is pressed on the right hand device
        if (RightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool pressed))
        {
            if (pressed)
            {
                return true;
            }
        }

        APressed = false; // Reset APressed flag
        return false;
    }

    // Check if B button is pressed
    bool TryGetBPressed()
    {
        // If devices are not initialized, return false
        if (!devicesInitialized)
        {
            return false;
        }

        // Check if secondary button is pressed on the right hand device
        if (RightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool pressed))
        {
            if (pressed)
            {
                return true;
            }
        }

        BPressed = false; // Reset BPressed flag
        return false;
    }

    bool TryGetMenuPressed()
    {
        // If devices are not initialized, return false
        if (!devicesInitialized)
        {
            return false;
        }

        // Check if primary button is pressed on the left hand device
        if (LeftHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out bool pressed))
        {
            if (pressed)
            {
                return true;
            }
        }

        MenuPressed = false; // Reset MenuPressed flag
        return false;
    }

    // Coroutine to respawn an object after a delay
    private IEnumerator Respawn(GameObject Object)
    {
        yield return new WaitForSeconds(5.0f);

        // Enable renderer, collider, and reset interacted state
        Object.GetComponent<MeshRenderer>().enabled = true;
        Object.GetComponent<MeshCollider>().enabled = true;
        Object.GetComponent<Hoverable>().interacted = false;
    }

    // Opens the on-screen keyboard
    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    // Submits high score and initials
    public void Submit()
    {
        SaveData.GetComponent<SaveDataManager>().TryAddHighScore(Initials.text.ToUpper(), Convert.ToDouble(Score.text));
    }

    // Hides all menus
    public void HideMenus()
    {
        foreach (GameObject menu in GameObject.FindGameObjectsWithTag("Menu"))
        {
            menu.SetActive(false);    
        }
    }

    // Updates the last active menu
    public void UpdateLastMenu(GameObject menu)
    {
        lastMenuActive = menu;
    }

    // Appends a letter to the initials input field
    public void AppendInitial(string letter)
    {
        if (Initials.text.Length > 2) return;

        Initials.text += letter;
    }

    // Deletes the last character in the initials input field
    public void DeleteLastInitial ()
    {
        if (Initials.text.Length == 0) return;

        Initials.text = Initials.text.Substring(0, Initials.text.Length - 1);
    }
}
