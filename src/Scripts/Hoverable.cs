using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hoverable : MonoBehaviour
{
    // Flag to track if the object is currently being hovered
    public bool isHovering = false;

    // Reference to the XR Grab Interactable component
    private XRGrabInteractable hoverInteractable;

    // Flag to track if the object has been interacted with
    internal bool interacted = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the XR Grab Interactable component attached to this GameObject
        hoverInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the Select Entered and Exit events
        hoverInteractable.hoverEntered.AddListener(OnHover);
        hoverInteractable.hoverExited.AddListener(OnHoverExit);
    }

    // Update is called once per frame
    void Update()
    {
        // Enable the MeshCollider when it's not interacted with
        MeshCollider mc = this.GetComponent<MeshCollider>();
        if (mc != null && !interacted)
        {
            if (mc.enabled == false)
            {
                mc.enabled = true;
            }
        }

        // Disable MeshColliders of nearby flower, grass, and plant objects
        Collider[] cols = Physics.OverlapSphere(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>().position, 2f);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].name == this.name)
            {
                if (cols[i].name.ToLower().Contains("flower") || cols[i].name.ToLower().Contains("grass") || cols[i].name.ToLower().Contains("plant"))
                {
                    MeshCollider mc2 = cols[i].gameObject.GetComponent<MeshCollider>();
                    if (mc2 != null)
                    {
                        mc2.enabled = false;
                    }
                }
            }
        }
    }

    // Called when the object is hovered over by a hand in virtual reality
    public void OnHover(HoverEnterEventArgs args)
    {
        // Check if the object is already being hovered
        if (isHovering == true)
        {
            return;
        }

        // Get the XRGrabInteractable component to check if the object is being hovered
        var grabInteractable = GetComponent<XRGrabInteractable>();

        // Check if the object is being hovered
        if (grabInteractable.isHovered)
        {
            // Get the interactor (hand) that is hovering over the object
            var interactor = grabInteractable.interactorsHovering;

            // Check if the interactor is not null
            if (interactor != null)
            {
                // Set isHovering to true
                isHovering = true;

                // Get the GameObject being hovered over
                GameObject ObjectHovered = interactor.FirstOrDefault().interactablesHovered.FirstOrDefault().colliders.FirstOrDefault().gameObject;

                // Find the GameManager object in the scene
                GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");

                // Update the CurrentlyHovering reference in the GameManager
                GameManager.GetComponent<GameManager>().CurrentlyHovering = ObjectHovered;

                // Check if the hovered object is a specific NPC ("mcanpc")
                if (ObjectHovered.name.ToLower() == "mcanpc")
                {
                    // Find the object with the "MCA" tag in the scene
                    GameObject goMCA = GameObject.FindGameObjectWithTag("MCA");

                    // Check if the object is not null
                    if (goMCA != null)
                    {
                        // Get the MultipleChoiceActivity component from the object
                        MultipleChoiceActivity MCA = goMCA.GetComponent<MultipleChoiceActivity>();

                        // Call the NPCHovered method on the MultipleChoiceActivity
                        MCA.NPCHovered();
                    }
                }

                // Check if the hovered object is a specific NPC ("peasant nolant")
                if (ObjectHovered.name.ToLower() == "peasant nolant")
                {
                    // Find the object with the "FlowerQuest" tag in the scene
                    GameObject goFQ = GameObject.FindGameObjectWithTag("FlowerQuest");

                    // Check if the object is not null
                    if (goFQ != null)
                    {
                        // Get the FlowerQuest component from the object
                        FlowerQuest FQ = goFQ.GetComponent<FlowerQuest>();

                        // Call the NPCHovered method on the FlowerQuest
                        FQ.NPCHovered();
                    }
                }
            }
        }
    }

    // Event handler for when the hover ends
    public void OnHoverExit(HoverExitEventArgs args)
    {
        // Reset the hovering flag and CurrentlyHovering reference
        isHovering = false;

        GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");
        GameManager.GetComponent<GameManager>().CurrentlyHovering = null;
        
    }
}
