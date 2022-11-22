using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthNumber : MonoBehaviour
{
    public float speed = 0.1f;
    public float fadeSpeed = 1.0f;
    private RectTransform _rectTransform;
    public Vector3 worldPosition;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
       // Vector3 offset = _camera.WorldToScreenPoint(_camera.transform.position - _originalPosition);

        //_rectTransform.position = _camera.WorldToScreenPoint(_rectTransform.position);

        Color color = GetComponent<Text>().color;
        float fadeAmount = color.a - (fadeSpeed * Time.deltaTime);
        if (fadeAmount <= 0.0f)
        {
            Destroy(gameObject);
        }
        GetComponent<Text>().color = new Color(color.r, color.g, color.b, fadeAmount);

        worldPosition = new Vector3(worldPosition.x, worldPosition.y += speed * Time.deltaTime, worldPosition.z);
        _rectTransform.position = Camera.main.WorldToScreenPoint(worldPosition);
    }

    private void LateUpdate()
    {
        _rectTransform.position = Camera.main.WorldToScreenPoint(worldPosition);
        //_rectTransform.Translate(Camera.main.ScreenToWorldPoint(_rectTransform.position));
    }
}
