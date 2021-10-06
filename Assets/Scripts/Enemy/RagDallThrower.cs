using System;
using UnityEngine;
using DG.Tweening;

public class RagDallThrower : MonoBehaviour
{
    [SerializeField] private CharectorMoving _charector;

    private void Update()
    {
        transform.position = new Vector3(_charector.transform.position.x, -10f, _charector.transform.position.z);
    }

    public void Throw()
    {
        transform.DOMoveY(transform.position.y + 20f, 0.1f);
        
    }
}