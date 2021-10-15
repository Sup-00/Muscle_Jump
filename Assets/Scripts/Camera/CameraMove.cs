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

        transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 6f,
            _player.transform.position.z - 7.8f);
        transform.LookAt(_player.transform);
    }

    public void OnJump()
    {
        transform.DORotate(new Vector3(35f, 0f, 0f), 0.4f);
    }

    public void OnLanding()
    {
    }
}