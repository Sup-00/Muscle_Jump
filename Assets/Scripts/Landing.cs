using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing : MonoBehaviour
{
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CharectorMoving _charector;

    private bool _isJumped = false;
    private bool _jumped = false;
    private float _timer = 0.2f;

    private void Update()
    {
        if (_jumped == true && _isJumped == false)
        {
            _collider.enabled = true;
            _timer -= 1 * Time.deltaTime;

            if (_timer < 0)
            {
                _timer = 0.2f;
                _jumped = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
            other.GetComponent<Enemy>().DestroySelf();
    }


    public void SetTrigger(bool state)
    {
        _isJumped = state;
    }
}