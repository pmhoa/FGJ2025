using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("NikenPlayGround");
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
