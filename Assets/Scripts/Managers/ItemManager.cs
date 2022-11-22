using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject _healthItemPrefab;
    [SerializeField] private List<GameObject> _items;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.Find("Player");
    }

    public void CreateNewHealthItem(Vector2 position)
    {
        if (_player)
        {
            GameObject item = Instantiate(_healthItemPrefab, new Vector3(position.x, 0.5f, position.y), Quaternion.identity);
            _items.Add(item);
        }
    }

    public void RemoveItem(GameObject item)
    {
        _items.Remove(item);
        Destroy(item);
    }

    // singleton pattern
    private static ItemManager _instance;

    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemManager();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
}
