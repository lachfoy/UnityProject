using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemSpawner : MonoBehaviour
{
    public float spawnRadius = 10f;
    public int numberOfItems = 10;
    public float spawnFrequency = 10f;

    void Start()
    {
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        for (;;)
        { 
            // spawn numberOfItems items randomly in the radius
            for (var i = 0; i < numberOfItems; i++)
            {
                Vector2 spawnPoint = Random.insideUnitCircle * spawnRadius;
                spawnPoint += new Vector2(transform.position.x, transform.position.z); // use the world position of the spawner as an offset
                ItemManager.Instance.CreateNewHealthItem(spawnPoint);
            }
            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ItemSpawner))]
public class ItemSpawnerEditor : Editor
{
    private void OnSceneGUI()
    {
        var t = target as ItemSpawner;
        Handles.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);
        Handles.DrawSolidDisc(t.transform.position, Vector3.up, t.spawnRadius);
    }
}
#endif