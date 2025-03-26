using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] _spawnPoints;

    private readonly float _repeatRate = .2f;
    private readonly int _poolCapacity = 100;
    private readonly int _poolMaxSize = 100;

    private WaitForSeconds _waitForSeconds;

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

        _waitForSeconds = new WaitForSeconds(_repeatRate);
    }

    private void Start()
    {
        StartCoroutine(WaitForUse());
    }

    private Enemy OnCreateAction()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Length)].SpawnEnemy();
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

    private IEnumerator WaitForUse()
    {
        while (enabled)
        {
            yield return _waitForSeconds ;

            _enemyPool.Get();
        }
    }
}
