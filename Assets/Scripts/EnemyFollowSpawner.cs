using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyFollowSpawner : MonoBehaviour
{
    [SerializeField] private float _enemySpawnExtents = 23f;
    public float EnemySpawnExtents => _enemySpawnExtents;
    [SerializeField] private Vector2 _playerSafeExtents = new Vector2(9f, 9f);
    public Vector2 PlayerSafeExtents => new Vector2(_playerSafeExtents.x + _playerTransform.position.x, _playerSafeExtents.y + _playerTransform.position.z);

    [SerializeField] private int _numberOfEnemies = 10;
    [SerializeField] private float _spawnFrequency = 1.0f;
    private Transform _playerTransform;
    [SerializeField] private int _potentialSpawnBufferSize = 20;
    private List<Vector2> _potentialSpawnLocations;
    [SerializeField] private float _potentialSpawnRefreshFrequency = 1.0f;

    void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;
        _potentialSpawnLocations = new List<Vector2>();
        StartCoroutine(GeneratePotentialSpawnLocations());
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator GeneratePotentialSpawnLocations()
    {
        for(;;)
        {
            // generate a list of viable spawns
            _potentialSpawnLocations.Clear();
            for (int i = 0; i < _potentialSpawnBufferSize; i++)
            {
                _potentialSpawnLocations.Add(new Vector2(Random.Range(-_enemySpawnExtents, _enemySpawnExtents), Random.Range(-_enemySpawnExtents, _enemySpawnExtents)));
            }

            // remove any spawn locations that intersect with the player safe zone
            _potentialSpawnLocations.RemoveAll(spawnLocation => (
                    spawnLocation.x > -_playerSafeExtents.x + _playerTransform.position.x && spawnLocation.x < _playerSafeExtents.x + _playerTransform.position.x &&
                    spawnLocation.y > -_playerSafeExtents.y + _playerTransform.position.z && spawnLocation.y < _playerSafeExtents.y + _playerTransform.position.z
                )
            );

            Debug.Assert(_potentialSpawnLocations.Count == 0, "Error: As unlikely as it is, every potential spawn was inside the player safe zone...");

            yield return new WaitForSeconds(_potentialSpawnRefreshFrequency);
        }
    }

    IEnumerator SpawnEnemies()
    {
        for (; ; )
        {
            // spawn 10 enemies randomly in the radius
            for (var i = 0; i < _numberOfEnemies; i++)
            {
                Vector2 spawnPoint = _potentialSpawnLocations[Random.Range(0, _potentialSpawnLocations.Count)];
                EnemyManager.Instance.AddNewRandomEnemy(spawnPoint);
            }
            yield return new WaitForSeconds(_spawnFrequency);
        }
    }
}