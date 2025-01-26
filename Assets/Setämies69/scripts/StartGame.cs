using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    public void DestroyAudioSources()
    {
        // Find all AudioSource components in the scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Loop through all AudioSource components and destroy them
        foreach (AudioSource audioSource in audioSources)
        {
            Destroy(audioSource);
        }
    }

    public void OnButtonClick()
    {
        DestroyAudioSources();

        // Load the next scene
        SceneManager.LoadScene(2);
    }
}
