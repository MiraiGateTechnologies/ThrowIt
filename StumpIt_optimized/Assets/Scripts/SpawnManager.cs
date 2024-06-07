using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> cameraTransforms;
    [SerializeField] private List<Transform> ballTransforms;
    [SerializeField] private List<Transform> playerStartTransforms;
    [SerializeField] private List<Transform> ChangeAnimTransforms;
    [SerializeField] private List<Transform> playerEndTransforms;
    [SerializeField] private List<Transform> afterThrowPositions;

    [SerializeField] GameObject fireBallPrefab;
    [SerializeField] GameObject BallPrefab;
    [SerializeField] GameObject Stumps;
    [SerializeField] GameObject runningPlayerPrefab;
    [SerializeField] List<GameObject> stumps;
    public bool alreadyIncremented = false;
    public int index = 0;
    int indexOfThrowCamera = 0;
    Camera cam;
    private GameObject Ball;
    public GameObject runningPlayer;
    
    private void Start()
    {
       
    }
    public void Init()
    {
        #region SpawnBall
        cam = Camera.main;

        //Ball = Instantiate(BallPrefab, ballTransforms[0].localPosition, ballTransforms[0].rotation);
        ////Ball.GetComponentInChildren<ParticleSystem>().Stop();

        //runningPlayer = Instantiate(runningPlayerPrefab, playerStartTransforms[0].localPosition, playerStartTransforms[0].rotation);
        //RunningPlayerMovement runningPlayerMovement = runningPlayer.GetComponent<RunningPlayerMovement>();
        //runningPlayerMovement.StartMovement(playerStartTransforms[0], playerEndTransforms[0], ChangeAnimTransforms[0]);

        //GameManager.Instance.currentPosition = ballTransforms[0].gameObject.GetComponent<PositionsManager>().position;
        //GameManager.Instance.isBallBehindTheStump = ballTransforms[0].gameObject.GetComponent<PositionsManager>().behindStumpPosition;

        StartCoroutine(TransitionCamera(0));
        Debug.Log("Index ::: " + index);

        #endregion


        //cam.transform.LookAt(Stumps.transform);
        //index = 1;
        //indexOfThrowCamera = 0;
    }
    public void IncrementIndexIfMiss()
    {
        indexOfThrowCamera++;
        if (indexOfThrowCamera > afterThrowPositions.Count - 1)
            indexOfThrowCamera = 0;
        alreadyIncremented = true;
        Debug.Log("Already incremented");
        Debug.Log("Incex of throw cam = " + indexOfThrowCamera);
    }

    public void ChangeAfterThrowCameraPosition()
    {
        if (GameManager.Instance.gameOver) return;

       if(Ball.GetComponent<SwipeBallManager>().swipeDistance >10f)
        StartCoroutine(TransitionCameraAtThrow(index));
        //if (alreadyIncremented) return;
        //indexOfThrowCamera++;
        //if (indexOfThrowCamera > afterThrowPositions.Count-1)
        //    indexOfThrowCamera = 0;
    }
    private IEnumerator TransitionCameraAtThrow(int i)
    {
        yield return new WaitForSeconds(0.01f);
        float lerpSpeed = 5f;
        Transform targetCameraTransform = afterThrowPositions[i];

        StartCoroutine(LerpPosition(cam.transform, targetCameraTransform.localPosition, lerpSpeed));
    }

    public void LerpTransform()
    {
        if (GameManager.Instance.gameOver) return;
        index ++;
        
        if (index > GameManager.Instance.changePositions.ballTransforms.Count - 1)
            index = 0;

        Debug.Log("Index ::: " + index);
        StartCoroutine(TransitionCamera(index));
        

         
        //index
    }

    private IEnumerator TransitionCamera(int i)
    {
        
        yield return new WaitForSeconds(0.5f);
        if(Ball)
        Destroy(Ball);
        if(runningPlayer)
        Destroy(runningPlayer);

        float lerpSpeed = 3f; // Adjust the speed as needed

        Transform targetCameraTransform = cameraTransforms[i];
        Transform targetBallTransform = ballTransforms[i];
        Transform targetStartRunningPlayerTransform = playerStartTransforms[i];
        Transform targetEndRunningPlayerTransform = playerEndTransforms[i];
        Transform targetAnimRunningPlayerTransform = ChangeAnimTransforms[i];


        foreach (var items in stumps)
        {
            items.GetComponent<Stumps>().down = false;
        }

        // Lerp the camera position
        StartCoroutine(LerpPosition(cam.transform, targetCameraTransform.localPosition, lerpSpeed));

        // Slerp the camera rotation
        StartCoroutine(SlerpRotation(cam.transform, targetCameraTransform.localRotation, lerpSpeed));

        // Spawn ball at the corresponding index
        Ball = Instantiate(BallPrefab, targetBallTransform.localPosition, targetBallTransform.localRotation);

        runningPlayer = Instantiate(runningPlayerPrefab, targetStartRunningPlayerTransform.localPosition, targetStartRunningPlayerTransform.rotation);
        RunningPlayerMovement runningPlayerMovementObj = runningPlayer.GetComponent<RunningPlayerMovement>();//.StartMovement();
        runningPlayerMovementObj.StartMovement(targetStartRunningPlayerTransform, targetEndRunningPlayerTransform, targetAnimRunningPlayerTransform);

        if (GameManager.Instance.streakStarted == true)
        {

            Ball.GetComponentInChildren<ParticleSystem>().Play();
            //Ball.GetComponentInChildren<ParticleSystem>().Play();
        }
        else
        {
            Ball.GetComponentInChildren<ParticleSystem>().Stop();
            //Ball = Instantiate(BallPrefab, targetBallTransform.localPosition, targetBallTransform.localRotation);
            //runningPlayer = Instantiate(runningPlayerPrefab, targetStartRunningPlayerTransform.localPosition, targetStartRunningPlayerTransform.rotation);
            //RunningPlayerMovement runningPlayerMovementObj = runningPlayer.GetComponent<RunningPlayerMovement>();//.StartMovement();
            //runningPlayerMovementObj.StartMovement(targetStartRunningPlayerTransform, targetEndRunningPlayerTransform, targetAnimRunningPlayerTransform);
        }

        GameManager.Instance.currentPosition = targetBallTransform.gameObject.GetComponent<PositionsManager>().position;
        GameManager.Instance.isBallBehindTheStump = targetBallTransform.gameObject.GetComponent<PositionsManager>().behindStumpPosition;

        yield return new WaitForSeconds(1f); // Wait for 1 second before transitioning to the next index
        GameManager.Instance.positionReset = false;
        alreadyIncremented = false;
        
    }

    private IEnumerator LerpPosition(Transform objectToLerp, Vector3 targetPosition, float lerpSpeed)
    {
        float t = 0f;
        Vector3 startPosition = objectToLerp.position;

        while (t < 1f)
        {
            t += Time.deltaTime * lerpSpeed;
            objectToLerp.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
            if (Ball != null)
            {
                Vector3 direction = Ball.transform.position - transform.position;

                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }

        objectToLerp.position = targetPosition;
    }

    private IEnumerator SlerpRotation(Transform objectToSlerp, Quaternion targetRotation, float lerpSpeed)
    {
        float t = 0f;
        Quaternion startRotation = objectToSlerp.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * lerpSpeed;
            objectToSlerp.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        objectToSlerp.rotation = targetRotation;
    }
    public void DestroyElements()
    {
        Destroy(Ball);
    }
}
