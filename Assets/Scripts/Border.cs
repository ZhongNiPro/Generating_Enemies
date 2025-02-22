using UnityEngine;

public class Border : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        float radius = GetComponent<SphereCollider>().radius;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}