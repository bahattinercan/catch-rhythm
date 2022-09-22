using UnityEngine;

public class ChangeColorObs : MonoBehaviour
{
    public EColor eColor;
    public EShape eShape;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.player))
        {
            GameManager.instance.ChangePlayerTheme(eColor, eShape);
            GameManager.instance.PlayCollectParticle();
            Destroy(transform.parent.gameObject);
        }
    }
}