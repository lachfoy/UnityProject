using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // stats
    [SerializeField] private int _health = 100;
    [SerializeField] private int _triggerDamage = 20;
    public int TriggerDamage => _triggerDamage;
    [SerializeField] private float _playerCheckCooldown = 0.2f;

    // movement/physics
    [SerializeField] private float _speed = 10f;
    private Rigidbody _rb;
    private Vector3 _direction;
    
    // pushing
    [SerializeField] private float _pushAmount = 10f;
    [SerializeField] private float _pushDuration = 0.2f;
    private bool _beingPushed = false;

    // hurt
    [SerializeField] private float _hurtTime = 0.2f;
    [SerializeField] private Color _hurtColor = new Color(1f, 0.5f, 0.5f);
    [SerializeField] private GameObject _deathFXPrefab;
    private float _hurtCurrentTime = 0f;
    private bool _isBeingHurt = false;

    // sprite walking "animation"
    [SerializeField] private bool _spriteRotation = true;
    [SerializeField] private float _rotationFreq = 2f;
    [SerializeField] private float _rotationAmp = 2f;

    // sound
    [SerializeField] private AudioClip _hurtSound;
    [SerializeField] private AudioClip _deathSound;
    private AudioSource _audioSource;

    // misc
    [SerializeField] private bool _selfSpawning = false;
    [SerializeField] private bool _flipSprite = false;
    private Vector3 _scale;
    private Transform _playerTransform;

    void Start()
    {
        // get everything that needs to be initialized
        _playerTransform = GameObject.Find("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _scale = transform.localScale;
        _audioSource = GetComponent<AudioSource>();

        // ignore regular collisions with player (trigger messages are still sent)
        Physics.IgnoreCollision(_playerTransform.GetComponent<Collider>(), GetComponent<Collider>());
        
        // add self to enemy manager
        if (_selfSpawning)
        {
            EnemyManager.Instance.AddEnemy(gameObject);
        }

        StartCoroutine(CheckForPlayer()); // start a loop that sets the direction to move towards player every "playerCheckCooldown"
    }

    private void FixedUpdate()
    {
        // Movement -- enemies use RB movement
        _rb.velocity = _beingPushed ? _direction * _speed * -_pushAmount * Time.fixedDeltaTime : _direction * _speed * Time.fixedDeltaTime;
    }

    void Update()
    {
        if (_isBeingHurt)
        {
            if (_hurtCurrentTime >= _hurtTime)
            {
                _isBeingHurt = false;
                _hurtCurrentTime = 0.0f;
            }
            else
            {
                _hurtCurrentTime += Time.deltaTime;
            }
        }

        GetComponent<Renderer>().material.color = _isBeingHurt ? _hurtColor : Color.white;

        // flip sprite based on velocity
        if (_direction.x > 0.0f)
        {
            transform.localScale = _flipSprite ? new Vector3(-_scale.x, _scale.y, _scale.z) : new Vector3(_scale.x, _scale.y, _scale.z);
        }
        else if (_direction.x < 0.0f)
        {
            transform.localScale = _flipSprite ? new Vector3(_scale.x, _scale.y, _scale.z) : new Vector3(-_scale.x, _scale.y, _scale.z);
        }

        // oscillate rotation
        if (_spriteRotation)
        {
            float rotationOffset = (0.0f + Mathf.Sin(Time.fixedTime * Mathf.PI * _rotationFreq) * _rotationAmp) - _rotationAmp * 0.5f;
            //transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotationOffset);

            //transform.Rotate(new Vector3(0.0f, 0.0f, Mathf.PingPong(Time.fixedTime * rotationSpeedModifier, rotationAmount * 2) - rotationAmount));
        }
    }

    private void OnTakeDamage(object args)
    {
        Debug.Log("Enemy took damage");
        _isBeingHurt = true;

        StartCoroutine(Push());

        int amount = (int)((object[])args)[0];
        bool isCrit = (bool)((object[])args)[1];
        if (_health - amount <= 0)
        {
            // add damage number
            UiManager.Instance.AddDamageNumber(transform.position, amount, isCrit);

            // spawn a death effect
            if (_deathFXPrefab)
            {
                GameObject deathFX = Instantiate(_deathFXPrefab, transform.position, Quaternion.identity);
                deathFX.GetComponent<AudioSource>().PlayOneShot(_deathSound); // offload the deathsound to the deathfx prefab... kinda a hack but works
                deathFX.GetComponent<ParticleSystem>().Play();
            }

            // remove self
            EnemyManager.Instance.RemoveEnemy(gameObject);
        }
        else
        {
            if (_hurtSound)
            {
                _audioSource.PlayOneShot(_hurtSound);
            }
            
            // add damage number
            UiManager.Instance.AddDamageNumber(transform.position, amount, isCrit);
            _health -= amount;
        }
    }

    Vector3 GetDirectionToPlayer()
    {
        if (_playerTransform)
        {
            Vector3 direction = Vector3.Normalize(_playerTransform.position - transform.position);
            direction.y = 0.0f; // zero out the y
            return direction;
        }
        return Vector3.zero;
    }

    IEnumerator Push()
    {
        if (!_beingPushed)
        {
            _direction = GetDirectionToPlayer(); // update this
            _beingPushed = true;
            yield return new WaitForSeconds(_pushDuration);
        }

        _beingPushed = false;
        yield return null;
    }

    IEnumerator CheckForPlayer()
    {
        for (;;)
        {
            _direction = GetDirectionToPlayer();
            yield return new WaitForSeconds(_playerCheckCooldown);
        }
    }
}
