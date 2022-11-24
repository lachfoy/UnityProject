using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private int _maxEnemies = 500;
    private Transform _playerTransform;
    private int _enemiesKilled = 0;
    public int EnemiesKilled => _enemiesKilled;
    

    private void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;
    }

    public List<GameObject> GetEnemiesInRange(Vector3 position, float range)
    {
        List<GameObject> enemiesInRange = new List<GameObject>();
        foreach (GameObject enemy in _enemies)
        {
            if ((enemy.transform.position - position).magnitude < range)
            {
                enemiesInRange.Add(enemy);
            }

        }

        return enemiesInRange;
    }

    public List<GameObject> GetEnemiesInPlayerRange()
    {
        return GetEnemiesInRange(_playerTransform.position, _playerTransform.GetComponent<Player>().attackRange);
    }

    // MAKE ADDING NEW ENEMIES GENERIC!!! PLES
    public void AddNewRandomEnemy(Vector2 position)
    {
        if (_playerTransform && _enemies.Count < _maxEnemies)
        {
            int enemyChoice = Random.Range(0, _enemyPrefabs.Count);
            GameObject enemy = Instantiate(_enemyPrefabs[enemyChoice], new Vector3(position.x, _enemyPrefabs[enemyChoice].transform.localScale.y * 0.5f, position.y), Quaternion.identity);
            _enemies.Add(enemy);
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        if (_playerTransform && _enemies.Count < _maxEnemies)
        {
            _enemies.Add(enemy);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
        _enemiesKilled++;
        Destroy(enemy);
    }

    // singleton
    private static EnemyManager _instance;

    public static EnemyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EnemyManager();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
}
