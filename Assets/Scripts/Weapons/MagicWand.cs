using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : Weapon
{
    protected override void SetWeaponStats()
    {
        _amount = 6;
        _weaponCooldown = 0.5f;
        _weaponPerShotCooldown = 0.05f;
    }

    protected override void WeaponShoot()
    {
        ProjectileManager.Instance.CreateNewMagicProjectile(transform.position);
    }
}
