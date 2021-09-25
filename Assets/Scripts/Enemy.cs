using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;

    private CharectorMoving _charectorMoving;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    private float _deltaBetweenPlayerAndEnemy;
    private string RUN_ANIMATION = "Run";
    private string PULL_ANIMATION = "Pull";
    private float _moveSpeed = 1f;
    private bool _isDead = false;
    private Material _deadMaterial;
    private Collider[] _colliders;
    private Rigidbody[] _rigidbodies;

    private void Start()
    {
        _colliders = GetComponentsInChildren<Collider>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();

        SetRigidebodies(true);
        SetColliders(true);

        _capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _charectorMoving = FindObjectOfType<CharectorMoving>();
    }

    private void Update()
    {
        if (transform.position.y <= -10f)
        {
            Destroy(gameObject);
        }

        if (_isDead != true)
        {
            _deltaBetweenPlayerAndEnemy = transform.position.z - _charectorMoving.transform.position.z;
            if (_deltaBetweenPlayerAndEnemy <= 4f)
            {
                FollowPlayer();
            }
        }
    }

    public void Init(Material material)
    {
        _deadMaterial = material;
    }

    public void DestroySelf()
    {
        _isDead = true;
        _meshRenderer.material = _deadMaterial;
        _animator.enabled = false;
        Vector3 forceDirection = new Vector3(0, 3, Random.Range(-3f, 3f));
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.AddForce(forceDirection * 100f);
        }

        SetRigidebodies(false);
    }

    private void FollowPlayer()
    {
        _animator.SetTrigger(RUN_ANIMATION);

        Vector3 target = new Vector3(_charectorMoving.transform.position.x, transform.position.y,
            _charectorMoving.transform.position.z);
        transform.LookAt(target);

        transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);
    }

    private void Pull()
    {
        _animator.SetTrigger(PULL_ANIMATION);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Ring>())
        {
            DestroySelf();
        }

        if (other.GetComponent<JumpTrigger>())
        {
            _moveSpeed = 0f;
        }
    }

    private void SetRigidebodies(bool state)
    {
        foreach (var rigidbodie in _rigidbodies)
        {
            rigidbodie.isKinematic = state;
        }
    }

    private void SetColliders(bool state)
    {
        foreach (var collider in _colliders)
        {
            collider.isTrigger = state;
        }
    }
}