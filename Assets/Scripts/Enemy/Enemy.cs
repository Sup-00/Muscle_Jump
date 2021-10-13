using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private float _moveSpeed;

    private HingeJoint _hingeJoint;
    private Landing _landing;
    private Ring _ring;
    private CharectorMoving _charectorMoving;
    private CharectorChangeSize _charectorChangeSize;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private bool _isDead = false;
    private bool _isPulling = false;
    private Material _deadMaterial;
    private Collider[] _colliders;
    private Rigidbody[] _rigidbodies;
    private Vector3 _offset;
    private bool _destroySelf = false;
    private bool _isRingActive = false;
    private bool _isLooked = false;

    private string RUN_ANIMATION = "Run";
    private string PULL_ANIMATION = "Pull";

    private void Start()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        _offset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, -0.2f));
        _colliders = GetComponentsInChildren<Collider>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();

        SetRigidebodies(true);
        SetColliders(true);


        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_landing.IsRingActive == true)
        {
            _isRingActive = true;
        }

        if (_destroySelf == false)
        {
            if (transform.position.y <= -10f)
            {
                Destroy(gameObject);
            }

            if (_isDead != true)
            {
                if (Vector3.Distance(_ring.transform.position, transform.position) < 7f)
                {
                    if (Vector3.Distance(_ring.transform.position, transform.position) < 1f &&
                        _charectorMoving.IsGround)
                    {
                        if (_isPulling == false)
                        {
                            _charectorMoving.SlowMovingSpeed();
                            _isPulling = true;
                        }

                        _animator.SetTrigger(PULL_ANIMATION);
                        PullPlayer();
                    }
                    else
                    {
                        FollowPlayer();
                    }
                }
            }
        }
    }

    private void PullPlayer()
    {
        if (_charectorMoving.IsGround == false)
        {
            DestroySelf();
        }


        /*if (_isLooked == false)
        {
            SetRigidebodies(true);
            SetColliders(true);
            transform.SetParent(_charectorMoving.transform);
            _isLooked = true;
        }

        _hingeJoint.connectedBody = _charectorMoving.GetComponent<Rigidbody>();*/


        Vector3 target = _charectorMoving.transform.position + _offset;
        transform.position = target;

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
            DestroySelf();
        }

        if (other.GetComponent<Ring>())
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
            if (rigidbodie != null)
                rigidbodie.isKinematic = state;
        }
    }

    private void SetColliders(bool state)
    {
        foreach (var collider in _colliders)
        {
            if (collider != null)
                collider.isTrigger = state;
        }

        transform.GetComponent<CapsuleCollider>().isTrigger = true;
    }

    public void Init(Material material, CharectorChangeSize charectorChangeSize, CharectorMoving charectorMoving,
        Ring ring, Landing landing)
    {
        _landing = landing;
        _ring = ring;
        _charectorMoving = charectorMoving;
        _charectorChangeSize = charectorChangeSize;
        _deadMaterial = material;
    }

    public void DestroySelf()
    {
        _destroySelf = true;
        if (_isDead == false)
        {
            transform.SetParent(null);
            Destroy(_hingeJoint);
            _charectorMoving.NormolizeMovingSpeed();
            _charectorChangeSize.RiseCharector();
            _meshRenderer.material = _deadMaterial;
            _animator.enabled = false;
            transform.DOMoveY(transform.position.y + 5f, 2f);
            SetRigidebodies(false);
            SetColliders(false);
            Destroy(_colliders[0]);
            Destroy(_rigidbodies[0]);
            _isDead = true;
        }
    }
}