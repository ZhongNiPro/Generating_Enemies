using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Transform[] _spawnPoints;

    private readonly float _repeatRate = .2f;
    private readonly int _poolCapacity = 100;
    private readonly int _poolMaxSize = 100;

    private ObjectPool<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_enemy),
            actionOnGet: (enemy) => ActionOnGet(enemy),
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(WaitForUse(_repeatRate));
    }

    private void ActionOnGet(Enemy enemy)
    {
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        enemy.Collided += OnBorderOut;
        enemy.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        enemy.Renderer.material.color = Random.ColorHSV();
        enemy.gameObject.SetActive(true);
    }

    private void OnBorderOut(Enemy enemy)
    {
        enemy.Collided -= OnBorderOut;

        _enemyPool.Release(enemy);
    }

    private IEnumerator WaitForUse(float seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);

            _enemyPool.Get();
        }
    }
}
