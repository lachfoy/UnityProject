using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject bloodDecalPrefab;
    private Quaternion _decalOrientation;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        _decalOrientation = Quaternion.LookRotation(Quaternion.Euler(90.0f, 0.0f, 0.0f) * Vector3.forward, Vector3.up);
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;
            Instantiate(bloodDecalPrefab, new Vector3(pos.x, 0.01f, pos.z), _decalOrientation);
            i++;
        }
    }
}
