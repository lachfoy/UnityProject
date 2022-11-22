using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public GameObject magicProjectilePrefab;
    public GameObject fireProjectilePrefab;
    public GameObject player;
    public List<GameObject> projectiles;

    public void CreateNewMagicProjectile(Vector3 position)
    {
        // why am i doing this here ..
        // choose the closest enemy in range to shoot at
        List<GameObject> enemies = EnemyManager.Instance.GetEnemiesInPlayerRange();
        if (enemies.Count > 0)
        {
            enemies.Sort((a, b) => ((a.transform.position - player.transform.position).magnitude.CompareTo((b.transform.position - player.transform.position).magnitude)));

            GameObject projectile = Instantiate(magicProjectilePrefab, new Vector3(position.x, 0.5f, position.z), Quaternion.identity);
            Vector3 dir = Vector3.Normalize(enemies[0].transform.position - player.transform.position);

            // ignore collision between the player and bullet
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), player.GetComponent<Collider>());
            projectile.GetComponent<Projectile>().direction = new Vector2(dir.x, dir.z);
            projectile.GetComponent<Projectile>().minDamage = 25;
            projectile.GetComponent<Projectile>().maxDamage = 35;
            projectiles.Add(projectile);
        }
    }

    public void CreateNewFireProjectile(Vector3 position)
    {
        // choose a random enemy in range to shoot at
        List<GameObject> enemies = EnemyManager.Instance.GetEnemiesInPlayerRange();
        if (enemies.Count > 0)
        {
            GameObject enemy = enemies[Random.Range(0, enemies.Count)];

            GameObject projectile = Instantiate(fireProjectilePrefab, new Vector3(position.x, 0.5f, position.z), Quaternion.identity);
            Vector3 dir = Vector3.Normalize(enemy.transform.position - player.transform.position);

            // ignore collision between the player and bullet
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), player.GetComponent<Collider>());
            projectile.GetComponent<Projectile>().direction = new Vector2(dir.x, dir.z);
            projectile.GetComponent<Projectile>().minDamage = 30;
            projectile.GetComponent<Projectile>().maxDamage = 65;
            projectiles.Add(projectile);
        }
    }

    public void RemoveProjectile(GameObject projectile)
    {
        projectiles.Remove(projectile);
        Destroy(projectile);
    }

    // singleton
    private static ProjectileManager _instance;

    public static ProjectileManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ProjectileManager();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
}
