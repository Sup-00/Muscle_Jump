using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private CharectorMoving _player;

    private void Update()
    {
        transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 10f,
            _player.transform.position.z - 10f);
    }

    public void OnJump()
    {
        transform.DORotate(new Vector3(45f, 0f, 0f), 0.8f);
    }

    public void OnGrounded()
    {
        transform.DORotate(new Vector3(25f, 0f, 0f), 0.8f);
    }
}