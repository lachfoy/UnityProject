using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;
    public int health = 100;
    public int maxHealth = 100;
    private float movementX;
    private float movementY;

    public Healthbar healthbar;
    public GameObject deathFXPrefab;

    public float immuneTime = 0.1f;
    private float _immuneCurrentTime = 0.0f;
    private bool _isImmune = false;

    public bool spriteRotation = false;
    public float rotationAmount = 0.1f;
    public float rotationSpeedModifier = 0.3f;

    public float itemPickUpRange = 1.5f;
    public float attackRange = 10f;

    // sounds
    private AudioSource _audioSource;
    public AudioClip healSound;
    public AudioClip hurtSound;
    public AudioClip magicWandShootSound;
    public AudioClip fireWandShootSound;

    public GameObject lightningArcPrefab;
    public GameObject lightningHitPrefab;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // add weapons
        gameObject.AddComponent<MagicWand>();
        gameObject.AddComponent<FireWand>();
        LightningWand lw = gameObject.AddComponent<LightningWand>();
        lw.lightningArcPrefab = lightningArcPrefab; // AJHSDGKASJHDGAKSJDHGASJD
        lw.lightningHitPrefab = lightningHitPrefab; // AJHSDGKASJHDGAKSJDHGASJD
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        /// MOVEMENT
        //transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z); // make sure y is always .5
        //Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        //transform.Translate(movement * speed * Time.deltaTime);
    }

    void Update()
    {
        // Movement -- Player uses Kinematic movement
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z); // make sure y is always .5
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        transform.Translate(movement * speed * Time.deltaTime);

        // TIMERS
        if (_isImmune)
        {
            if (_immuneCurrentTime >= immuneTime)
            {
                _isImmune = false;
                _immuneCurrentTime = 0.0f;
            }
            else
            {
                _immuneCurrentTime += Time.deltaTime;
            }
        }

        // flip sprite based on velocity
        if (movementX > 0.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else if (movementX < 0.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        // oscillate rotation
        if (spriteRotation)
        {
            if (!(movementX == 0.0f && movementY == 0.0f))
            {
                transform.Rotate(new Vector3(0.0f, 0.0f, Mathf.PingPong(Time.time * rotationSpeedModifier, rotationAmount * 2) - rotationAmount));
            }
            else
            {
                transform.eulerAngles = Vector3.zero;
                Debug.Log("no move");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy":
                Debug.Log("Enemy trigger hit");
                TakeDamage(other.gameObject.GetComponent<Enemy>().TriggerDamage);
                break;
            case "Item":
                Debug.Log("Item trigger hit");
                other.gameObject.GetComponent<Item>().ApplyItemEffect(gameObject);
                break;
        }

        
    }

    public void TakeDamage(int amount)
    {
        if (!_isImmune)
        {
            if (health - amount <= 0)
            {
                DIE(); // KILL KILL KILL!!!
            }
            else
            {
                health -= amount;
                healthbar.UpdateHealth(health, maxHealth);
                _isImmune = true;
            }
        }
    }

    public void Heal(int amount)
    {
        if (health + amount >= maxHealth)
        {
            health = maxHealth;
            health -= amount;
        }
        else
        {
            health += amount;
        }

        _audioSource.PlayOneShot(healSound, 0.5f);
        UiManager.Instance.AddHealNumber(transform.position, amount);
        healthbar.UpdateHealth(health, maxHealth);
    }

    private void DIE()
    {
        // spawn a death effect
        if (deathFXPrefab)
        {
            GameObject deathFX = Instantiate(deathFXPrefab, transform.position, Quaternion.identity);
            deathFX.GetComponent<ParticleSystem>().Play();
        }

        UiManager.Instance.CallShowDeathStatusCoroutine(); // show the "you died" text
        Destroy(gameObject);
        Destroy(healthbar.gameObject);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    private void OnSceneGUI()
    {
        var t = target as Player;

        Color attackRangeColor = new Color(0f, 1f, 1f, 0.1f);
        Handles.color = attackRangeColor;
        Handles.DrawSolidDisc(t.transform.position, Vector3.up, t.attackRange);
    }
}
#endif