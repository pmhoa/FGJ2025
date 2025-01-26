using UnityEngine.SceneManagement;
using UnityEngine;

public class PressAnyButtonToReturn : MonoBehaviour
{
    private bool sceneLoaded = false; // Flag to check if the scene is currently loaded

    void Update()
    {
        // Check if any key is pressed
        if (Input.anyKey)
        {
            SceneManager.LoadScene(0);
        }
    }

}
