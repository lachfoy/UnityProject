using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PropSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 _spawnExtents;
    [SerializeField] private int _numberOfProps = 50;
    [SerializeField] private List<GameObject> _propPrefabs;

    void Start()
    {
        for (int i = 0; i < _numberOfProps; i++)
        {
            Vector2 spawnPoint = new Vector2(Random.Range(-_spawnExtents.x, _spawnExtents.x), Random.Range(-_spawnExtents.y, _spawnExtents.y));
            int index = Random.Range(0, _propPrefabs.Count);
            Instantiate(_propPrefabs[index], new Vector3(spawnPoint.x, _propPrefabs[index].transform.localScale.y * 0.5f, spawnPoint.y), Quaternion.identity);
        }
    }
}