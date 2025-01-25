using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float growthSpeed = 1f; // How fast the object grows
    public float maxScale = 3f;    // The maximum scale size before destruction
    public float destroyDelay = 0.5f; // How long to wait before destroying the object

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale of the object
        StartCoroutine(ExpandAndDestroy());
        StartCoroutine(Destroy());
    }

    private IEnumerator ExpandAndDestroy()
    {
        float elapsedTime = 0f;

        // Gradually increase the size of the object
        while (elapsedTime < maxScale / growthSpeed)
        {
            transform.localScale = Vector3.Lerp(originalScale, originalScale * maxScale, elapsedTime * growthSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale * maxScale; // Ensure it reaches the max size

        // Wait for the specified delay before destroying the object
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject); // Destroy the object
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
