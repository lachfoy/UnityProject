using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float floatFreq = 0.5f;
    public float floatAmp = 0.1f;
    private float _startY;
    public float startSpeed = 5.0f;
    private float _currentSpeed;
    private Transform _playerTransform;
    public bool isActive = false;
    public int healAmount = 20;

    void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;
        _startY = transform.position.y;
        _currentSpeed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerTransform) // if player exists
        {
            if ((_playerTransform.position - transform.position).magnitude <= _playerTransform.GetComponent<Player>().itemPickUpRange)
            {
                isActive = true;
            }

            if (isActive)
            {
                Vector3 direction = Vector3.Normalize(_playerTransform.position - transform.position);
                direction.y = 0.0f; // zero out the y
                transform.Translate(direction * _currentSpeed * Time.deltaTime);
                _currentSpeed += _currentSpeed * 2 * Time.deltaTime; // increase speed over time
            }
        }

        // floating up and down
        // it would be cooler to do this in a vertex shader but whatever
        float yOffset = (_startY + Mathf.Sin(Time.fixedTime * Mathf.PI * floatFreq) * floatAmp) - floatAmp * 0.5f;
        transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
    }

    public void ApplyItemEffect(GameObject player)
    {
        if (player)
        {
            player.GetComponent<Player>().Heal(20);
            ItemManager.Instance.RemoveItem(gameObject);
        }
    }
}
