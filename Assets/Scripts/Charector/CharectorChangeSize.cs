using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharectorChangeSize : MonoBehaviour
{
    //[SerializeField] private GameObject[] _risingParts;

    private Vector3 _scaleChangeValue;
    private Ring _ring;

    private void Start()
    {
        _ring = GetComponentInChildren<Ring>();
        _scaleChangeValue = new Vector3(0.01f, 0.01f, 0.01f);
    }

    private void Update()
    {
    }

    public void RiseCharector()
    {
        transform.localScale += _scaleChangeValue;
        _ring.transform.localScale += _scaleChangeValue;

        /*foreach (var part in _risingParts)
        {
            part.transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
        }*/
    }
}