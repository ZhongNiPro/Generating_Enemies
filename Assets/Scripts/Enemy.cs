using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Target[] _targets;
    private Target _target;
    private float _speed = 3f;

    public string TargetTag { get; private set; } = default;

    public event Action<Enemy> Collided;

    private void Awake()
    {
        _targets = FindObjectsOfType<Target>();
    }

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

        foreach (Target target in _targets)
            if (target.CompareTag(TargetTag))
                _target = target;

        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Target>())
        {
            Collided.Invoke(this);
        }
    }

    public void ChooseTargetTag(string tag)
    {
        TargetTag = tag;
    }
}
