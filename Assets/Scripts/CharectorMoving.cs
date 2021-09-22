using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

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
    private bool _isGrounded = true;

    private void Update()
    {
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

        if (_onJumpTrigger && _isGrounded)
        {
            Jump();
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

    private void OnCollisionStay(Collision other)
    {
        _grounded?.Invoke();
        _isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        _jump?.Invoke();
        _isGrounded = false;
    }
}