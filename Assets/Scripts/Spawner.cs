using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] _spawnPoints;

    private readonly float _repeatRate = .2f;
    private readonly int _poolCapacity = 100;
    private readonly int _poolMaxSize = 100;

    private ObjectPool<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            createFunc: () => OnCreateAction(),
            actionOnGet: (enemy) => OnGetAction(enemy),
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

    private Enemy OnCreateAction()
    {
        Enemy enemy = _spawnPoints[Random.Range(0, _spawnPoints.Length)].SpawnEnemy();

        return enemy;
    }

    private void OnGetAction(Enemy enemy)
    {
        enemy.RefreshPosition();

        enemy.Collided += OnBorderOut;
        enemy.gameObject.SetActive(true);
    }

    private void OnBorderOut(Enemy enemy)
    {
        enemy.Collided -= OnBorderOut;

        _enemyPool.Release(enemy);
    }

    private IEnumerator WaitForUse(float seconds)
    {
        while (enabled)
        {
            yield return new WaitForSeconds(seconds);

            _enemyPool.Get();
        }
    }
}
