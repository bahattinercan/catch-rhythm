using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public EColor color;
    private int value = 2;
    public EObstacle eObstacle;

    // Start is called before the first frame update
    private void Start()
    {
        value = GameManager.instance.obsValue;
        // random value
        switch (eObstacle)
        {
            case EObstacle.normal:
                value = Random.Range(value - 1, value + 1);
                break;

            case EObstacle.chance:
                value = Random.Range(value - 10, value + 10);
                break;

            case EObstacle.multiplier:
                value = Random.Range(value, value + 1);
                break;

            case EObstacle.divide:
                value = Random.Range(value, value + 1);
                break;

            case EObstacle.minus:
                value = Random.Range(value - 5, value + 10);
                break;

            case EObstacle.plus:
                value = Random.Range(value - 5, value + 5);
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.player))
        {
            PlayerColor pColor = other.GetComponent<PlayerColor>();
            if (color == pColor.color)
            {
                switch (eObstacle)
                {
                    case EObstacle.normal:
                        GameManager.instance.AddScore(value);
                        break;
                    default:
                        break;
                }
                GameManager.instance.UpdateScore();
                gameObject.SetActive(false);
            }
            else
            {
                GameManager.instance.Fail();
                gameObject.SetActive(false);
            }
        }
    }
}