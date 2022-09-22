using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    [SerializeField] private bool isRandom = false;
    [SerializeField] private bool isReverse = false;

    private void Start()
    {
        if(isRandom)
            speed = Random.Range(speed * .75f, speed * 1.25f);

        if (isReverse)
            if (Random.Range(0, 2) == 1)
                rotation *= -1;
    }

    private void FixedUpdate()
    {
        transform.Rotate(rotation * speed * Time.deltaTime);
    }
}