using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

// SaveDataManager class for managing game data and saving/loading game state
public class SaveDataManager : MonoBehaviour
{
    // Filepath to store the save data JSON file
    private string filepath;

    // Reference to the player GameObject
    public GameObject Player;

    // References to the MultipleChoiceActivity GameObject
    public GameObject goMCA;
    private MultipleChoiceActivity MCA;

    // JSON object to store save data
    public JObject SaveData;

    // Reference to the TextMeshProUGUI for displaying MCA high scores
    public GameObject goMCAHighScoreContent;
    private TextMeshProUGUI MCAHighScoreContent;

    // Start is called before the first frame update
    void Start()
    {
        // Set the filepath for the save data JSON file
        filepath = Path.Combine(Application.persistentDataPath, "SaveData.json");

        // Get the MultipleChoiceActivity component
        MCA = goMCA.GetComponent<MultipleChoiceActivity>();
        // Get the TextMeshProUGUI component for MCA high scores
        MCAHighScoreContent = goMCAHighScoreContent.GetComponent<TextMeshProUGUI>();

        // Initialize data and save
        this.SetData();
        this.Save();
    }

    // SetData method initializes or loads the save data
    private void SetData()
    {
        // Check if the save data file exists
        if (!File.Exists(filepath))
        {
            // If the file doesn't exist, create a new file and initialize default values

            // Open a new file stream to create the save data file
            using (FileStream file = File.Open(filepath, FileMode.Create))
            {
                // Define initial properties for the SaveData JSON object

                // Counter for the number of times the game has been played
                JProperty TimesPlayed = new JProperty("TimesPlayed", 1);

                // Counter for the number of FlowerQuest completions
                JProperty FlowerQuestCompletions = new JProperty("FlowerQuestCompletions", 0);

                // List to store names of interacted objects
                JProperty InteractedObject = new JProperty("InteractedObjects", new JArray());

                // Save the player's position and rotation
                JProperty Position = new JProperty("Position", new JObject()
            {
                new JProperty("PositionX", Player.transform.position.x),
                new JProperty("PositionY", Player.transform.position.y),
                new JProperty("PositionZ", Player.transform.position.z),
                new JProperty("RotationY", Player.transform.eulerAngles.y),
            });

                // Save data related to the MultipleChoiceActivity
                JProperty propMCA = new JProperty("MCA", new JObject()
            {
                // Save the list of objects for the MultipleChoiceActivity
                 new JProperty("Objects", JArray.Parse(JsonConvert.SerializeObject(MCA.GetObjects()))),
                 
                 // Save the activity times for the MultipleChoiceActivity
                 new JProperty("ActivityTimes", new JArray())
            });

                // Save high scores, initialized with a default high score
                JProperty propHighScores = new JProperty("HighScores", new JObject()
            {
                new JProperty("MCA", new JArray()
                {
                    new JObject ()
                    {
                        new JProperty ("Initials", "MLN"),
                        new JProperty ("Score", 500000)
                    }
                })
            });

                // Add the initialized properties to the SaveData JObject
                SaveData.Add(TimesPlayed);
                SaveData.Add(FlowerQuestCompletions);
                SaveData.Add(InteractedObject);
                SaveData.Add(Position);
                SaveData.Add(propMCA);
                SaveData.Add(propHighScores);

                return; // Exit the method after setting up initial data
            }
        }

        // If the file exists, load the existing save data

        // Parse the contents of the save data file into the SaveData JObject
        SaveData = JObject.Parse(File.ReadAllText(filepath));

        // Increment the counter for the number of times the game has been played
        SaveData["TimesPlayed"] = Convert.ToInt32(SaveData["TimesPlayed"]) + 1;

        // Set the player's position and rotation based on saved data

        // Set the player's position based on the saved data
        Player.transform.position = new Vector3()
        {
            x = float.Parse(SaveData["Position"]["PositionX"].ToString()),
            y = float.Parse(SaveData["Position"]["PositionY"].ToString()),
            z = float.Parse(SaveData["Position"]["PositionZ"].ToString())
        };

        // Set the player's rotation based on the saved data
        Quaternion rotation = Quaternion.Euler(0.0f,
            float.Parse(SaveData["Position"]["RotationY"].ToString()), 0.0f);

        Player.transform.rotation = rotation;

        // Display MCA high scores on the UI

        // Retrieve the list of high scores for the MultipleChoiceActivity
        List<JToken> mcaHighscores = SaveData["HighScores"]["MCA"].ToList();

        // Update the UI with the formatted list of high scores
        MCAHighScoreContent.text = String.Join("\r\n", mcaHighscores.OrderByDescending(t => (int)t["Score"]).Select(t => (string)t["Initials"] + ": " + (string)t["Score"]));
    }

    // Asynchronously retrieves JSON data from the save data file
    private Task<JObject> GetJsonData()
    {
        return Task.Factory.StartNew<JObject>(() =>
        {
            // Read the contents of the file and parse it into a JObject
            return JObject.Parse(File.ReadAllText(filepath));
        });
    }

    // Saves the current game state to the save data file
    public void Save()
    {
        // Update player position and rotation in the SaveData JObject
        SaveData["Position"]["PositionX"] = Player.transform.position.x;
        SaveData["Position"]["PositionY"] = Player.transform.position.y;
        SaveData["Position"]["PositionZ"] = Player.transform.position.z;
        SaveData["Position"]["RotationY"] = Player.transform.eulerAngles.y;

        // Order MultipleChoiceActivity objects by rating and update in SaveData
        SaveData["MCA"]["Objects"] = JArray.Parse(JsonConvert.SerializeObject(SaveData["MCA"]["Objects"].OrderByDescending(t => CalculateRating(t))));

        // Write the updated SaveData to the save data file
        File.WriteAllText(filepath, SaveData.ToString());
    }

    // Tries to add an interacted object to the save data, avoiding duplicates
    public void TryAddInteractedObject(string ObjectName)
    {
        // Convert the object name to lowercase for case-insensitive comparison
        string cleanObjectName = ObjectName.ToLower();

        // Check if the object name is already in the list of interacted objects
        if (SaveData["InteractedObjects"].Select(o => o.ToString().ToLower()).ToList().Contains(cleanObjectName))
        {
            return; // Exit the method if the object name already exists
        }

        // Add the new object name to the list of interacted objects
        ((JArray)SaveData["InteractedObjects"]).Add(ObjectName);

        // Save the updated data to the file
        Save();
    }

    // Increments the click statistics for a MultipleChoiceActivity object
    internal void IncrementClick(string cleanChoiceName, string cleanClickedObjectName)
    {
        // Find the object in the MultipleChoiceActivity data corresponding to the chosen option
        JToken obj = SaveData["MCA"]["Objects"].Where(t => Convert.ToString(t["name"]).ToLower() == cleanChoiceName).FirstOrDefault();

        // Exit the method if the object is not found
        if (obj == null)
        {
            return;
        }

        // Increment the correct or incorrect counter based on the comparison
        if (cleanChoiceName == cleanClickedObjectName)
        {
            obj["Correct"] = Convert.ToInt32(obj["Correct"]) + 1;
        }
        else
        {
            obj["Incorrect"] = Convert.ToInt32(obj["Incorrect"]) + 1;
        }

        // Save the updated data to the file
        Save();
    }

    // Adds the time spent on an activity to the list of recent activity times
    internal void AddActivityTime(TimeSpan timeSpan)
    {
        // Retrieve the list of activity times from the SaveData
        List<int> Times = ((JArray)SaveData["MCA"]["ActivityTimes"]).ToObject<List<int>>();

        // Add the time spent on the current activity to the list
        Times.Add(Convert.ToInt32(timeSpan.TotalMilliseconds));

        // Sort the list of activity times in ascending order
        Times.Sort();

        // Remove the oldest entry if the list exceeds the maximum allowed size
        if (Times.Count > 10)
        {
            Times.RemoveAt(Times.Count - 1);
        }

        // Update the SaveData with the modified list of activity times
        SaveData["MCA"]["ActivityTimes"] = JArray.Parse(JsonConvert.SerializeObject(Times.ToArray()));

        // Save the updated data to the file
        Save();
    }

    // Adds the time spent on a specific object within the activity
    internal void AddObjectTime(TimeSpan timeSpan, string cleanObjectName)
    {
        // Find the object in the MultipleChoiceActivity data corresponding to the clean object name
        JToken obj = SaveData["MCA"]["Objects"].Where(t => Convert.ToString(t["name"]).ToLower() == cleanObjectName).FirstOrDefault();

        // Exit the method if the object is not found
        if (obj == null)
        {
            return;
        }

        // Retrieve the current average time spent on the object
        int AverageTime = String.IsNullOrEmpty((string)obj["AverageTime"]) ? 0 : Convert.ToInt32(obj["AverageTime"]);

        // Calculate the new average time, considering the latest time spent
        AverageTime = AverageTime == 0 ? Convert.ToInt32(timeSpan.TotalMilliseconds) : (AverageTime + Convert.ToInt32(timeSpan.TotalMilliseconds)) / 2;

        // Update the object's average time in the SaveData
        obj["AverageTime"] = AverageTime;

        // Save the updated data to the file
        Save();
    }

    // Calculates the rating for a MultipleChoiceActivity object based on its statistics
    private double CalculateRating(JToken obj)
    {
        double rating = 0;

        // Extract statistics from the object
        int correct = Convert.ToInt32(obj["Correct"]);
        int incorrect = Convert.ToInt32(obj["Incorrect"]);
        double averageTime = Convert.ToInt32(obj["AverageTime"]);

        // Adjust the rating based on correct and incorrect answers
        rating = rating - correct;
        rating = rating + (incorrect * 2);

        // Adjust the rating based on average time, converted to seconds
        rating = rating + (averageTime / 1000 * 3);

        // Return the calculated rating
        return rating;
    }

    // Retrieves the next set of objects for the MultipleChoiceActivity based on their ratings
    public string[] GetNextObjects()
    {
        // Create a dictionary mapping object names to their calculated ratings
        IEnumerable<KeyValuePair<string, double>> objs = SaveData["MCA"]["Objects"].ToDictionary(t => (string)t["name"], t => CalculateRating(t)).OrderByDescending(o => o.Value);

        // Select the top 8 worst-rated objects
        IEnumerable<KeyValuePair<string, double>> badRatings = objs.Take(8);

        // Select the remaining objects
        IEnumerable<KeyValuePair<string, double>> remaining = objs.Skip(8);

        // Populate selectedObjects with 4 random objects from badRatings
        IEnumerable<string> selectedObjects = badRatings.OrderBy(o => UnityEngine.Random.value).Select(o => o.Key).Take(4);

        // Concatenate 4 more random objects from remaining
        selectedObjects = selectedObjects.Concat(remaining.OrderBy(o => UnityEngine.Random.value).Select(o => o.Key).Take(4));

        // Convert the selected objects to an array and return
        return selectedObjects.ToArray();
    }

    // Attempts to add a new high score to the list of high scores for the MultipleChoiceActivity
    internal void TryAddHighScore(string initials, double score)
    {
        // Flag to determine whether to add the high score
        bool AddHighScore = false;

        // Retrieve the list of high scores for the MultipleChoiceActivity
        List<JToken> highScores = ((JArray)SaveData["HighScores"]["MCA"]).OrderBy(t => (double)t["Score"]).ToList();

        // Check if there are fewer than 10 high scores, or if the new score is higher than the lowest current score
        if (highScores.Count() < 10 || (double)highScores.FirstOrDefault()["Score"] < score)
        {
            AddHighScore = true;

            // If there are already 10 high scores, remove the lowest score to make space for the new one
            if (highScores.Count() >= 10)
            {
                highScores = highScores.Skip(1).Take(9).ToList();
            }
        }

        // If no high score needs to be added, exit the method
        if (!AddHighScore)
        {
            return;
        }

        // Add the new high score to the list
        highScores.Add(
            new JObject()
            {
            new JProperty("Initials", initials),
            new JProperty("Score", Convert.ToInt32(score))
            }
        );

        // Update the SaveData with the modified list of high scores
        SaveData["HighScores"]["MCA"] = JArray.Parse(JsonConvert.SerializeObject(highScores.OrderByDescending(t => (double)t["Score"]).ToArray()));

        // Update the UI text displaying high scores
        MCAHighScoreContent.text = String.Join("\r\n", highScores.OrderByDescending(t => (int)t["Score"]).Select(t => (string)t["Initials"] + ": " + (string)t["Score"]));

        // Save the updated data to the file
        Save();
    }
}
