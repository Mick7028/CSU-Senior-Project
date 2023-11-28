using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FlowerQuest : MonoBehaviour
{
    public GameObject NPC; // Reference to the NPC GameObject
    public GameObject FlowerText; // Reference to the Text GameObject for displaying quest information
    private Dictionary<Tuple<string, int>, bool> CurrentFlowerDic; // Dictionary to store current flower quest progress
    private int State; // Current state of the flower quest

    public GameObject[] MenuItems; // Array of GameObjects representing quest menu items
    public Sprite[] flowers; // Array of flower sprites

    // Start is called before the first frame update
    void Start()
    {
        InitFlowerQuest(); // Initialize the flower quest
    }

    // Initialize the flower quest
    private void InitFlowerQuest()
    {
        CurrentFlowerDic = new Dictionary<Tuple<string, int>, bool>(); // Initialize the dictionary
        State = 0; // Set initial state to 0
        List<string> possibilities = new List<string> { "Blue Flower", "Yellow Flower", "Black Flower",
            "Light Blue Flower", "Orange Flower", "Red Flower", "Purple Flower" }; // List of possible flower names

        for (int i = 0; i < 3; i++)
        {
            string Flower = possibilities[new System.Random().Next(possibilities.Count)]; // Choose a random flower
            possibilities.Remove(Flower);

            Tuple<string, int> flowerType = new Tuple<string, int>(Flower, UnityEngine.Random.Range(1, 4)); // Create a tuple with flower name and quantity

            CurrentFlowerDic.Add(flowerType, false); // Add flower tuple to the dictionary with initial completion status as false
            SetMenuItem(MenuItems[i], flowerType); // Set menu item visuals based on the flower tuple
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Perform interaction with the flower quest
    public void Interact()
    {

        if (State == 3)
        {
            InventoryManager inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
            this.CheckFlowers(inventory); // Check the collected flowers in the inventory
        }

        if (State == 0 || State == 1)
        {
            State = 3;
        }

        StartCoroutine(DisplayText()); // Display text based on the quest state
    }

    // Check collected flowers against the quest requirements
    private void CheckFlowers(InventoryManager InvMan)
    {
        Dictionary<string, int> inv = InvMan.GetInventory();
        List<Tuple<string, int>> keys = CurrentFlowerDic.Keys.ToList();
        foreach (Tuple<string, int> key in keys)
        {
            if (CurrentFlowerDic[key] == true) continue;

            if (inv.ContainsKey(key.Item1))
            {
                if (inv[key.Item1] >= key.Item2) // Need to update to == eventually, user needs to learn numbers
                {
                    InvMan.RemoveItem(key.Item1);
                    CurrentFlowerDic[key] = true;
                }
            }
        }

        bool complete = true;
        foreach (Tuple<string, int> key in keys)
        {
            if (CurrentFlowerDic[key] == false)
            {
                complete = false;
                break;
            }
        }

        if (complete)
        {
            State = 4;
            InvMan.RemoveAllWithNameOf(keys.Select(t => t.Item1).ToList()); // Remove all collected flowers from inventory
        }
    }

    // Check if the given flower name is valid for the quest
    public bool isValidFlower(string flower)
    {
        bool Valid = CurrentFlowerDic.Where(kvp => !kvp.Value).ToDictionary(f => f).Keys.Select(kvp => kvp.Key.Item1.ToLower()).ToList().Contains(flower.ToLower());
        return Valid;
    }

    // Display text based on the quest state
    private IEnumerator DisplayText()
    {
        // Display initial instructions
        if (State == 1)
        {
            FlowerText.GetComponent<TextMeshProUGUI>().text = "Press B to Start or Press A for Instructions";
            yield return new WaitForSeconds(4.0f);
            if (State != 1)
            {
                yield break;

            }
            FlowerText.GetComponent<TextMeshProUGUI>().text = "";
            State = 0;
            yield break;


        }

        // Display additional instructions
        if (State == 2)
        {
            FlowerText.GetComponent<TextMeshProUGUI>().text = "In this activity, 8 objects will appear around you.";
            yield return new WaitForSeconds(4.0f);
            FlowerText.GetComponent<TextMeshProUGUI>().text = "The name of one of the objects will appear on the HUD...";
            yield return new WaitForSeconds(4.0f);
            FlowerText.GetComponent<TextMeshProUGUI>().text = "press the B button while hovering the correct object...";
            yield return new WaitForSeconds(4.0f);
            FlowerText.GetComponent<TextMeshProUGUI>().text = "and a new word will appear on the HUD.";
            yield return new WaitForSeconds(4.0f);
            FlowerText.GetComponent<TextMeshProUGUI>().text = "The goal is to pick the 8 objects as fast and as accurately as possible.";
            yield return new WaitForSeconds(5.0f);
            FlowerText.GetComponent<TextMeshProUGUI>().text = "There will be three sets of 8 objects per game, try to get the fastest time!";
            yield return new WaitForSeconds(5.0f);
            FlowerText.GetComponent<TextMeshProUGUI>().text = "";
            State = 0;
        }

        // Activity starts, flower colors and amounts calculated
        if (State == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                MenuItems[i].SetActive(true);
            }

            // Loops through the flowers and requests them
            foreach (KeyValuePair<Tuple<string, int>, bool> KVP in CurrentFlowerDic)
            {
                if (KVP.Value) continue;
                FlowerText.GetComponent<TextMeshProUGUI>().text = "Please give me " + KVP.Key.Item2 + " " + KVP.Key.Item1;
                yield return new WaitForSeconds(3.0f);
            }
            FlowerText.GetComponent<TextMeshProUGUI>().text = "";
            yield break;
        }

        // Display completion message
        if (State == 4)
        {
            for (int i = 0; i < 3; i++)
            {
                MenuItems[i].SetActive(false);
            }
            FlowerText.GetComponent<TextMeshProUGUI>().text = $"Thanks!";
            yield return new WaitForSeconds(3.0f);
            FlowerText.GetComponent<TextMeshProUGUI>().text = "";

            SaveDataManager SDM = GameObject.FindGameObjectWithTag("SaveData").gameObject.GetComponent<SaveDataManager>();
            SDM.SaveData["FlowerQuestCompletions"] = Convert.ToInt32(SDM.SaveData["FlowerQuestCompletions"]) +1;
            SDM.Save();
            InitFlowerQuest(); // Reset the flower quest
            yield break;
        }
    }

    // Set visuals for a menu item based on the given flower type
    private void SetMenuItem(GameObject menuItem, Tuple<string, int> flower)
    {
        // Gets the sprite for the flower
        Sprite flowerSprite = GetFlowerSprite(flower.Item1);

        // Sets up the sprite for the menu item
        GameObject goImage = menuItem.transform.GetChild(0).gameObject;
        UnityEngine.UI.Image img = goImage.GetComponent<UnityEngine.UI.Image>();
        img.sprite = flowerSprite;

        // Sets up the text for the menu item
        GameObject goText = menuItem.transform.GetChild(1).gameObject;
        TextMeshProUGUI text = goText.GetComponent<TextMeshProUGUI>();
        text.text = "0/" + flower.Item2;
    }

    // Get the sprite for the given flower name
    private Sprite GetFlowerSprite(string flowerName)
    {
        string cleanFlowerName = flowerName.ToLower();

        if (cleanFlowerName == "black flower")
        {
            return flowers[0];
        }
        if (cleanFlowerName == "blue flower")
        {
            return flowers[1];
        }
        if (cleanFlowerName == "light blue flower")
        {
            return flowers[2];
        }
        if (cleanFlowerName == "orange flower")
        {
            return flowers[3];
        }
        if (cleanFlowerName == "purple flower")
        {
            return flowers[4];
        }
        if (cleanFlowerName == "red flower")
        {
            return flowers[5];
        }
        if (cleanFlowerName == "yellow flower")
        {
            return flowers[6];
        }

        return flowers[0];
    }

    // Increment the count of a menu item
    public void IncrementMenuItem(string FlowerName)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject goImage = MenuItems[i].transform.GetChild(0).gameObject;
            UnityEngine.UI.Image img = goImage.GetComponent<UnityEngine.UI.Image>();

            if (img.sprite.name.ToLower() != FlowerName.ToLower()) continue;

            GameObject goText = MenuItems[i].transform.GetChild(1).gameObject;
            TextMeshProUGUI text = goText.GetComponent<TextMeshProUGUI>();
            int index = text.text.IndexOf('/');
            int Quantity = Int32.Parse(text.text.Substring(0, index));
            int Total = Int32.Parse(text.text.Substring(index + 1, text.text.Length - index - 1));
            Quantity++;
            text.text = Quantity + "/" + Total;

            return;
        }
    }

    // Called when the NPC is hovered
    internal void NPCHovered()
    {
        // Check if the quest is in the initial state
        if (State == 0)
        {
            State = 1;
            StartCoroutine(DisplayText()); // Start coroutine to display instructions
        }
    }

    // Called when 'A' button is pressed
    internal void APressed()
    {
        // Checks if the quest is in the initial state or instruction state
        if (State == 0 || State == 1)
        {
            State = 2;
            StartCoroutine(DisplayText()); // Starts coroutine to display additional instructions
        }
    }
}
