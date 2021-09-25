using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Material _deadMaterial;
    [SerializeField] private Enemy _enemyPrefab;

    private SpawnPoint[] _spawnPoints;
    private List<Enemy> _enemies;

    private void Start()
    {
        _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        _enemies = new List<Enemy>();

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            _enemies.Add(Instantiate(_enemyPrefab, _spawnPoints[i].transform));
            _enemies[i].Init(_deadMaterial);
        }
    }
}