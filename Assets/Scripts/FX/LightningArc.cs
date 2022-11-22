using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningArc : MonoBehaviour
{
    public float aliveTime = 0.3f;
    LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();

        StartCoroutine(animateLR());
        Destroy(gameObject, aliveTime);
    }

    private IEnumerator animateLR()
    {
        for(; ; )
        {
            Vector2 textureOffset = GetComponent<LineRenderer>().material.mainTextureOffset;
            textureOffset.x += Random.Range(0.1f, 0.9f);

            GetComponent<LineRenderer>().material.mainTextureOffset = textureOffset;

            yield return new WaitForSeconds(0.05f); // animate every .05 seconds
        }

    }

}
