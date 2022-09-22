using UnityEngine;

public class DestroyWithTag : MonoBehaviour
{
    public string tagg;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagg))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag(tagg))
        {
            Destroy(collision.gameObject);
        }
    }
}