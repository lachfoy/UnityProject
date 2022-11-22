using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : Weapon
{
    protected override void SetWeaponStats()
    {
        _amount = 3;
        _weaponCooldown = 0.6f;
        _weaponPerShotCooldown = 0.1f;
    }

    protected override void WeaponShoot()
    {
        ProjectileManager.Instance.CreateNewMagicProjectile(transform.position);
    }
}
