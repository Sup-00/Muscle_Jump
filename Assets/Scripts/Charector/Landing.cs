using System;
using UnityEngine;
using DG.Tweening;

public class Landing : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CharectorMoving _charector;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Animator _animator;
    [SerializeField] private RagDallTrower _ragDallTrower;

    private CameraMove _cameraMove;
    private SphereCollider _collider;
    private bool _isJumped = false;
    private bool _jumped = false;
    private float _triggerTimer = 0.2f;
    private float _slowMotionTimer;
    private bool _isPlayed = false;

    private string LANDING_TRIGGER = "Landing";

    public bool IsRingActive => _spriteRenderer.enabled;

    private void Start()
    {
        _cameraMove = FindObjectOfType<CameraMove>();
        _collider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (_slowMotionTimer > 0)
        {
            Time.timeScale = 0.5f;
            _slowMotionTimer -= 1f * Time.deltaTime;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (_jumped == true && _isJumped == false)
        {
            if (_isPlayed == false)
            {
                _animator.SetTrigger(LANDING_TRIGGER);
                Camera.main.DOShakeRotation(1f, 2f);
                _particleSystem.Play();
                _charector.IsGrounded(true);
                SetRagdallrowerPosition();
                _ragDallTrower.gameObject.SetActive(true);
                _ragDallTrower.transform.DOMoveY(transform.position.y + 1f, 0.1f);
                _isPlayed = true;
            }

            _collider.enabled = true;
            _triggerTimer -= 1 * Time.deltaTime;
            _slowMotionTimer = 0.4f;

            if (_triggerTimer < 0)
            {
                _ragDallTrower.gameObject.SetActive(false);
                SetRagdallrowerPosition();
                _triggerTimer = 0.2f;
                _jumped = false;
                _isPlayed = false;
            }
        }
        else if (_jumped == false && _isJumped == false)
        {
            _collider.enabled = false;
            _spriteRenderer.enabled = false;
        }

        if (_isJumped)
        {
            _jumped = true;
            RaycastHit hit;

            Ray ray = new Ray(_charector.transform.position, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit) && hit.transform.GetComponent<Ground>())
            {
                _spriteRenderer.enabled = true;
                transform.position = hit.point + new Vector3(0, 0.1f, 0);
            }
            else
            {
                _spriteRenderer.enabled = false;
            }
        }
    }

    private void SetRagdallrowerPosition()
    {
        _ragDallTrower.transform.position =
            new Vector3(transform.position.x, transform.position.y - 4f, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
            other.GetComponent<Enemy>().DestroySelf();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<Enemy>() || other.transform.GetComponent<EnemyIgnore>() ||
            other.transform.GetComponent<RagDallTrower>())
        {
            Collider enemyCollider = other.transform.GetComponent<Collider>();
            Physics.IgnoreCollision(enemyCollider, transform.GetComponent<CapsuleCollider>());
        }
    }

    public void SetTrigger(bool state)
    {
        _isJumped = state;
    }
}