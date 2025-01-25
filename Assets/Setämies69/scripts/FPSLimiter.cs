using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;
    void Start()
    {
        Application.targetFrameRate = targetFPS;
    }
}
