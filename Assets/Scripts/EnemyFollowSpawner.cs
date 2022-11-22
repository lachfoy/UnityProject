using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyFollowSpawner : MonoBehaviour
{
    public float spawnRectangle = 23f;
    public float playerSafeRectangle = 12f;
    public int numberOfEnemies = 10;
    public float spawnFrequency = 1.0f;
    private Transform _playerTransform;

    void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;

        StartCoroutine(SpawnEnemies());
    }

    Vector2 GetSpawnPoint()
    {
        Vector2 pointOnEdgeOfCircle = Random.insideUnitCircle.normalized * playerSafeRectangle;
        float extendSpawnPointAmount = Random.Range(0f, 2f);
        Vector2 spawnPoint = pointOnEdgeOfCircle;// * extendSpawnPointAmount;
        spawnPoint += new Vector2(_playerTransform.position.x, _playerTransform.position.z); // offset by the player's position

        bool yippee = false;
        for (int i = 0; i < 10; i++)
        {
            // check that the point is within bounds
            if (!(spawnPoint.x < -spawnRectangle || spawnPoint.x > spawnRectangle || spawnPoint.y < -spawnRectangle || spawnPoint.y > spawnRectangle))
            {
                yippee = true;
                break;
            }
        }

        if (!yippee)
        {
            throw new System.Exception("COULD NOT FIND A SUITABLE SPAWN POINT");
        }

        return spawnPoint;
    }

    IEnumerator SpawnEnemies()
    {
        for (; ; )
        {
            // spawn 10 enemies randomly in the radius
            for (var i = 0; i < numberOfEnemies; i++)
            {
                try
                {
                    Vector2 spawnPoint = GetSpawnPoint();
                    EnemyManager.Instance.AddNewRandomEnemy(spawnPoint);
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e, this);
                }
                
            }
            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyFollowSpawner))]
public class EnemyFollowSpawnerEditor : Editor
{
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmosSelected(EnemyFollowSpawner scr, GizmoType gizmoType)
    {
        //Gizmos.DrawCube
    }

    private void OnSceneGUI()
    {
        var t = target as EnemyFollowSpawner;

        // draw enemy spawn rectangle
        float spawnRectSize = t.spawnRectangle;
        Vector3[] spawnRectVerts = {
            new Vector3(-spawnRectSize, 0f, spawnRectSize),
            new Vector3(spawnRectSize, 0f, spawnRectSize),
            new Vector3(spawnRectSize, 0f, -spawnRectSize),
            new Vector3(-spawnRectSize, 0f, -spawnRectSize)
        };

        Color spawnRectFillColor = new Color(1.0f, 0.0f, 0.0f, 0.1f);
        Handles.DrawSolidRectangleWithOutline(spawnRectVerts, spawnRectFillColor, Color.red);

        // draw the safe rectangle
        float safeRectSize = t.playerSafeRectangle;
        Vector3[] safeRectVerts = {
            new Vector3(-safeRectSize, 0f, safeRectSize),
            new Vector3(safeRectSize, 0f, safeRectSize),
            new Vector3(safeRectSize, 0f, -safeRectSize),
            new Vector3(-safeRectSize, 0f, -safeRectSize)
        };

        Color safeRectFillColor = new Color(0.0f, 1.0f, 0.0f, 0.1f);
        Handles.DrawSolidRectangleWithOutline(safeRectVerts, safeRectFillColor, Color.green);
    }
}
#endif