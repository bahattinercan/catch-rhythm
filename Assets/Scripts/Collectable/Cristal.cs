using UnityEngine;

public class Cristal : MonoBehaviour
{
    private int value;

    private void Start()
    {
        value = GameManager.instance.cristalValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.player))
        {
            GameManager.instance.AddScore(value,true,true);
            Destroy(gameObject);
        }
    }
}