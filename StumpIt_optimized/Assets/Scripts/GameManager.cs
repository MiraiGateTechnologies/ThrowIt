using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TimerManager         timerManager;
    public StumpBailManager     stumpBailManager;
    public SpawnManager         changePositions;
    public ScoreManager         scoreManager;
    public UiManager            uiManager;
    public SceneLoader          sceneLoader;
    public CameraShake          cameraShake;
    public PopupManager         popupManager;
    public PopupUIColorChanger  popupUIColorChanger;
    public AudioManager         audioManager;

    [SerializeField] List<GameObject> Bails;

    public bool gameOver = false;
    public bool streakStarted = false;
    public int throwCounter = 0;
    public bool positionReset = false;
    public bool firstHitRed = false;
    public int stumpsDown = 0;
    public bool missedShot = true;
    public int TotalShots = 0;
    public bool resetTimer = false;
    public bool waitTimer;
    public bool timerEnded=false;
    public bool waitGamePlay = false;
    public Position currentPosition;
    public BehindTheStump isBallBehindTheStump;
    public Action OnStumpHit;
    public Action OnLivesFinished;
    public Action OnLivesRefilled;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //Time.timeScale = 0f;
        //InitGame(); 
    }
    public void InitGame()
    {
        timerManager.timerIsRunning = true;
        waitGamePlay = false;
        timerManager.Init();
        stumpBailManager.Init();
        changePositions.Init();
        scoreManager.Init();;
        uiManager.Init();
        
        cameraShake.Init(); 
        audioManager.Init();
    }
    public void ResetPositions()
    {
        if(positionReset==false)
        {
            if (missedShot == false)
            {
                missedShot = true;
                Debug.Log("Missed shot Set true");
            }
            else
            {
                BallThrowCheck();
            }

            firstHitRed = false;
            stumpsDown = 0;
            changePositions.LerpTransform();
            StartCoroutine(stumpBailManager.ResetStumpsAndBails());
            positionReset = true;
        }
        
    }
    public void BallThrowCheck()
    {
        uiManager.StreakReset();
        StartCoroutine(scoreManager.LivesReduce());

    }
    public void GameOver()
    {
        gameOver = true;
        uiManager.ResetScreenEnable();
        changePositions.DestroyElements();
    }

    public void ManageBails()
    {
        foreach(var bail in Bails)
        {
            bail.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
