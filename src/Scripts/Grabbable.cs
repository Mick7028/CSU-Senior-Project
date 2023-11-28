using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Grabbable : MonoBehaviour
{
    // Reference to the XR Grab Interactable component
    private XRGrabInteractable grabInteractable;

    // Start is called before the first frame update
    void Start()
    {
        // Get the XR Grab Interactable component attached to this GameObject
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the Select Entered event
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic (if any) can go here
    }

    // Event handler for when the object is grabbed
    public void OnGrab(SelectEnterEventArgs args)
    {
        // Get the XR Grab Interactable component
        var grabInteractable = GetComponent<XRGrabInteractable>();

        // Check if the object is selected (grabbed)
        if (grabInteractable.isSelected)
        {
            // Get the interactor (controller) that grabbed the object
            var interactor = grabInteractable.firstInteractorSelecting as XRBaseInteractor;
            if (interactor != null)
            {
                // Get the GameObject that was grabbed
                GameObject ObjectGrabbed = interactor.interactablesSelected.FirstOrDefault().colliders.FirstOrDefault().gameObject;

                // Find the NPC1 GameObject in the scene
                GameObject NPC = GameObject.FindGameObjectWithTag("NPC1");

                // Compare the current goal of the NPC with the name of the grabbed object
                if (NPC.GetComponent<NPC1>().currentGoal.ToLower() == ObjectGrabbed.name.ToLower())
                {
                    // Update the NPC's current goal and UI text
                    NPC.GetComponent<NPC1>().UpdateCurrentGoal();
                }
            }
        }
    }
}
