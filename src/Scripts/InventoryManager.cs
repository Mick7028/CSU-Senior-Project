using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<string, int> inventory = new Dictionary<string, int>(); // Dictionary to store inventory items and their quantities

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Add an item to the inventory or increment its quantity if already present
    public void AddItem(string key)
    {
        if (inventory.ContainsKey(key))
        {
            inventory[key]++; // Increment quantity if item is already in inventory
            return;
        }

        inventory.Add(key, 1); // Add the item to inventory with quantity 1
    }

    // Remove an item from the inventory or decrement its quantity if multiple
    public void RemoveItem(string key)
    {
        if (!inventory.ContainsKey(key))
        {
            return; // Return if item is not present in inventory
        }

        if (inventory[key] > 1)
        {
            inventory[key]--; // Decrement quantity if multiple of the item
            return;
        }

        inventory.Remove(key); // Remove the item from inventory if only one left
    }

    // Remove all items with specified names from the inventory
    public void RemoveAllWithNameOf(List<string> items)
    {
        foreach (string item in items)
        {
            if (inventory.ContainsKey(item))
            {
                inventory.Remove(item); // Remove the item from inventory
            }
        }
    }

    // Get the entire inventory dictionary
    public Dictionary<string, int> GetInventory()
    {
        return inventory;
    }
}
