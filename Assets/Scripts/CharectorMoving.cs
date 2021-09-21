using System;
using UnityEngine;
using DG.Tweening;

public class CharectorMoving : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private bool _onJumpTrigger = false;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveForward();
        }
        else if (_onJumpTrigger)
        {
            Jump();
        }
    }

    private void MoveForward()
    {
        transform.position += Vector3.forward * _moveSpeed * Time.deltaTime;

        Vector3 mousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        if (mousePosition.x != transform.position.x)
        {
            if (mousePosition.x > transform.position.x)
            {
                if (transform.rotation.y != 50f)
                {
                    transform.DORotate(new Vector3(0, 50f, 0), 1f);
                }

                transform.position += Vector3.right * _moveSpeed * Time.deltaTime * 1.5f;
            }
            else if (mousePosition.x < transform.position.x)
            {
                if (transform.rotation.y != -50f)
                {
                    transform.DORotate(new Vector3(0, -50f, 0), 1f);
                }

                transform.position += Vector3.left * _moveSpeed * Time.deltaTime * 1.5f;
            }
        }
    }

    private void Jump()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<JumpTrigger>())
        {
            Debug.Log("Jump)");
            _onJumpTrigger = true;
        }
    }
}