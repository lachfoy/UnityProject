using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLightBasedOnRange : MonoBehaviour
{
    private Transform _playerTransform;
    public float disableRange = 10f;
    public float checkFrequency = 0.1f;
    private Light _light;

    void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;
        _light = GetComponent<Light>();
        StartCoroutine(LightDisable());
    }

    float GetDistanceToPlayer()
    {
        if (_playerTransform)
        {
            return Vector3.Magnitude(_playerTransform.position - transform.position);
        }
        return 0f;
    }

    IEnumerator LightDisable()
    {
        for(; ; )
        {
            if (GetDistanceToPlayer() > disableRange)
            {
                _light.enabled = false;
            }
            else
            {
                _light.enabled = true;
            }
            yield return new WaitForSeconds(checkFrequency);
        }
    }
}
