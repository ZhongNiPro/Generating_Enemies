using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(Renderer))]

public class Enemy : MonoBehaviour                                                       
{
    private Target _target;
    private float _speed = 3f;
    private Vector3 zeroPosition = default;

    public event Action<Enemy> Collided;

    private void Update()
    {
        if (GetComponent<Collider>().GetType() == typeof(SphereCollider))
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (GetComponent<Collider>().GetType() == typeof(CapsuleCollider))
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (GetComponent<Collider>().GetType() == typeof(BoxCollider))
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }

        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Target>())
        {
            Collided.Invoke(this);
        }
    }

    public void SetTarget(Target target)
    {
        _target = target;
    }

    public void SetSpawnPoint(Vector3 point)
    {
        zeroPosition = point;
    }

    public void RefreshPosition()
    {
        transform.position = zeroPosition;
    }
}
