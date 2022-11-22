using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int _amount;
    [SerializeField] protected float _weaponCooldown;
    [SerializeField] protected float _weaponPerShotCooldown;
    [SerializeField] protected AudioClip _shootSound;

    private float _weaponCooldownCurrentTime;
    private int _currentAmount;
    private float _weaponPerShotCooldownCurrentTime;
    private AudioSource _audioSource;

    private void Start()
    {
        SetWeaponStats();
        _audioSource = GetComponent<AudioSource>();
        _weaponCooldownCurrentTime = _weaponCooldown;
        _weaponPerShotCooldownCurrentTime = _weaponPerShotCooldown;
    }

    private void Update()
    {
        // main cooldown
        if (_weaponCooldownCurrentTime >= _weaponCooldown)
        {
            _currentAmount = _amount; // fire burst equal to amount
            _weaponCooldownCurrentTime = 0.0f;  // reset the current time
        }
        else
        {
            _weaponCooldownCurrentTime += Time.deltaTime;
        }

        // fire the burst
        if (_currentAmount > 0)
        {
            // burst inner cooldown
            if (_weaponPerShotCooldownCurrentTime >= _weaponPerShotCooldown)
            {
                WeaponShoot();
                if (_shootSound) _audioSource.PlayOneShot(_shootSound, 0.5f);
                _currentAmount--;
                _weaponPerShotCooldownCurrentTime = 0.0f; // reset the current time
            }
            else
            {
                _weaponPerShotCooldownCurrentTime += Time.deltaTime;
            }
        }
    }

    protected abstract void SetWeaponStats();
    protected abstract void WeaponShoot();
}