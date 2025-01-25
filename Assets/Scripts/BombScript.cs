using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public GameObject bombExplosion;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click (0)
        {
            Instantiate(bombExplosion, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
