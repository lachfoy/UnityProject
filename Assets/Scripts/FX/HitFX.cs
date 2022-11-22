using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFX : MonoBehaviour
{
    public float aliveTime = 0.3f;
    void Start()
    {
        Destroy(gameObject, aliveTime);
    }
}
