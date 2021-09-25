using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private SphereCollider _collider;

    public void SetCollider(bool state)
    {
        _collider.enabled = state;
    }
}