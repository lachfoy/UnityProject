using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRadius = 10.0f;
    public int numberOfEnemies = 10;
    public float spawnFrequency = 5.0f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        for (; ; )
        {
            // spawn 10 enemies randomly in the radius
            for (var i = 0; i < numberOfEnemies; i++)
            {
                Vector2 spawnPoint = Random.insideUnitCircle * spawnRadius;
                spawnPoint += new Vector2(transform.position.x, transform.position.z); // use the world position as an offset
                EnemyManager.Instance.AddNewRandomEnemy(spawnPoint);
            }
            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    private void OnSceneGUI()
    {
        var t = target as EnemySpawner;
        Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.3f);
        Handles.DrawSolidDisc(t.transform.position, Vector3.up, t.spawnRadius);
    }
}
#endif