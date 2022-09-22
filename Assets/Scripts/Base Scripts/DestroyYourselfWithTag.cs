using UnityEngine;

public class DestroyYourselfWithTag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.destroyer))
        {
            Destroy(gameObject);
        }
    }
}