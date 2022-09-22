using UnityEngine;

public class DestroyWithTime : MonoBehaviour
{
    public float delay = 1f;

    private void Start()
    {
        Destroy(gameObject, delay);
    }
}