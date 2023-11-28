using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.TextCore.Text;
using UnityEngine.XR.Interaction.Toolkit;

public class AudioManager : MonoBehaviour
{
    // Flag to track if a sound is currently being played
    public bool isPlayingSound = false;

    // Reference to the AudioSource component
    public AudioSource AudioSource;

    // Array of audio clips for different objects
    public AudioClip[] AudioClips;

    void Start()
    {
        // Initialization code (if any) can go here
    }

    void Update()
    {
        // Update logic (if any) can go here
    }

    // Start playing a sound coroutine based on sound name
    public void StartSoundCoroutine(string soundName)
    {
        // If a sound is already playing, return
        if (this.isPlayingSound)
        {
            return;
        }

        // Start the coroutine to play the sound
        StartCoroutine(PlaySound(soundName));
    }

    // Coroutine to play a sound clip and manage isPlayingSound flag
    private IEnumerator PlaySound(string soundName)
    {
        // Get the index of the sound clip based on the sound name
        int clipIndex = GetIndexFromString(soundName);

        // If the index is 0, return immediately
        if (clipIndex == 0)
        {
            yield break;
        }


        // Set the flag to indicate a sound is playing
        this.isPlayingSound = true;

        // Get the audio clip and play it
        AudioClip clip = this.AudioClips[clipIndex];
        this.AudioSource.PlayOneShot(clip);

        // Wait for the clip to finish playing
        yield return new WaitForSeconds(clip.length);

        // Reset the flag after the sound has finished playing
        this.isPlayingSound = false;
    }

    // Map sound name to audio clip index
    private int GetIndexFromString(string soundName)
    {
        // Convert the sound name to lowercase for case-insensitive comparison
        string CleanSoundName = soundName.ToLower();

        // Map clean sound names to their corresponding audio clip indices

        if (CleanSoundName == "barrel")
        {
            return 1;
        }
        if (CleanSoundName == "bathtub")
        {
            return 2;
        }
        if (CleanSoundName == "bench")
        {
            return 3;
        }

        if (CleanSoundName == "bird house")
        {
            return 4;
        }


        if (CleanSoundName == "blue flower")
        {
            return 5;
        }


        if (CleanSoundName == "boulder")
        {
            return 6;
        }


        if (CleanSoundName == "bowl")
        {
            return 7;
        }


        if (CleanSoundName == "box")
        {
            return 8;
        }


        if (CleanSoundName == "bush")
        {
            return 9;
        }


        if (CleanSoundName == "chair")
        {
            return 10;
        }


        if (CleanSoundName == "clothes hanger")
        {
            return 11;
        }


        if (CleanSoundName == "grass")
        {
            return 12;
        }


        if (CleanSoundName == "green plant")
        {
            return 13;
        }


        if (CleanSoundName == "jug")
        {
            return 14;
        }


        if (CleanSoundName == "ladder")
        {
            return 15;
        }


        if (CleanSoundName == "larder")
        {
            return 16;
        }


        if (CleanSoundName == "logs")
        {
            return 17;
        }


        if (CleanSoundName == "package")
        {
            return 18;
        }


        if (CleanSoundName == "rake")
        {
            return 19;
        }


        if (CleanSoundName == "red flower")
        {
            return 20;
        }


        if (CleanSoundName == "rock")
        {
            return 21;
        }


        if (CleanSoundName == "rocks")
        {
            return 22;
        }


        if (CleanSoundName == "shelter")
        {
            return 23;
        }


        if (CleanSoundName == "table")
        {
            return 24;
        }


        if (CleanSoundName == "the mayor's house")
        {
            return 25;
        }


        if (CleanSoundName == "town hall")
        {
            return 26;
        }


        if (CleanSoundName == "town shop")
        {
            return 27;
        }


        if (CleanSoundName == "tree")
        {
            return 28;
        }


        if (CleanSoundName == "trough")
        {
            return 29;
        }


        if (CleanSoundName == "unopened bag")
        {
            return 30;
        }


        if (CleanSoundName == "vase")
        {
            return 31;
        }


        if (CleanSoundName == "wagon")
        {
            return 32;
        }


        if (CleanSoundName == "well")
        {
            return 33;
        }


        if (CleanSoundName == "wheat sack")
        {
            return 34;
        }



        if (CleanSoundName == "yellow flower")
        {
            return 35;
        }

        if (CleanSoundName == "black flower")
        {
            return 36;
        }

        if (CleanSoundName == "fence")
        {
            return 37;
        }

        if (CleanSoundName == "gate")
        {
            return 38;
        }

        if (CleanSoundName == "light blue flower")
        {
            return 39;
        }

        if (CleanSoundName == "orange flower")
        {
            return 40;
        }

        if (CleanSoundName == "purple flower")
        {
            return 41;
        }

        // If no match is found, return the default sound index 0
        return 0;
    }
}