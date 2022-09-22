using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float touchSpeed = 1f;
    public Touch theTouch;
    public float keyboardSpeed = 10f;

    private void FixedUpdate()
    {
        /* dönme iþlemleri
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.back, turnSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);

        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                // start phase
            }
            else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
            {
                transform.Rotate(Vector3.forward, touchSpeed * theTouch.deltaPosition.x * Time.deltaTime);
            }
        }
        */

        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector3.left * keyboardSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Vector3.right * keyboardSpeed * Time.deltaTime);

        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                // start phase
            }
            else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
            {
                transform.Translate(Vector3.right * touchSpeed * theTouch.deltaPosition.x * Time.deltaTime);
            }
        }
        if (transform.position.x > 3)
            transform.position = new Vector3(2.99f, transform.position.y, transform.position.z);
        if (transform.position.x < -3)
            transform.position = new Vector3(-2.99f, transform.position.y, transform.position.z);
    }
}