using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWand : Weapon
{
    [SerializeField] private int _numberOfShots = 3;

    protected override void SetWeaponStats()
    {
        _amount = 3;
        _weaponCooldown = 1.3f;
        _weaponPerShotCooldown = 0.4f;
    }

    protected override void WeaponShoot()
    {
        for (int i = 0; i < _numberOfShots; i++)
        {
            ProjectileManager.Instance.CreateNewFireProjectile(transform.position);
        }
    }
}
