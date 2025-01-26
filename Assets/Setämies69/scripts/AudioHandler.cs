using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioHandler : MonoBehaviour
{
    private static AudioHandler instance; // Ensure only one instance persists

    void Awake()
    {
        // If there's already an instance of AudioManager, destroy this one
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, set this instance as the singleton
        instance = this;

        // Don't destroy the AudioSource when loading new scenes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Ensure the AudioSource is set to loop
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true; // Set to loop indefinitely
            audioSource.Play(); // Start playing the audio if it's not already playing
        }
    }
    // Attach the button's OnClick() event to this method
    public void OnButtonClick()
    {
        // Load the next scene
        SceneManager.LoadScene(1);
    }

}
