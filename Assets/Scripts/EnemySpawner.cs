using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Material _deadMaterial;
    [SerializeField] private Enemy _enemyPrefab;

    private Landing _landing;
    private CharectorChangeSize _charectorChangeSize;
    private CharectorMoving _charectorMoving;
    private Ring _ring;
    private SpawnPoint[] _spawnPoints;
    private List<Enemy> _enemies;

    private void Start()
    {
        _landing = FindObjectOfType<Landing>();
        _ring = FindObjectOfType<Ring>();
        _charectorMoving = FindObjectOfType<CharectorMoving>();
        _charectorChangeSize = FindObjectOfType<CharectorChangeSize>();
        _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        _enemies = new List<Enemy>();

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            _enemies.Add(Instantiate(_enemyPrefab, _spawnPoints[i].transform));
            _enemies[i].Init(_deadMaterial, _charectorChangeSize, _charectorMoving, _ring, _landing);
        }
    }
}