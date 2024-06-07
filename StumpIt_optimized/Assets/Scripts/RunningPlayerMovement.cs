using System;
using UnityEngine;

public class RunningPlayerMovement : MonoBehaviour
{
    public Animator animator;
    public Transform startPos; // Starting position
    public Transform endPos;   // Ending position
    public Transform animPos; // Animation position
    public float animationDistanceThreshold = 0.5f; // Distance threshold for playing animation


    public float speed = 1.0f; // Movement speed
    public Transform startPosReset;
    public Transform endPosReset;
    public Transform animPosRest;
    private float startTime;
    private float journeyLength;
    public bool hit = false;
    private void OnEnable()
    {
        GameManager.Instance.OnStumpHit += StopAnimation;
        GameManager.Instance.OnLivesFinished += MakeHitTrue;
        GameManager.Instance.OnLivesRefilled += MakeHitFalse;
    }

    private void MakeHitFalse()
    {
        hit = false;
        startPos = startPosReset;
        endPosReset = endPos = endPosReset;
        animPos = animPosRest;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPos.position, endPos.position);
    }

    private void MakeHitTrue()
    {
        hit = true;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStumpHit -= StopAnimation;
        GameManager.Instance.OnLivesFinished -= MakeHitTrue;
        GameManager.Instance.OnLivesRefilled -= MakeHitFalse;
    }
    public void StartMovement(Transform startPos,Transform endPos,Transform animPos)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.animPos = animPos;
        startPosReset = startPos; 
        endPosReset = endPos;
        animPosRest = animPos;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPos.position, endPos.position);
    }

    void Update()
    {
        if (hit)
        {
            return;
        }
        float distCovered = (Time.time - startTime) * speed;

        float fracJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(startPos.position, endPos.position, fracJourney);


        if (fracJourney >= 0.8f)
        {
            Debug.Log("<color=green>Object reached the destination!</color>");
           
            GameManager.Instance.changePositions.DestroyElements();
            GameManager.Instance.ResetPositions();
            GameManager.Instance.changePositions.IncrementIndexIfMiss();
            hit = true;
            enabled = true;
        }
        if (Vector3.Distance(transform.position, animPos.position) <= animationDistanceThreshold)
        {
            // Play animation
            if (animator != null)
            {
                animator.SetTrigger("ChangeAnimation");
            }
            // Optionally, perform any other action when near animation position
            Debug.Log("Near animation position!");
        }
    }

    public void StopAnimation()
    {
        hit = true;
        animator.SetTrigger("Stop");
    }
}
