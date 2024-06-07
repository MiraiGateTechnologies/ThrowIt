using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuUiManager : MonoBehaviour
{
    [Header("Player Data")]
    public int betAmount = 0;
    public string playerName;
    public Sprite playerProfileImage;

    private int currentIndex;
    [Header("Main Menu Buttons")]
    [SerializeField] Button quickPlayButton;
    [SerializeField] Button playWithFriendsButton;
    [SerializeField] Button practiceMatch;
    [SerializeField] Button soundButton;
    [SerializeField] Button rulesButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button profileButton;
    public Button blurBG;

    [Header("Popups")]
    [SerializeField] GameObject popupObject;
    [SerializeField] GameObject hostJoinOption;
    [SerializeField] GameObject hostCodePage;
    [SerializeField] GameObject joinCodePage;
    [SerializeField] GameObject quickPlayPage;
    [SerializeField] GameObject profilePopup;
    [SerializeField] GameObject rulesPopup;

    public GameObject practicePopup;


    [Header("Host Join Options Buttons")]
    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;

    [Header("Host Code Page")]
    [SerializeField] TMP_InputField hostCode;
    [SerializeField] Button copyCodeButton;
    [SerializeField] Button shareCodeButton;
    [SerializeField] List<Button> amountsListHost;
    [SerializeField] Toggle termsAndConditionsCheck;
    [SerializeField] Button ProceedButtonHost;

    [Header("Join code page Options")]
    [SerializeField] TMP_InputField codeInput;
    [SerializeField] Button proceedButtonJoin;

    [Header("Quick Play Page")]
    [SerializeField] List<Button> amountsListQuickPlay;
    [SerializeField] GameObject checkButton;
    [SerializeField] Toggle termsAndConditionsCheckQuickPlay;
    [SerializeField] Button proceedButtonQuickPlay;

    [Header("Profile popups")]
    [SerializeField] Button editNameButton;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] Button saveButton;
    [SerializeField] List<Sprite> profilePics;
    [SerializeField] Image profileImage;
    [SerializeField] Button profileChangeButton;
    [SerializeField] TextMeshProUGUI nameText;

    [Header("Matchmaking Screen")]
    
    public TextMeshProUGUI matchTitle;
    public GameObject matchmakingScreen;
    public GameObject inGameScreen;
    [SerializeField] TextMeshProUGUI timerText;
    public TextMeshProUGUI player1Name;
    [SerializeField] TextMeshProUGUI player2Name;
    public Image player1Profile;
    [SerializeField] Image player2Profile;
    public GameObject loadingScreen;

    [SerializeField] SceneLoader sceneLoader;

    List<GameObject> allPopups = new List<GameObject>();
    public static MenuUiManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    private void Start()
    {
        allPopups.Add(hostJoinOption);
        allPopups.Add(hostCodePage);
        allPopups.Add(joinCodePage);
        allPopups.Add(quickPlayPage);
        allPopups.Add(profilePopup);

        proceedButtonQuickPlay.interactable = false;
        profileImage.sprite = profilePics[currentIndex];
        ButtonListeners();

        if (!PlayerPrefs.HasKey("ProfileName") || !PlayerPrefs.HasKey("ProfileSpriteIndex"))
        {
            blurBG.gameObject.SetActive(true);
            profilePopup.SetActive(true);
        }
        else
        {
            playerName = PlayerPrefs.GetString("ProfileName") ;
            nameText.text = playerName;
            playerProfileImage = profilePics[PlayerPrefs.GetInt("ProfileSpriteIndex")];
        }
        
    }

    public void OnCLickPracticeMatch()
    { 
        practicePopup.SetActive(true);
    }
    private void ButtonListeners()
    {
        //blurBG.onClick.AddListener(() =>
        //{
        //    popupObject.SetActive(false);
        //    foreach (var popups in allPopups)
        //    {
        //        popups.gameObject.SetActive(false);
        //    }
        //});
        quickPlayButton.onClick.AddListener(() =>
        {
            //if (string.IsNullOrEmpty(nameInput.text))
            //{
            //    blurBG.gameObject.SetActive(true);
            //    profilePopup.SetActive(true);
            //}
            //else
            //{
                popupObject.SetActive(true);
                quickPlayPage.SetActive(true);
                blurBG.gameObject.SetActive(true);
            //}
        });

        practiceMatch.onClick.AddListener(() =>
        {
            //if (string.IsNullOrEmpty(nameInput.text))
            //{
                blurBG.gameObject.SetActive(true);
                practicePopup.SetActive(true);
                matchTitle.text = "practice mode ";
            //}
            //else
            //{
            //    popupObject.SetActive(true);
            //    quickPlayPage.SetActive(true);
            //    blurBG.gameObject.SetActive(true);
            //}
        });

        termsAndConditionsCheckQuickPlay.onValueChanged.AddListener(isOn =>
        {
            if (isOn == false && betAmount > 0)
            {
                proceedButtonQuickPlay.interactable = false;
            }
            else if (isOn == true)
            {
                proceedButtonQuickPlay.interactable = true;
            }
        });

        proceedButtonQuickPlay.onClick.AddListener(() =>
        {
            quickPlayPage.SetActive(false);
            player1Profile.sprite = playerProfileImage;
            player1Name.text = playerName;
            loadingScreen.SetActive(true);
            FusionConnector.Instance.SetMatchValues(betAmount, nameText.text, currentIndex);
            matchTitle.text = "quick play";
            /*            PlayerDataHandler.Instance.UpdatePlayerData(playerName, playerProfileImage, betAmount);

                        SessionDataHandler.Instance.CreateSession(playerName,betAmount);*/
            //StartCoroutine(MatchMakingScreenStart());
            //Enable Timer Screen Here
        });
        profileButton.onClick.AddListener(() =>
        {
            blurBG.gameObject.SetActive(true);
            profilePopup.SetActive(true);
            if (PlayerPrefs.HasKey("ProfileName"))
            { 
                nameInput.gameObject.SetActive(false);
                nameText.gameObject.SetActive(true);
                saveButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "close";

                nameText.text = playerName;
                profileImage.sprite = profilePics[PlayerPrefs.GetInt("ProfileSpriteIndex")];
            }
        });
        saveButton.onClick.AddListener(() =>
        {

            if (!PlayerPrefs.HasKey("ProfileName"))
            {
                playerName = nameInput.text;
                PlayerPrefs.SetString("ProfileName", playerName);
                
            }
            else
            {
                nameInput.text = PlayerPrefs.GetString("ProfileName");
            }
            if (!PlayerPrefs.HasKey("ProfileSpriteIndex"))
            {
                PlayerPrefs.SetInt("ProfileSpriteIndex", currentIndex);
                playerProfileImage = profileImage.sprite;
            }
            else
            {
                profileImage.sprite = profilePics[PlayerPrefs.GetInt("ProfileSpriteIndex")];
            }
           
            profilePopup.SetActive(false);
            blurBG.gameObject.SetActive(false);
        });
        profileChangeButton.onClick.AddListener(() => { 
            ChangeImageButtonClicked();
        });

        rulesButton.onClick.AddListener(() => { 
            
            blurBG.gameObject.SetActive(true);
            rulesPopup.SetActive(true);
        
        });

        quitButton.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
  
        });

    }
    private void ChangeImageButtonClicked()
    {
        if (!PlayerPrefs.HasKey("ProfileName"))
        {
            currentIndex++;

            if (currentIndex >= profilePics.Count)
            {
                currentIndex = 0;
            }

            profileImage.sprite = profilePics[currentIndex];
        }

    }
    public void AmountSelectListener(int amount)
    {
        betAmount = amount;
        foreach (var buttons in amountsListQuickPlay)
        {
            if (int.Parse(buttons.GetComponentInChildren<TextMeshProUGUI>().text) == amount)
            {
                buttons.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                buttons.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        if (termsAndConditionsCheckQuickPlay.isOn == true && betAmount > 0)
        {
            proceedButtonQuickPlay.interactable = true;
        }
    }

    public void OnValueChange()
    {
        if (termsAndConditionsCheck.isOn == true && betAmount > 0)
        { 
            proceedButtonQuickPlay.interactable = true;
        }
        else
        {
            proceedButtonQuickPlay.interactable=false;
        }
    }

    IEnumerator MatchMakingScreenStart()
    {
        int seconds = 5;
        
        matchmakingScreen.SetActive(true);
        while (seconds > 0)
        {
            timerText.text = seconds.ToString();
            yield return new WaitForSeconds(1);
            seconds--;
        }
        sceneLoader.startGameScene();
    }
   
}
