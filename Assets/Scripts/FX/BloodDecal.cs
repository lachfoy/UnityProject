using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDecal : MonoBehaviour
{
    public float fadeSpeed = 0.3f;
    void Update()
    {
        Color color = GetComponent<Renderer>().material.color;
        float fadeAmount = color.a - (fadeSpeed * Time.deltaTime);
        if (fadeAmount <= 0.0f)
        {
            Destroy(gameObject);
        }
        GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, fadeAmount);
    }
}
