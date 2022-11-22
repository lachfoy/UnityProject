using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10.0f;
    public Vector2 direction;
    public int minDamage;
    public int maxDamage;
    private int _damage;
    public GameObject hitFXPrefab;
    public AudioClip hitSound;

    private void Start()
    {
        _damage = Random.Range(minDamage, maxDamage);
    }

    // Update is called once per frame
    void Update()
    {
        // Movement -- projectiles use kinematic movement
        // move along the direction vector
        transform.rotation = Quaternion.identity;
        Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);
        transform.Translate(movement * speed * Time.deltaTime);
        //GetComponent<Rigidbody>().velocity = movement*200 * speed * Time.deltaTime;

        // look at the camera always
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
    }

    private void OnCollisionEnter(Collision other)
    {
        // remove the projectile and tell the game object to take damage
        //ProjectileManager.Instance.RemoveProjectile(gameObject);
       
        switch (other.gameObject.tag)
        {
        case "Enemy":
            ProjectileManager.Instance.RemoveProjectile(gameObject);
            bool isCrit = Random.Range(0, 2) != 0;
            object[] args = new object[2];
            args[0] = isCrit ? _damage * 2 : _damage;
            args[1] = isCrit;
            other.gameObject.SendMessage("OnTakeDamage", args);
            break;
        case "Wall":
            ProjectileManager.Instance.RemoveProjectile(gameObject);
            Debug.Log("Projectile Hit wall");
            break;
        }


        if (hitFXPrefab)
        {
            GameObject hitFX = Instantiate(hitFXPrefab, other.transform.position, Quaternion.identity);
            hitFX.GetComponent<AudioSource>().PlayOneShot(hitSound); // offload the deathsound to the deathfx prefab... kinda a hack but works
        }
    }
}
