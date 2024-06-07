using System.Collections;
using TMPro;
using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI streakText;
    [SerializeField] ParticleSystem redStumpParticleSystem;
    [SerializeField] TextMeshProUGUI missText;
    [SerializeField] public ParticleSystem BurstParticleEffect;
    [SerializeField] public ParticleSystem DustParticleEffect;

    public int score = 0;
    private int blueStumpScore = 1;
    private int redStumpScore = 3;
    public const int TotalLives = 3;
    public int CurrentLives = 3;
    public int currentStreak = 0;
    int currentStreakLevel = 5;
    public int currentTotalScore = 0;
    private void Start()
    {
       
    }
    public void Init()
    {
        redStumpParticleSystem.Stop();
        streakText.text = "+2";
    }
    public void IncreaseScore(Stump stumpType)
    {
        if (GameManager.Instance.firstHitRed == true)
        {
            switch (stumpType)
            {
                case Stump.Red:
                    StartCoroutine(PlayParticle());
                    score += redStumpScore;
                    currentTotalScore += redStumpScore;
                    Debug.Log("<color=red>Red Score</color>" + currentTotalScore);
                    break;

                case Stump.Blue:
                    score += blueStumpScore;
                    currentTotalScore += blueStumpScore;
                    Debug.Log("<color=blue>Blue Score</color>" + currentTotalScore);
                    break;
            }
        }
        else
        {
            score += blueStumpScore;
            currentTotalScore += blueStumpScore;
            Debug.Log("<color=blue>Blue Score</color>" + currentTotalScore);
        }
        if (currentStreakLevel > 5)
            BurstParticleEffect.Play();
        scoreText.text = score.ToString();
        
        FusionConnector.Instance.UpdateScore(score);
        
    }

    public IEnumerator LivesReduce()
    {
      //  GameManager.Instance.changePositions.IncrementIndexIfMiss();  COMMENTED THIS BECAUSE EVEN IS IF BALL MISSES BUT INCREMENT IS CALLED ALREADY
        GameManager.Instance.audioManager.PlayMissSound();
        RunningPlayerMovement runningPlayerMovement=FindAnyObjectByType<RunningPlayerMovement>();
        runningPlayerMovement.StopAnimation();
        GameManager.Instance.streakStarted = false;
        missText.gameObject.SetActive(true);
        CurrentLives -= 1;
        currentStreak = 0;
        currentStreakLevel = 5;
        streakText.text = "+2";
        blueStumpScore = 1;
        redStumpScore = 3;
        GameManager.Instance.uiManager.ReduceLife(CurrentLives);
        if (CurrentLives < 1)
        {
         
            GameManager.Instance.uiManager.WaitScreen();
            CurrentLives = 3;
            GameManager.Instance.uiManager.StreakReset();
        }
        else
        {
            yield return new WaitForSeconds(0.3f); 
            Debug.Log("Current Streak = " + currentStreak.ToString());

        }
            missText.gameObject.SetActive(false);
    }

    public void ScoreMultiplier(int currentStreak)
    {
        switch (currentStreak)
        {
            case 5:
                blueStumpScore += 2;
                redStumpScore += 2;
               // streakText.text = "+2";
                break;
            case 10:
                blueStumpScore += 1;
                redStumpScore += 1;
                streakText.text = "+3";
                break;
            case 15:
                blueStumpScore += 1;
                redStumpScore += 1;
                streakText.text = "+4";
                break;
        }
    }
    IEnumerator PlayParticle()
    {
        redStumpParticleSystem.Play();
        yield return new WaitForSeconds(0.1f);
        redStumpParticleSystem.Stop();
    }
    public void IncreaseStreak()
    {
        currentStreak++;
        string message="";
        if (currentStreak >= currentStreakLevel)
        {
            GameManager.Instance.streakStarted = true;
            ScoreMultiplier(currentStreakLevel);
            currentStreakLevel += 5;
            GameManager.Instance.uiManager.StreakReset();
            switch(currentStreak)
            {
                case 5:
                    message = PopupManager.twoScoreText;
                    break;

                case 10:
                    message = PopupManager.threeScoreText;
                    break;
                case 15:
                    message = PopupManager.fourScoreText;
                    break;

            }

        }
        else
        {
            message = GameManager.Instance.popupManager.GetRandomPopupMessage();
        }
            StartCoroutine(GameManager.Instance.uiManager.DisplayPopup(message));

    }

}
