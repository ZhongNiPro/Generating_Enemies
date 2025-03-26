using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(Renderer))]

public class Enemy : MonoBehaviour                                                       
{
    [SerializeField] private Color _color;

    private Target _target;
    private float _speed = 3f;
    private Vector3 _zeroPosition = default;

    public event Action<Enemy> Collided;

    private void Awake()
    {
        GetComponent<Renderer>().material.color = _color;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Target>())
        {
            Collided?.Invoke(this);
        }
    }

    public void SetTarget(Target target)
    {
        _target = target;
    }

    public void SetSpawnPoint(Vector3 point)
    {
        _zeroPosition = point;
    }

    public void RefreshPosition()
    {
        transform.position = _zeroPosition;
    }
}
