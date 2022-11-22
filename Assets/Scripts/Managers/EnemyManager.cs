using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private List<GameObject> _enemies;
    private Transform _playerTransform;

    public List<GameObject> Enemies { get; }

    private void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;
    }

    public List<GameObject> GetEnemiesInPlayerRange()
    {
        List<GameObject> enemiesInRange = new List<GameObject>();
        foreach (GameObject enemy in _enemies)
        {
            if ((enemy.transform.position - _playerTransform.position).magnitude < _playerTransform.GetComponent<Player>().attackRange)
            {
                enemiesInRange.Add(enemy);
            }

        }
        return enemiesInRange;
    }

    // MAKE ADDING NEW ENEMIES GENERIC!!! PLES
    public void AddNewRandomEnemy(Vector2 position)
    {
        if (_playerTransform)
        {
            int enemyChoice = Random.Range(0, _enemyPrefabs.Count);
            GameObject enemy = Instantiate(_enemyPrefabs[enemyChoice], new Vector3(position.x, _enemyPrefabs[enemyChoice].transform.localScale.y * 0.5f, position.y), Quaternion.identity);
            _enemies.Add(enemy);
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
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
