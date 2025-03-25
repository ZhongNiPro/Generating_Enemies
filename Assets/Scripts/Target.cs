using UnityEngine;

[RequireComponent (typeof(Renderer))]

public class Target : MonoBehaviour
{
    private float _pointValue = 10f;
    private float _speed = 5f;
    private Vector3 _nextPosition;

    private void Awake()
    {
        _nextPosition = new(Random.Range(-_pointValue, _pointValue), Random.Range(-_pointValue, _pointValue), Random.Range(-_pointValue, _pointValue));
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    private void Update()
    {
        if (transform.position == _nextPosition)
            _nextPosition = GetNextPosition();

        transform.position = Vector3.MoveTowards(transform.position, _nextPosition, _speed * Time.deltaTime);
    }

    private Vector3 GetNextPosition()
    {
        Vector3 previousPosition = transform.position;
        Vector3 nextPosition = Vector3.zero;
        int axisCount = 3;
        int signCount = 2;
        int axis = Random.Range(0, axisCount);
        int sign = Random.Range(0, signCount) == 0 ? 1 : -1;

        switch (axis)
        {
            case 0:
                nextPosition = new Vector3(previousPosition.x, previousPosition.y, _pointValue * sign);
                break;

            case 1:
                nextPosition = new Vector3(previousPosition.x, _pointValue * sign, previousPosition.z);
                break;

            case 2:
                nextPosition = new Vector3(_pointValue * sign, previousPosition.y, previousPosition.z);
                break;
        }

        return nextPosition;
    }
}
