using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;

public class UiManager : MonoBehaviour
{
    [SerializeField] List<Image> liveImages = new List<Image>(3);
    [SerializeField] GameObject ResetScreen;
    [SerializeField] Button playAgainButton;
    [SerializeField] TextMeshProUGUI player1ResultScore;
    [SerializeField] TextMeshProUGUI player2ResultScore;
    [SerializeField] Image StreakBar;
    [SerializeField] GameObject liveWaitPanel;
    [SerializeField] TextMeshProUGUI waitTimeText;
    [SerializeField] TextMeshProUGUI currentScoreText;
    [SerializeField] Image popupImage;
    [SerializeField] TextMeshProUGUI popupText;
    [SerializeField] GameObject windObjectRight;
    [SerializeField] GameObject windObjectLeft;
    [SerializeField] List<Image> liveRefillImages;

    const float totalStreak = 5;
    float waitTime = 5f;
    float streakIncreaseValue;
    private float fadeDuration=1f;
   


    private void Start()
    {
      
    }

    public void Init()
    {
        //playAgainButton.onClick.AddListener(GameManager.Instance.sceneLoader.ResetScene);
        streakIncreaseValue = 1f / totalStreak;
    }
    #region Score related methods
    private void EnableLives()
    {
        foreach (var item in liveImages)
        {
            item.enabled = true;
        }
    }

    public void ReduceLife(int index)
    {
        liveImages[index].enabled = false;
    }

    public void StreakIncrease()
    {
        StreakBar.fillAmount += streakIncreaseValue;
    }
    public void StreakReset()
    {
        StreakBar.fillAmount = 0;
    }

    public void DisplayCurrentScore(int score)
    {
        currentScoreText.text = score.ToString();
        Debug.Log("CurrentScore  = "+score.ToString());
        StartCoroutine(DisplayAndFadeText(currentScoreText));
    }

    public IEnumerator DisplayAndFadeText(TextMeshProUGUI textObject)
    {
        textObject.gameObject.SetActive(true); // Ensure the text is visible
        yield return new WaitForSeconds(0f); // Wait for the display duration

        // Fade out
        float currentTime = 0;
        Color startColor = textObject.color;
        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            textObject.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        textObject.gameObject.SetActive(false);

    }

    #endregion Score Related methods

    #region Reset Screen and Wait Screen
    public void ResetScreenEnable()
    {
        player1ResultScore.text= GameManager.Instance.scoreManager.score.ToString();       
        ResetScreen.SetActive(true);
        
    }

    public void WaitScreen()
    {
     
        GameManager.Instance.waitGamePlay = true;
        liveWaitPanel.SetActive(true);
        StartCoroutine(FillImages(waitTime));

    }


    IEnumerator FillImages(float duration)
    {
        float elapsed = 0; // Time elapsed since the start of the animation

        foreach (Image image in liveRefillImages)
        {
            image.fillAmount = 0;
        }

        while (elapsed < duration)
        {
            GameManager.Instance.OnLivesFinished?.Invoke();

            elapsed += Time.deltaTime; // Increase elapsed time
            float fillAmount = elapsed / duration; // Calculate the current fill amount

            // Update the fill amount of each image in the list
            foreach (Image image in liveRefillImages)
            {
                image.fillAmount = fillAmount;
            }

            yield return null; // Wait for the next frame
        }

        // Ensure the fill amount is set to 1 for all images at the end
        foreach (Image image in liveRefillImages)
        {
            image.fillAmount = 1;
        }
        liveWaitPanel.SetActive(false);
        GameManager.Instance.OnLivesRefilled?.Invoke();
        GameManager.Instance.waitGamePlay = false;
        EnableLives();

    }

    #endregion Reset Screen and Wait Screen

    #region Popup Messages Related Methods

    public IEnumerator DisplayPopup(string textMessgae)
    {
        popupText.text = textMessgae;
        GameManager.Instance.popupUIColorChanger.DisplayPopupWithColor(popupImage);
        yield return new WaitForSeconds (1f);
        popupImage.gameObject.SetActive(false);
    }   

    public void DecideUIWind(float direction)
    {
        if(direction >= 0f && GameManager.Instance.isBallBehindTheStump==BehindTheStump.False)
        {
            StartCoroutine(displayWind(windObjectRight));
        }

        else if(direction >= 0f && GameManager.Instance.isBallBehindTheStump == BehindTheStump.True)
        {
            StartCoroutine(displayWind(windObjectLeft));
        }

        else if(direction <= 0f && GameManager.Instance.isBallBehindTheStump == BehindTheStump.True)
        {
            StartCoroutine(displayWind(windObjectRight));
        }
        else if (direction <= 0f && GameManager.Instance.isBallBehindTheStump == BehindTheStump.False)
        {
            StartCoroutine(displayWind(windObjectLeft));
        }
    }
    IEnumerator displayWind(GameObject windUI)
    {
        windUI.SetActive(true);
        yield return new WaitForSeconds (3f);
        windUI.SetActive(false);
    }


    #endregion Popup Messages Related Methods


}
