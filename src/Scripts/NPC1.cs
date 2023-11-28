using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NPC1 : MonoBehaviour
{
    // Reference to the InstructionText GameObject
    private GameObject InstructionText;

    // Current goal that the NPC wants the player to achieve
    public string currentGoal;

    // Array of available grabbable objects
    GameObject[] AvailableObjects { get; set; }

    private void Start()
    {
        // Find all GameObjects with the "grab" tag and initialize AvailableObjects
        AvailableObjects = GameObject.FindGameObjectsWithTag("grab");

        // Find the InstructionText script in the scene
        InstructionText = GameObject.FindGameObjectWithTag("Dialog1");

        // Set the initial current goal to "'Object'"
        currentGoal = "'Object'";

        // Update the UI to display the current goal
        UpdateCurrentGoal();
    }

    // Remove an item from an array at a specific index
    private static GameObject[] RemoveItemFromIndex(int index, GameObject[] array)
    {
        var list = new List<GameObject>(array);
        list.RemoveAt(index);
        return list.ToArray();
    }

    // Update the current goal and UI text
    public void UpdateCurrentGoal()
    {
        // If there are no more available objects, display a completion message
        if (AvailableObjects.Length == 0)
        {
            InstructionText.GetComponent<TextMeshProUGUI>().text = "Thank you for getting everything for me :)";
            return;
        }

        // Choose a random index from the AvailableObjects array
        int randomIndex = UnityEngine.Random.Range(0, AvailableObjects.Length);

        // Update the current goal and remove the chosen object from the AvailableObjects array
        currentGoal = AvailableObjects[randomIndex].name;
        AvailableObjects = RemoveItemFromIndex(randomIndex, AvailableObjects);

        // Update the UI text to instruct the player
        InstructionText.GetComponent<TextMeshProUGUI>().text = "Grab a(n) " + currentGoal + " for me, please.";
    }
}
