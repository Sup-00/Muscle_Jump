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

    private float _distanceDeltaZ;
    private float _distanceDeltaX;
    private string RUN_ANIMATION = "Run";
    private string PULL_ANIMATION = "Pull";
    private float _moveSpeed = 1f;
    private bool _isDead = false;
    private bool _isPulling = false;
    private Material _deadMaterial;
    private Collider[] _colliders;
    private Rigidbody[] _rigidbodies;
    private Vector3 _offset;
    private bool _destroySelf = false;

    private void Start()
    {
        _offset = new Vector3(Random.Range(-0.9f, 0.9f), 0, Random.Range(-0.9f, 0f));
        _colliders = GetComponentsInChildren<Collider>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();

        SetRigidebodies(true);
        SetColliders(true);


        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _charectorMoving = FindObjectOfType<CharectorMoving>();
    }

    private void Update()
    {
        if (_destroySelf == false)
        {
            if (transform.position.y <= -10f)
            {
                Destroy(gameObject);
            }

            if (_isDead != true)
            {
                _distanceDeltaZ = transform.position.z - _charectorMoving.transform.position.z;
                _distanceDeltaX = transform.position.x - _charectorMoving.transform.position.x;

                if (_distanceDeltaZ <= 6f && _distanceDeltaX <= 6f)
                {
                    if (_distanceDeltaZ <= 0.5f && _distanceDeltaX <= 0.5f && _charectorMoving.IsGrounded)
                    {
                        _animator.SetTrigger(PULL_ANIMATION);
                        PullPlayer();
                    }

                    FollowPlayer();
                }
            }
        }
    }

    public void Init(Material material)
    {
        _deadMaterial = material;
    }

    public void DestroySelf()
    {
        _destroySelf = true;
        if (_isDead == false)
        {
            _charectorMoving.NormolizeMoving();
            _charectorMoving.RiseCharector();
            _meshRenderer.material = _deadMaterial;
            _animator.enabled = false;
            transform.DOMoveY(transform.position.y + 5f, 1f);
            SetRigidebodies(false);
            SetColliders(false);
            _isDead = true;
        }
    }

    private void PullPlayer()
    {
        transform.GetComponent<CapsuleCollider>().isTrigger = false;
        if (_distanceDeltaZ < 0.5f && _distanceDeltaX < 0.5f)
        {
            if (_isPulling == false)
            {
                _charectorMoving.SlowMoving();
                _isPulling = true;
            }

            Vector3 target = _charectorMoving.transform.position + _offset;

            transform.DOMove(target, 0.2f);
        }

        transform.LookAt(_charectorMoving.transform);
    }

    private void FollowPlayer()
    {
        _animator.SetTrigger(RUN_ANIMATION);

        Vector3 target = new Vector3(_charectorMoving.transform.position.x, transform.position.y,
            _charectorMoving.transform.position.z);
        transform.LookAt(target);

        transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<JumpTrigger>())
        {
            Destroy(gameObject);
        }

        if (other.transform.GetComponent<Ring>())
        {
            DestroySelf();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<CharectorMoving>())
        {
            foreach (var colider in _colliders)
            {
                Collider charectorCollider = other.transform.GetComponent<CapsuleCollider>();
                Physics.IgnoreCollision(charectorCollider, colider);
            }
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

        transform.GetComponent<CapsuleCollider>().isTrigger = true;
    }
}