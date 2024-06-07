using UnityEngine;

public class SwipeController : MonoBehaviour
{
    public Rigidbody ballRigidbody; // Assign your ball's Rigidbody in the inspector
    public float throwForce = 500f; // Adjust the force as needed

    private Vector2 startPos;
    private Vector2 endPos;
    private float swipeDistance;
    private float swipeTime;

    private void Start()
    {
        // Ensure the Rigidbody is not kinematic to respond to physics
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    swipeTime = Time.time;
                    break;

                case TouchPhase.Ended:
                    endPos = touch.position;
                    swipeDistance = (endPos - startPos).magnitude;
                    float endTime = Time.time - swipeTime;

                    SwipeThrow(endPos - startPos, swipeDistance, endTime);
                    break;
            }
        }
    }

    private void SwipeThrow(Vector2 swipeDirection, float distance, float time)
    {
        ballRigidbody.isKinematic = false;
        // Calculate the swipe direction in world space based on the camera's orientation
        Vector3 horizontalDirection = Camera.main.transform.right * swipeDirection.x;
        Vector3 verticalDirection = Camera.main.transform.forward * swipeDirection.y;
        // Ensure that the swipe force is applied horizontally
        verticalDirection.y = 0;

        Vector3 worldSwipeDirection = horizontalDirection + verticalDirection;

        float velocity = distance / time; // Basic calculation of swipe velocity
        // Apply force to the ball in the direction of the swipe
        // The force is normalized and then multiplied by the swipe velocity and throwForce factor
        ballRigidbody.AddForce(worldSwipeDirection.normalized * velocity * throwForce);
    }
}