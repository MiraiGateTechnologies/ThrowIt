using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeBallManager : MonoBehaviour
{

    Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    public float swipeDistance;
    float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction

    private float windDirection = 1.2f;
    private bool windActive = false;
    private Vector3 windForce;
    private const int initialWindlessThrows = 3;
    [SerializeField]
    float throwForce = 1f;
    float additionalForce;
    [SerializeField] float throwVelocity = 6000f;

    [SerializeField] float zLimit = 10f;
    public GameObject trail;
    private Rigidbody rb;
    public bool ballInAir = false;
    bool ballOutAir = false;
    private Transform stumps;

    bool isTapOverBall;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stumps = GameObject.FindWithTag("Target").transform;
        DecideWind();
        if (GameManager.Instance.streakStarted == false)
            trail.SetActive(false);
        else trail.SetActive(true);
        // transform.LookAt(stumps.transform.position);
    }

    public void Init()
    {

    }
    private void Update()
    {
        if (GameManager.Instance.waitGamePlay == true) return;
        if (IsPointerOverUIObject()) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && ballInAir == false)
        {
            //if (IsPointerOverGameObject(Input.GetTouch(0).position) == false)
            //    return;
            //sGameManager.Instance.positionReset = false;
            isTapOverBall = IsPointerOverGameObject(Input.GetTouch(0).position);

            ballOutAir = true;
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
            timeInterval = Time.time;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && ballOutAir)
        {

            endPos = Input.GetTouch(0).position;
            float endTime = Time.time - timeInterval;
            swipeDistance = (endPos - startPos).magnitude;
            Vector2 swipeDirection = endPos - startPos;

            Debug.Log("Swipe Direction :: " + swipeDirection + "     swipeDistance::: " + swipeDistance);

            Debug.Log("i am at " + GameManager.Instance.changePositions.index);
            Debug.Log("Distance :::" + swipeDistance);
            if ((swipeDistance <= 50f))
            {
                switch (GameManager.Instance.changePositions.index)
                {
                    case 0:

                        swipeDirection = new Vector2(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(240f, 450f));
                        additionalForce = 500f;
                        break;

                    case 1:

                        swipeDirection = new Vector2(22f, 148f);
                        additionalForce = 400f;
                        break;
                    case 2:
                        swipeDirection = new Vector2(22f, 230f);
                        additionalForce = 500f;
                        break;
                    case 3:
                        additionalForce = 500f;
                        swipeDirection = new Vector2(60f, 444f);

                        break;
                    case 4:
                        additionalForce = 500f;
                        swipeDirection = new Vector2(10f, 230f);

                        break;
                    case 5:
                        additionalForce = 450f;
                        swipeDirection = new Vector2(0f, 370f);

                        break;
                    case 6:
                        additionalForce = 300f;
                        swipeDirection = new Vector2(5f, 230f);

                        break;
                }
            }
            else if (swipeDistance > 50f)
            {
                additionalForce = 500f;

            }
            if (isTapOverBall == false)
                return;
            ballInAir = true;
            ballOutAir = false;
            rb.isKinematic = false;
            GameManager.Instance.throwCounter++;
            StartCoroutine(SwipeThrow(endPos - startPos, swipeDistance, endTime));

        }

        /*
                if(transform.position.z>=zLimit)
                {
                    GameManager.Instance.ResetPositions();
                }*/
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    private bool IsPointerOverGameObject(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Physics.Raycast(ray, out RaycastHit hit);
        bool isBall = false;
        if (hit.collider.gameObject != null)
        {
            if (hit.collider.gameObject.tag == "ball")
                isBall = true;
            else
                isBall = false;
        }
        return isBall;
    }
    private IEnumerator SwipeThrow(Vector2 swipeDirection, float distance, float time)
    {

        // Calculate the swipe direction in world space based on the camera's orientation
        Vector3 horizontalDirection = Camera.main.transform.right * Mathf.Abs(swipeDirection.x);
        Vector3 verticalDirection = Camera.main.transform.forward * Mathf.Abs(swipeDirection.y);
        verticalDirection.y = 0; // Ensure that the swipe force is applied horizontally

        Vector3 worldSwipeDirection = (horizontalDirection + verticalDirection).normalized;
        Vector3 directionToTarget = (stumps.position - rb.position).normalized;

        // Adjust this value to control the mix between the swipe direction and the target direction
        float blendFactor = 0.7f; // More swipe influence with a value closer to 0

        //if (swipeDirection != Vector2.zero)
        //{
        Vector3 finalDirection = Vector3.Lerp(worldSwipeDirection, directionToTarget, 1).normalized;
        ////Vector3 finalDirection = worldSwipeDirection;
        if (windActive)
        {
            finalDirection += windForce; // Add wind force to the final direction
            finalDirection.Normalize(); // Ensure the direction is normalized after adding wind force

            Debug.Log("<color=green>WINDD</color> ");
        }



        trail.SetActive(true);
        Vector3 finalForce = finalDirection * throwVelocity * throwForce;
        //Debug.Log("FinalForce:: "+ finalForce * distanceChange);
        rb.AddForce(finalForce * additionalForce, ForceMode.Acceleration);
        isTapOverBall = false;


        //}
        GameManager.Instance.changePositions.ChangeAfterThrowCameraPosition();
        // yield return new WaitForSeconds(0.3f);


        yield return new WaitForSeconds(1f);
        Debug.Log("Current Score = " + GameManager.Instance.scoreManager.currentTotalScore);
        int score = GameManager.Instance.scoreManager.currentTotalScore;
        GameManager.Instance.uiManager.DisplayCurrentScore(score);
        GameManager.Instance.scoreManager.currentTotalScore = 0;
        if (GameManager.Instance.waitGamePlay == false)
        {
            Debug.Log("<color=yellow>RESET CALLED</color>");
            GameManager.Instance.ResetPositions();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stump") && GameManager.Instance.missedShot == true)
        {
            if (GameManager.Instance.scoreManager.currentStreak > 5)
                GameManager.Instance.scoreManager.BurstParticleEffect.Play();
            else
                GameManager.Instance.scoreManager.DustParticleEffect.Play();
            GameManager.Instance.audioManager.PlayRandomRunOutSound();
            Debug.Log("<color = green>NOW COLLIDED</color>");
            GameManager.Instance.scoreManager.IncreaseStreak();
            GameManager.Instance.uiManager.StreakIncrease();
            GameManager.Instance.missedShot = false;
            GameManager.Instance.cameraShake.Shake(0.5f, 0.2f);
            GameManager.Instance.ManageBails();
            GameManager.Instance.OnStumpHit?.Invoke();
        }
        else if (collision.gameObject.CompareTag("Stump") && GameManager.Instance.missedShot == false) Debug.Log("<color=orange>Stump hit but Wrong bool</color>");
    }
    private void DecideWind()
    {
        if (GameManager.Instance.throwCounter >= initialWindlessThrows)
        {
            // Randomly enable wind
            windActive = Random.Range(0, 2) == 0; // 50% chance to activate wind after initial throws
            Debug.Log("wind  = " + windActive);
            if (windActive)
            {
                // Randomly determine wind force and direction
                float xDirection = Random.Range(-0.07f, 0.07f);
                windForce = new Vector3(xDirection, 0, Random.Range(0, 0.01f));
                Debug.Log("Wind Direction=  " + xDirection);
                GameManager.Instance.uiManager.DecideUIWind(xDirection);
                Debug.Log("<color=green>WINDD</color> " + windForce);
            }
        }
    }

}