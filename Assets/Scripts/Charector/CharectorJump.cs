using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class CharectorJump : MonoBehaviour
{
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _startTimer;
    [SerializeField] private float _jumpHeight;

    private CharectorMoving _charector;
    private Landing _landing;
    private CameraMove _cameraMove;
    private CharectorMoving _charectorMoving;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private float _currentTimer;
    private Coroutine _currentCoroutine;
    private bool _isJumped = false;

    private string JUMP_TRIGGET = "Jump";

    private void Update()
    {
        if (_isJumped == true)
        {
            _currentTimer -= 1f * Time.deltaTime;

            if (_currentTimer <= 0)
            {
                _animator.SetBool(JUMP_TRIGGET, false);
                _rigidbody.isKinematic = false;
                _currentTimer = _startTimer;
                _isJumped = false;
            }
        }
    }

    private void Start()
    {
        _currentTimer = _startTimer;
        _charector = FindObjectOfType<CharectorMoving>();
        _landing = FindObjectOfType<Landing>();
        _charectorMoving = GetComponent<CharectorMoving>();
        _cameraMove = FindObjectOfType<CameraMove>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Jump()
    {
        _charectorMoving.ResetConnectionPoints();
        _animator.SetBool(JUMP_TRIGGET, true);
        _charectorMoving.SetStartMovingSpeed();
        _animator.speed = 1f;
        _rigidbody.isKinematic = true;
        transform.DOMoveY(transform.position.y + _jumpHeight, _jumpSpeed);
        _charector.IsGrounded(false);
        _landing.SetTrigger(true);
        _isJumped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<JumpTrigger>())
        {
            Jump();
        }
    }
}