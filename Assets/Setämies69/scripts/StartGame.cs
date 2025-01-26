using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void OnButtonClick()
    {
        // Load the next scene
        SceneManager.LoadScene(2);
    }
}
