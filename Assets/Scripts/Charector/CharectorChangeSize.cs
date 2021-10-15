using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharectorChangeSize : MonoBehaviour
{
    [SerializeField] private GameObject[] _risingParts;
    [SerializeField] private float _risePartSize;
    [SerializeField] private float _riseSize;
    [SerializeField] private float _rizeRingSize;

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
        transform.localScale += new Vector3(_riseSize, _riseSize, _riseSize);
        _ring.transform.localScale += new Vector3(_rizeRingSize, _rizeRingSize, _rizeRingSize);

        if (_risingParts[0].transform.localScale.x < 1f)
        {
            foreach (var part in _risingParts)
            {
                part.transform.localScale += new Vector3(_risePartSize, _risePartSize, _risePartSize);
            }
        }
    }
}