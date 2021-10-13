using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class CharectorMoving : MonoBehaviour
{
    [SerializeField] private float _startMovingSpeed;
    [SerializeField] private GameObject _target;

    private Landing _landing;
    private CameraMove _cameraMove;
    private Animator _animator;
    private float _lastFrameTargetPosition;
    private bool _isGrounded;
    private float _currentMovingSpeed;

    private string RUN_TRIGGET = "Run";
    private string IDLE_TRIGGET = "Idle";

    public bool IsGround => _isGrounded;

    private void Start()
    {
        _landing = GetComponentInChildren<Landing>();
        _cameraMove = FindObjectOfType<CameraMove>();
        _animator = GetComponent<Animator>();
        _currentMovingSpeed = _startMovingSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool(IDLE_TRIGGET, false);
            _animator.SetBool(RUN_TRIGGET, true);
            _lastFrameTargetPosition = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            MoveTargetPoint(_lastFrameTargetPosition);
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position,
                _currentMovingSpeed * Time.deltaTime);
            _lastFrameTargetPosition = Input.mousePosition.x;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _animator.SetBool(RUN_TRIGGET, false);
            _animator.SetBool(IDLE_TRIGGET, true);
            _lastFrameTargetPosition = 0f;
        }
    }

    private void MoveTargetPoint(float lastPosition)
    {
        float targetPosition = Input.mousePosition.x - lastPosition;
        Vector3 direction = new Vector3(targetPosition, 0, 1);

        _target.transform.Translate(direction * Time.deltaTime * _currentMovingSpeed);
        _target.transform.position =
            new Vector3(_target.transform.position.x, transform.position.y, transform.position.z + 3f);
        RotateCharector(_target.transform.position);
    }

    private void RotateCharector(Vector3 direction)
    {
        Vector3 lookDirection = direction + transform.position;
        transform.LookAt(new Vector3(lookDirection.x, transform.position.y, lookDirection.z));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<Enemy>() || other.transform.GetComponent<EnemyIgnore>() || other.transform.GetComponent<RagDallTrower>())
        {
            Collider enemyCollider = other.transform.GetComponent<Collider>();
            Physics.IgnoreCollision(enemyCollider, transform.GetComponent<CapsuleCollider>());
        }

        if (other.transform.GetComponent<Ground>())
        {
            _landing.SetTrigger(false);
        }
    }

    public void SlowMovingSpeed()
    {
        float slowingForce = 0.4f;
        _currentMovingSpeed -= slowingForce;

        if (_animator.speed > 0.2f)
        {
            _animator.speed = _currentMovingSpeed / _startMovingSpeed;
        }
    }

    public void NormolizeMovingSpeed()
    {
        if (_currentMovingSpeed < _startMovingSpeed)
        {
            float slowingForce = 0.4f;
            _currentMovingSpeed += slowingForce;
            _animator.speed = _currentMovingSpeed / _startMovingSpeed;
        }
        else
        {
            _currentMovingSpeed = _startMovingSpeed;
            _animator.speed = 1f;
        }
    }

    public void SetStartMovingSpeed()
    {
        _currentMovingSpeed = _startMovingSpeed;
    }

    public void IsGrounded(bool state)
    {
        _isGrounded = state;
    }
}