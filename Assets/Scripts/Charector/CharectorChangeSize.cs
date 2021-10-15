using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharectorChangeSize : MonoBehaviour
{
    [SerializeField] private GameObject[] _risingParts;
    [SerializeField] private float _rizePartSize;
    [SerializeField] private float _rizeSize;

    private Ring _ring;

    private void Start()
    {
        _ring = GetComponentInChildren<Ring>();
    }

    private void Update()
    {
    }

    public void RiseCharector()
    {
        transform.localScale += new Vector3(_rizeSize, _rizeSize, _rizeSize);
        _ring.transform.localScale += new Vector3(_rizeSize, _rizeSize, _rizeSize);

        if (_risingParts[0].transform.localScale.x < 1f)
        {
            foreach (var part in _risingParts)
            {
                part.transform.localScale += new Vector3(_rizePartSize, _rizePartSize, _rizePartSize);
            }
        }
    }
}