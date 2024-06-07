using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Transform targetPosition;
    public float speed = 5f;
    private bool shouldMove = false;
    public Animator animator; // Assign this in the inspector
    public bool ballHit = false; // You can set this to true externally when needed

    public void StartMoving()
    {
        shouldMove = true;
        ballHit = false;
    }

    void Update()
    {
        if (shouldMove && !ballHit)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, step);
            animator.SetFloat("Speed", 1.0f); // Assume 1.0f is your running speed

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.001f)
            {
                shouldMove = false;
            }
        }
        else
        {
            animator.SetFloat("Speed", 0.0f); // Assume 0.0f is your stopping speed
        }

        // If ballHit becomes true, stop the character and play stopping animation
        if (ballHit)
        {
            shouldMove = false;
            animator.SetFloat("Speed", 0.0f); // Transition to stopping animation
        }
    }
}
