using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private Transform[] _spawnPoints;

    private readonly float _repeatRate = .2f;
    private readonly int _poolCapacity = 100;
    private readonly int _poolMaxSize = 100;

    private readonly string _target0Name = "Target1";
    private readonly string _target1Name = "Target2";
    private readonly string _target2Name = "Target3";
    private readonly int _spawnPoint0Number = 0;
    private readonly int _spawnPoint1Number = 1;
    private readonly int _spawnPoint2Number = 2;

    private ObjectPool<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(
            createFunc: () => ActionOnCreate(),
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

    private Enemy ActionOnCreate()
    {
        Enemy enemy = _enemies[Random.Range(0, _enemies.Length)];

        return Instantiate(enemy);
    }

    private void ActionOnGet(Enemy enemy)
    {
        if (enemy.GetComponent<Collider>().GetType() == typeof(SphereCollider))
        {
            enemy.transform.SetPositionAndRotation(_spawnPoints[_spawnPoint0Number].position, _spawnPoints[_spawnPoint0Number].rotation); 
            enemy.ChooseTargetTag(_target0Name);
        }
        else if (enemy.GetComponent<Collider>().GetType() == typeof(CapsuleCollider))
        {
            enemy.transform.SetPositionAndRotation(_spawnPoints[_spawnPoint1Number].position, _spawnPoints[_spawnPoint1Number].rotation);
            enemy.ChooseTargetTag(_target1Name);
        }
        else if (enemy.GetComponent<Collider>().GetType() == typeof(BoxCollider))
        {
            enemy.transform.SetPositionAndRotation(_spawnPoints[_spawnPoint2Number].position, _spawnPoints[_spawnPoint2Number].rotation);
            enemy.ChooseTargetTag(_target2Name);
        }

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
        while (true)
        {
            yield return new WaitForSeconds(seconds);

            _enemyPool.Get();
        }
    }
}
