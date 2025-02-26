using UnityEngine;

public class Border : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Vector3 size = new(20, 20, 20);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, size);
    }
}