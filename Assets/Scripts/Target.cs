using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class Target : MonoBehaviour
{
    private float _pointValue = 10f;
    private float _speed = 5f;
    private int _wayCount = 5;
    private Vector3 _nextPosition;
    private Vector3[] _wayPoints;
    private Color _color;

    private void Awake()
    {
        _wayPoints = new Vector3[_wayCount];

        for (int i = 0; i < _wayCount; i++)
        {
            _wayPoints[i] = new Vector3(
                Random.Range(-_pointValue, _pointValue),
                Random.Range(-_pointValue, _pointValue),
                Random.Range(-_pointValue, _pointValue)
                );
        }

        _nextPosition = GetNextPosition();

        _color = Random.ColorHSV();
        GetComponent<Renderer>().material.color = _color;
    }

    private void Update()
    {
        if (transform.position == _nextPosition)
            _nextPosition = GetNextPosition();

        transform.position = Vector3.MoveTowards(
            transform.position, 
            _nextPosition, 
            _speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Vector3 size = new(.3f, .3f, .3f);

        Gizmos.color = _color;
        Gizmos.DrawWireCube(_nextPosition, size);
    }

    private Vector3 GetNextPosition()
    {
        int index = Random.Range(0, _wayCount);
        Vector3 nextPosition = _wayPoints[index];

        if (nextPosition == transform.position)
        {
            nextPosition = GetNextPosition();
        }

        return nextPosition;
    }
}
