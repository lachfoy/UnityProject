using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform playerTransform;

    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 8f, playerTransform.position.z - 12f);
            //transform.rotation = Quaternion.LookRotation(playerTransform.forward, playerTransform.up);
            transform.LookAt(playerTransform);
        }

    }
}
