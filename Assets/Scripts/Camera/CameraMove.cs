using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private CharectorMoving _player;

    private bool _isJumped = false;

    private void Update()
    {
        if (_isJumped == false)
        {
            transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 5f,
                _player.transform.position.z - 9f);
        }

        if (_isJumped)
        {
            transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 10f,
                _player.transform.position.z - 3f);
        }
    }

    public void OnJump()
    {
        transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        _isJumped = true;
    }

    public void OnGrounded()
    {
        transform.rotation = Quaternion.Euler(30f, 0f, 0f);
        _isJumped = false;
    }
}