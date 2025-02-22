using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Renderer _renderer;

    public event Action<Enemy> Collided;

    public Renderer Renderer => _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>(); 
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Border"))
        {
            Collided.Invoke(this);
        }
    }
}
