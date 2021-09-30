using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class CharectorMoving : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _target;
    [SerializeField] private UnityEvent _run;
    [SerializeField] private UnityEvent _idle;
    [SerializeField] private UnityEvent _jump;
    [SerializeField] private UnityEvent _grounded;

    private bool _onJumpTrigger = false;
    private float _lastFrameTargetPosition;
    private bool _isGrounded;
    private Animator _animator;
    private string GROUDED_TRIGGER = "Grounded";
    private float _startMoveSpeed;

    public bool IsGrounded => _isGrounded;

    private void Start()
    {
        _startMoveSpeed = _moveSpeed;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_onJumpTrigger)
        {
            _jump.Invoke();
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            _run?.Invoke();
            _lastFrameTargetPosition = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            MoveTargetPoint(_lastFrameTargetPosition);
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position,
                _moveSpeed * Time.deltaTime);
            _lastFrameTargetPosition = Input.mousePosition.x;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _idle?.Invoke();
            _lastFrameTargetPosition = 0f;
        }
    }

    private void MoveTargetPoint(float lastPosition)
    {
        float targetPosition = Input.mousePosition.x - lastPosition;
        Vector3 direction = new Vector3(targetPosition, 0, 1);

        _target.transform.Translate(direction * Time.deltaTime * _moveSpeed);
        RotateCharector(_target.transform.position);
    }

    private void RotateCharector(Vector3 direction)
    {
        Vector3 lookDirection = direction + transform.position;
        transform.LookAt(new Vector3(lookDirection.x, transform.position.y, lookDirection.z));
    }

    private void Jump()
    {
        _moveSpeed = _startMoveSpeed;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 1000);
        _onJumpTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<JumpTrigger>())
        {
            _onJumpTrigger = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<Enemy>())
        {
            Debug.Log("ignore");
            Collider enemyCollider = other.transform.GetComponent<CapsuleCollider>();
            Physics.IgnoreCollision(enemyCollider, transform.GetComponent<CapsuleCollider>());
        }

        if (other.transform.GetComponent<Ground>())
        {
            _isGrounded = true;
            _animator.SetBool(GROUDED_TRIGGER, _isGrounded);
            _grounded.Invoke();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.GetComponent<Ground>())
        {
            _isGrounded = false;
            _animator.SetBool(GROUDED_TRIGGER, _isGrounded);
        }
    }

    public void SlowMoving()
    {
        float slowingForce = 0.2f;
        _moveSpeed -= slowingForce;
    }
}