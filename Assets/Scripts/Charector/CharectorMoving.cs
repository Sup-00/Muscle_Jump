using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class CharectorMoving : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _target;
    [SerializeField] private UnityEvent _run;
    [SerializeField] private UnityEvent _jump;
    [SerializeField] private UnityEvent _grounded;
    [SerializeField] private UnityEvent _idle;

    private bool _onJumpTrigger = false;
    private float _lastFrameTargetPosition;
    private bool _isGrounded;
    private Animator _animator;
    private float _startMoveSpeed;
    private Vector3 _scaleChangeValue;
    private Ring _ring;

    private string GROUDED_TRIGGER = "Grounded";

    public bool IsGrounded => _isGrounded;

    private void Start()
    {
        _ring = GetComponentInChildren<Ring>();
        _scaleChangeValue = new Vector3(0.02f, 0.02f, 0.02f);
        _startMoveSpeed = _moveSpeed;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_onJumpTrigger)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            _run.Invoke();
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
            _idle.Invoke();
            _lastFrameTargetPosition = 0f;
        }
    }

    private void MoveTargetPoint(float lastPosition)
    {
        float targetPosition = Input.mousePosition.x - lastPosition;
        Vector3 direction = new Vector3(targetPosition, 0, 1);

        _target.transform.Translate(direction * Time.deltaTime * _moveSpeed);
        _target.transform.position =
            new Vector3(_target.transform.position.x, transform.position.y, transform.position.z + 3f);
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
        _animator.speed = 1f;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 1000);
        _onJumpTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<JumpTrigger>())
        {
            _jump.Invoke();
            _onJumpTrigger = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<Enemy>() || other.transform.GetComponent<EnemyIgnore>())
        {
            Collider enemyCollider = other.transform.GetComponent<Collider>();
            Physics.IgnoreCollision(enemyCollider, transform.GetComponent<CapsuleCollider>());
        }

        if (other.transform.GetComponent<Ground>())
        {
            _grounded.Invoke();
            _isGrounded = true;
            _animator.SetBool(GROUDED_TRIGGER, _isGrounded);
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
        float slowingForce = 0.4f;
        _moveSpeed -= slowingForce;

        if (_animator.speed > 0.2f)
        {
            _animator.speed = _moveSpeed / _startMoveSpeed;
        }
    }

    public void NormolizeMoving()
    {
        if (_moveSpeed < _startMoveSpeed)
        {
            float slowingForce = 0.4f;
            _moveSpeed += slowingForce;
            _animator.speed = _moveSpeed / _startMoveSpeed;
        }
        else
        {
            _moveSpeed = _startMoveSpeed;
            _animator.speed = 1f;
        }
    }

    public void RiseCharector()
    {
        transform.localScale += _scaleChangeValue;
        _ring.transform.localPosition += _scaleChangeValue;
    }
}