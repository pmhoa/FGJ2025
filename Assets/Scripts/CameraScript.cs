using UnityEngine;

public class Follow_player : MonoBehaviour
{

    public Transform player;
    public float x = 0;
    public float y = 5;
    public float z = -10;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(x, y, z);
    }
}