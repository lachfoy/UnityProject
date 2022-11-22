using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWand : Weapon
{
    protected override void SetWeaponStats()
    {
        _amount = 1;
        _weaponCooldown = 1.3f;
        _weaponPerShotCooldown = 0.1f;
    }

    protected override void WeaponShoot()
    {
        ProjectileManager.Instance.CreateNewFireProjectile(transform.position);
        ProjectileManager.Instance.CreateNewFireProjectile(transform.position);
        ProjectileManager.Instance.CreateNewFireProjectile(transform.position);
    }
}
