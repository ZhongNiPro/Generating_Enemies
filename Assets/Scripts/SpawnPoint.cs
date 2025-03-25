using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Target _target;

    private readonly float _maxAngle = 90;

    private void Update()
    {
        transform.Rotate(Random.Range(0, _maxAngle), Random.Range(0, _maxAngle), Random.Range(0, _maxAngle));
    }

    private void OnDrawGizmos()
    {
        Vector3 size = new(.5f, .5f, .5f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }

    public Enemy SpawnEnemy()
    {
        Enemy enemy = Instantiate(_enemy);
        enemy.transform.SetPositionAndRotation(transform.position, transform.rotation);
        enemy.SetTarget(_target);
        enemy.SetSpawnPoint(transform.position);

        return enemy;
    }
}
