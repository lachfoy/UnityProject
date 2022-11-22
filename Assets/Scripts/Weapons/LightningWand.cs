using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWand : Weapon
{
    public int damagePerHit = 20;
    public int numberOfArcs = 10;
    public GameObject lightningArcPrefab;
    public GameObject lightningHitPrefab;

    //private void Start()
    //{
    //    lightningArcPrefab = GameObject.Find("LightningArc");
    //    lightningHitPrefab = GameObject.Find("LightningHit");
    //}

    protected override void SetWeaponStats()
    {
        _amount = 1;
        _weaponCooldown = 0.6f;
        _weaponPerShotCooldown = 0.1f;
    }

    protected override void WeaponShoot()
    {
        // choose a random enemy in range to shoot at
        List<GameObject> enemies = EnemyManager.Instance.GetEnemiesInPlayerRange();
        if (enemies.Count > 0)
        {
            // if there are less enemies than the number of arcs, only arc to the enemies that exist
            int iterations = enemies.Count < numberOfArcs ? enemies.Count : numberOfArcs;

            // positions for arc fx
            Vector3[] arcPositions = new Vector3[iterations + 1];
            arcPositions[0] = transform.position; // first position is the "shooter"

            for (int i = 0; i < iterations; i++)
            {
                GameObject enemy = enemies[Random.Range(0, enemies.Count)]; // the enemy im gonna shoot at
                
                // deal damage to the enemy
                bool isCrit = Random.Range(0, 2) != 0;
                object[] args = new object[2];
                args[0] = isCrit ? damagePerHit * 2 : damagePerHit;
                args[1] = isCrit;
                enemy.gameObject.SendMessage("OnTakeDamage", args);

                arcPositions[i + 1] = enemy.transform.position;
                
                // add light fx
                Instantiate(lightningHitPrefab, enemy.transform.position, Quaternion.identity);

                // remove that enemy from the list (stops lightning from arcing to the same enemy twice)
                enemies.Remove(enemy);
            }

            // add the arc fx
            LineRenderer lr = Instantiate(lightningArcPrefab).GetComponent<LineRenderer>();
            lr.positionCount = arcPositions.Length;
            lr.SetPositions(arcPositions);
        }
    }
}
