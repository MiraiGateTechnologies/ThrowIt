using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FusionConnector : MonoBehaviour, INetworkRunnerCallbacks
{
   
    public string LocalPlayerName { get; set; }
    public bool isBotPlayer;
    public string botPlayerName;

    public bool isSimulater;
    public string simulaterName;
    public Sprite simulatorAvatar;
    public int simulaterFinalScore;
    public GameObject simulaterScorePanel;
    public TextMeshProUGUI simulaterScoreText;

    private const float gameplayFeePercentage = 0.10f;

    public GameObject searchingObject;
    public int currentAvatarIndex = 0;
    public int timerSeconds = 0;

    public Transform player1UIObj, player2UIObj;
    public Transform player1ScoreObj, player2ScoreObj;

    public FusionPlayer player1Player;
    public FusionPlayer player2Player;

    public TextMeshProUGUI waitingforSecondsPlayerTimerText;
    public Button restartGame;
    public int connectedPlayerCount=0;
    public string LocalRoomName { get; set; }
    public bool isRoomOpen { get; set; } = true;

    public int maxPlayerCount { get; set; } = 2;

    [Tooltip("The sprites used to render the character")]
    public Sprite[] avatarSprites;

    [SerializeField, Tooltip("The network runner prefab that will be instantiated when looking starting the game.")]
    private NetworkRunner _networkRunnerPrefab;

    [Tooltip("The canvas group that handles interactivity for the game.")]
    public CanvasGroup canvasGroup;

    [Tooltip("The GameObject that contains the main menu.")]
    public GameObject mainMenuObject;

    [Tooltip("The Game Object that handles the game itself")]
    public GameObject mainGameObject;

    [Tooltip("GameObject that appears if there is a network error when trying to join a room.")]
    public GameObject errorMessageObject;

    [Tooltip("The GameObject that displays the button to start the game.")]
    public GameObject showGameButton;

    [Tooltip("Text object that displays the room name.")]
    public TextMeshProUGUI roomName;

    [Tooltip("Prefab for the fusion game itself.")]
    public NetworkObject fusionGamePrefab;

    //public Transform playerContainer;
    public int scoreVariable;
    public int gameplayTimerVariable;

    [Tooltip("The message shown before starting the game.")]
    public TextMeshProUGUI preGameMessage;
    public Transform playerContainer;
    
    public TextMeshProUGUI player1Name;
    public Image profile1;

    public Image profile2;
    public TextMeshProUGUI player2Name;
    public Button prevButton, nextButton;
    public Button okButton;
    public TMP_InputField playerNameUI;
    public int waitForPlayerJoiningSeconds = 30;
    public int gameplaySeconds = 60;

    public Button ingameBackButton;
    public Animator inGameBackButtonAnimator;
    public TextMeshProUGUI gameplayTimerUI;

    public GameObject leavePopUp;
    public static FusionConnector Instance { get; private set; }
    #region BetAmount
    [Header("Bet Amount")]
    //public int betAmount;
    
    public List<Button> betAmountbuttonList;
    public GameType currentGameType;

    public enum GameType : int
    {
        two = 2,
        five = 5,
        ten = 10,
        tweenty = 20,
        fifty = 50,
        eighty = 80,
        oneHundred = 100,
        twoHundred = 200,
        fiveHundred = 500
    }

    #endregion

    #region Result
    [Header("Result Page")]
    public GameObject gameOverPanel;
    public GameObject scorePanel;
    public TextMeshProUGUI resultTextUI;
    public TextMeshProUGUI player1ResultNameTextUI;
    public TextMeshProUGUI player2ResultNameTextUI;

    public TextMeshProUGUI player1ResultScoreTextUI;
    public TextMeshProUGUI player2ResultScoreTextUI;

    public Button playAgain;

    bool gameOverBool = false;

    #endregion
    public NetworkRunner runnerObject;

    public BotNames botNames;
    public int botFinaScore;
    public int botCurrentScore;
    public bool isForFeit;
    int botTimer;
    int[] currentValueArrayfor2To20 = { 1, 2, 3, 5 };
    int[] currentValueArrayfor50Andabove = { 3, 5, 9,10,12 };
    Coroutine waitingOpponentPlayerCoroutine;
    Coroutine gamePlayTimerCoroutine;
    Coroutine updateBotScoreCoroutine;
    Coroutine gameOverCoroutine;

    float updateScoreWaitTimer;
    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
       
        //DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        //PlayerPrefs.DeleteAll();
        Instance = null;
    }

    void Start()
    {
        //if (avatarSprites.Length > 0)
        //{
        //    profile1.sprite = avatarSprites[currentAvatarIndex];
        //}
        //ResetValues();
        simulaterScorePanel.SetActive(false);
        if (PlayerPrefs.HasKey("BetAmount"))
        {
            currentGameType = (GameType)PlayerPrefs.GetInt("BetAmount");
            int currentAvtIndex = PlayerPrefs.GetInt("ProfileSpriteIndex");
            string playerName = PlayerPrefs.GetString("ProfileName");

            MenuUiManager.Instance.player1Profile.sprite = avatarSprites[currentAvtIndex];
            MenuUiManager.Instance.player1Name.text = playerName;
            MenuUiManager.Instance.matchTitle.text = "quick play";
            if (PlayerPrefs.HasKey("PlayAgain"))
            {
                PlayerPrefs.DeleteKey("PlayAgain");
                MenuUiManager.Instance.loadingScreen.SetActive(true);
                SetMatchValues((int)currentGameType, playerName, currentAvtIndex);
                StartFusionGame();
            }
            else 
            {
                MenuUiManager.Instance.player1Profile.sprite = avatarSprites[0];
                MenuUiManager.Instance.player1Name.text = "";
                MenuUiManager.Instance.matchTitle.text = "quick play";
                PlayerPrefs.DeleteAll();
            }
            

        }
    }

    #region PracticeMade

    public void OnPracticeMatchClicked(int index)
    {
        
        isSimulater = true;
        profile1.sprite = avatarSprites[PlayerPrefs.GetInt("ProfileSpriteIndex")];
        player1Name.text = PlayerPrefs.GetString("ProfileName");
        simulaterName = "Simulator";
        profile2.sprite = simulatorAvatar;

        switch (index) 
        {
            case 0:
                simulaterFinalScore = UnityEngine.Random.Range(80, 160);
                break;
            case 1:
                simulaterFinalScore = UnityEngine.Random.Range(120, 180);
                break;
            case 2:
                simulaterFinalScore = UnityEngine.Random.Range(140, 250);
                break;
            case 3:

                MenuUiManager.Instance.practicePopup.SetActive(false);
                MenuUiManager.Instance.blurBG.gameObject.SetActive(false);
                break;
        }
        if (index == 3)
            return;
        //waitingOpponentPlayerCoroutine = StartCoroutine(MatchMakingScreenStart());
        FusionConnector.Instance.searchingObject.SetActive(false);
        waitForPlayerJoiningSeconds = 5;
        GoToGame();
        waitingOpponentPlayerCoroutine = StartCoroutine(MatchMakingScreenStart());
        simulaterScorePanel.SetActive(true);
        simulaterScorePanel.GetComponent<NumberRunner>().StartRunNumber(simulaterFinalScore, simulaterScoreText);
        SetPregameMessage("Starting Game...");
        connectedPlayerCount = 2;

    }

    #endregion



    public void SetMatchValues(int betAmount,string playerName, int profileIndex)
    {
        SetBetAmount(betAmount);
        currentAvatarIndex  = profileIndex;
        LocalPlayerName = playerName;
    }

    public void SetBetAmount(int betAmount)
    {
        Debug.Log("BetAmont ::: "+betAmount);
        currentGameType = (GameType)betAmount;
        Debug.Log("currentGameType ::: " + currentGameType);
        if (!PlayerPrefs.HasKey("BetAmount"))
            PlayerPrefs.SetInt("BetAmount", betAmount);
        for (int i = 0; i < betAmountbuttonList.Count; i++)
        {
            betAmountbuttonList[i].interactable = false;
        }
        switch (currentGameType) 
        {
            case GameType.two:
                botFinaScore = UnityEngine.Random.Range(20,50);
                updateScoreWaitTimer = 3f;
                break;
            case GameType.five:
                botFinaScore = UnityEngine.Random.Range(100, 150);
                updateScoreWaitTimer = 2.9f;
                break;
            case GameType.ten:
                botFinaScore = UnityEngine.Random.Range(120, 170);
                updateScoreWaitTimer = 2.6f;

                break;
            case GameType.tweenty:
                botFinaScore = UnityEngine.Random.Range(130, 180);
                updateScoreWaitTimer = 2.5f;
                break;
            case GameType.fifty:
            case GameType.eighty:
            case GameType.oneHundred:
            case GameType.twoHundred:
            case GameType.fiveHundred:
                botFinaScore = UnityEngine.Random.Range(140, 250);
                updateScoreWaitTimer = 2.1f;
                break;
            

        }
        Debug.Log("Bot Final Score ::: "+botFinaScore);

    }

    public void UpdateScore(int localScore)
    {
        //scoreVariable += localScore;
        if(isBotPlayer == false && isSimulater == false)    
        player1Player.Score = localScore;
       
    }

    public void OnInGameBackButton()
    {
        inGameBackButtonAnimator.SetTrigger("ExitIn");
        ingameBackButton.interactable = false;
    }
    public void OnInGameCloseButton()
    {
        inGameBackButtonAnimator.SetTrigger("ExitOut");
        ingameBackButton.interactable = true;
        if (runnerObject != null)
        {
            //GameManager.Instance.audioManager.StopCrowedSound();

            runnerObject.UnloadScene(SceneRef.FromIndex(0));
            runnerObject.Shutdown(true);
        }
    }
    public void OnClickLeave()
    {
        leavePopUp.SetActive(true);
    }

    public void OnClickPopUpQuit()
    {
        isForFeit = true;
        inGameBackButtonAnimator.SetTrigger("ExitOut");
        ingameBackButton.interactable = true;
        StopAllCoroutines();
        gameplaySeconds = 0;

        GameManager.Instance.audioManager.StopCrowedSound();

        if (player1Player != null)
        {
            resultTextUI.text = "You Lose";
            player2ResultScoreTextUI.transform.parent.SetAsFirstSibling();
            player1ResultScoreTextUI.text = player1Player.Score.ToString();
            player2ResultScoreTextUI.text = player2Player.Score.ToString();

        }
        else if (isBotPlayer)
        {
            resultTextUI.text = "You Lose";
            player2ResultScoreTextUI.transform.parent.SetAsFirstSibling();
            player1ResultNameTextUI.text = MenuUiManager.Instance.playerName.ToString();
            player2ResultNameTextUI.text = botPlayerName.ToString();
            player1ResultScoreTextUI.text = GameManager.Instance.scoreManager.score.ToString();
            player2ResultScoreTextUI.text = (GameManager.Instance.scoreManager.score+ UnityEngine.Random.Range(5,20)).ToString();
        }
        else if (isSimulater)
        {
                resultTextUI.text = "You Lose";
                player2ResultScoreTextUI.transform.parent.SetAsFirstSibling();
                player1ResultNameTextUI.text = MenuUiManager.Instance.playerName;
                player2ResultNameTextUI.text = simulaterName.ToString();
                player1ResultScoreTextUI.text = GameManager.Instance.scoreManager.score.ToString();
                player2ResultScoreTextUI.text = simulaterFinalScore.ToString();
        }

        gameOverBool = false;
        scorePanel.SetActive(true);
        playAgain.interactable = true;

        if (runnerObject != null)
        {

            runnerObject.UnloadScene(SceneRef.FromIndex(0));
            runnerObject.Shutdown(true);
        }
        
        leavePopUp.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void OnClickRestart()
    {
        if (runnerObject != null)
        {
            //GameManager.Instance.audioManager.StopCrowedSound();

            runnerObject.UnloadScene(SceneRef.FromIndex(0));
            runnerObject.Shutdown(true);
        }
        OnClickOK();
    }
    public void OnClickOK()
    {
        
        SceneManager.LoadScene(0);
    }
    IEnumerator MatchMakingScreenStart()
    {
        //GameManager.Instance.InitGame();
        
        //MenuUiManager.Instance.inGameScreen.SetActive(true);

        int temp = UnityEngine.Random.Range(20, 25);

        botTimer = waitForPlayerJoiningSeconds - temp;
        Debug.Log("temp ::" + temp + "botTimer ::" + botTimer);
        while (waitForPlayerJoiningSeconds >= 0)
        {
            waitingforSecondsPlayerTimerText.text = waitForPlayerJoiningSeconds.ToString();
           
            yield return new WaitForSeconds(1);
            waitForPlayerJoiningSeconds--;

            if (connectedPlayerCount < maxPlayerCount && waitForPlayerJoiningSeconds == botTimer)
                GenerateBot();
        }

        if (connectedPlayerCount == maxPlayerCount)
        {
            MenuUiManager.Instance.matchmakingScreen.SetActive(false);
            MenuUiManager.Instance.inGameScreen.SetActive(true);

            if (isBotPlayer == false && isSimulater == false)
            {
                player1Player.nameText.gameObject.SetActive(false);
                player2Player.nameText.gameObject.SetActive(false);

                player1Player.avatarRenderer.gameObject.SetActive(false);
                player2Player.avatarRenderer.gameObject.SetActive(false);
            }
            player1ScoreObj = GameObject.FindGameObjectWithTag("Player1Score").transform;
            player2ScoreObj = GameObject.FindGameObjectWithTag("Player2Score").transform;

            if (isBotPlayer == false && isSimulater == false)
            {
                player1Player.transform.SetParent(player1ScoreObj, false);
                player2Player.transform.SetParent(player2ScoreObj, false);
            }

            waitingforSecondsPlayerTimerText.gameObject.SetActive(false);
            


            //player1Player.scoreText.gameObject.SetActive(true);
            //player2Player.scoreText.gameObject.SetActive(true);
            if (isBotPlayer == false && isSimulater == false)
            {
                player1Player.timerText.gameObject.SetActive(false);
                player2Player.timerText.gameObject.SetActive(false);
            }
            gamePlayTimerCoroutine = StartCoroutine(GameplayTimerCO());


            player1ScoreObj.GetComponent<TextMeshProUGUI>().text = "0";
            player2ScoreObj.GetComponent<TextMeshProUGUI>().text = "0";

            SetPregameMessage("");
            if(isBotPlayer)
                updateBotScoreCoroutine = StartCoroutine(UpdateBotScore());
            StopCoroutine(waitingOpponentPlayerCoroutine);
        }
        else
        {
            waitForPlayerJoiningSeconds = 30;
            StopCoroutine(waitingOpponentPlayerCoroutine);
            waitingOpponentPlayerCoroutine = StartCoroutine(MatchMakingScreenStart());
            SetPregameMessage("Players are busy... Searching again...");
        }
    }
    private void OnApplicationQuit()
    {
        if(isBotPlayer == false && isSimulater == false)
        {
            if (runnerObject != null)
            {
                runnerObject.UnloadScene(SceneRef.FromIndex(0));
                runnerObject.Shutdown(true);
            }

        }
        
        PlayerPrefs.DeleteAll();
    }
    private void OnDisable()
    {
        if (isBotPlayer == false && isSimulater == false)
        {
            if (runnerObject != null)
            {
                runnerObject.UnloadScene(SceneRef.FromIndex(0));
                runnerObject.Shutdown(true);
            }

        }

        //PlayerPrefs.DeleteAll();
    }
    private void GenerateBot()
    {
        runnerObject.UnloadScene(SceneRef.FromIndex(0));
        runnerObject.Shutdown(true);
        isBotPlayer = true;
        LoadNamesFromResources();
        UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
        string currentBotName = botNames.names[UnityEngine.Random.Range(0, botNames.names.Count)];
        botPlayerName = currentBotName;
        Debug.Log("currentBotName ::: "+currentBotName);
        Sprite currentBotSprite = avatarSprites[UnityEngine.Random.Range(0, avatarSprites.Length)];
        profile2.sprite = currentBotSprite;
        player2Name.text = currentBotName;
        FusionConnector.Instance.searchingObject.SetActive(false);
        waitForPlayerJoiningSeconds = 5;
        SetPregameMessage("Starting Game...");
        connectedPlayerCount += 1;

    }
    private void LoadNamesFromResources()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("names");

        if (jsonFile != null)
        {
            botNames = JsonUtility.FromJson<BotNames>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Failed to load JSON file from Resources folder.");
        }
    }
    IEnumerator GameplayTimerCO()
    {
        gameplayTimerUI.gameObject.SetActive(true);
        GameManager.Instance.InitGame();
        playAgain.interactable = false;
        scorePanel.gameObject.SetActive(false);
        while (gameplaySeconds >= 0)
        {
            yield return new WaitForSeconds(1f);
            yield return new WaitForEndOfFrame();
            gameplaySeconds--;
            gameplayTimerUI.text = FormatTime(gameplaySeconds);//.ToString();

            Debug.Log("gameplaySeconds ::: ............................." + gameplaySeconds);
                
            
            if (isBotPlayer == false && isSimulater == false)
            {
                player1Name.GetComponent<TextMeshProUGUI>().text = player1Player.PlayerName.ToString();
                player2Name.GetComponent<TextMeshProUGUI>().text = player2Player.PlayerName.ToString();


                player1ScoreObj.GetComponent<TextMeshProUGUI>().text = player1Player.Score.ToString();
                player2ScoreObj.GetComponent<TextMeshProUGUI>().text = player2Player.Score.ToString();
            }
            else if (isBotPlayer)
            {
                   
                player2ScoreObj.GetComponent<TextMeshProUGUI>().text = botCurrentScore.ToString();
                Debug.Log("Set Player 2 Score using Stratergy");
            }
            else if(isSimulater)
            {
                player2ScoreObj.GetComponent<TextMeshProUGUI>().text = simulaterFinalScore.ToString();
            }
            
            //
        }
        if (isBotPlayer == false && isSimulater == false)
        {
            player1Player.gameplayTimerNO = gameplaySeconds;
        }
        //gameplayTimerUI.gameObject.SetActive(false);
        StopCoroutine(gamePlayTimerCoroutine);
        if(isBotPlayer)
            StopCoroutine(updateBotScoreCoroutine);
        gameOverCoroutine = StartCoroutine(GameOverWaitingTimerCO());
       
    }

    IEnumerator UpdateBotScore()
    {
        int currentShotValue = 0;
        while (true)
        {
            yield return new WaitForSeconds(updateScoreWaitTimer);

            if (botCurrentScore < botFinaScore)
            {
                if ((int)currentGameType >= 20 && (int)currentGameType < 50)
                {
                    currentShotValue = currentValueArrayfor2To20[UnityEngine.Random.Range(0, currentValueArrayfor2To20.Length)];
                }
                else
                {
                    currentShotValue = currentValueArrayfor50Andabove[UnityEngine.Random.Range(0, currentValueArrayfor2To20.Length)];

                }

                botCurrentScore += currentShotValue;
            }
        }

    }

    IEnumerator GameOverWaitingTimerCO()
    {
        gameOverBool = true;
        gameOverPanel.SetActive(true);
        while (gameOverBool)
        {
           
            
            resultTextUI.text = "waiting";
            GameManager.Instance.audioManager.StopCrowedSound();
           
            float winnings = ((int)currentGameType) * 2;
            float fee = winnings * gameplayFeePercentage;
            float netWinnings = winnings - fee;
            yield return new WaitForSeconds(0.01f);
            if (isBotPlayer == false && isSimulater == false)
            {
                if (player1Player.gameplayTimerNO <= 0)
                {
                    

                    player1ResultNameTextUI.text = player1Player.PlayerName.ToString();
                    player2ResultNameTextUI.text = player2Player.PlayerName.ToString();
                    player1ResultScoreTextUI.text = player1Player.Score.ToString();
                    player2ResultScoreTextUI.text = "...";

                }
                else
                {
                    player1ResultNameTextUI.text = player1Player.PlayerName.ToString();
                    player2ResultNameTextUI.text = player2Player.PlayerName.ToString();
                    player1ResultScoreTextUI.text = "...";
                    player2ResultScoreTextUI.text = player2Player.Score.ToString();
                }
                if (player1Player.gameplayTimerNO <= 0 && player2Player.gameplayTimerNO <= 0)
                {
                    gameOverBool = false;
                    
                    if (player1Player.Score > player2Player.Score)
                    {
                        resultTextUI.text = "You Win";

                        player1ResultScoreTextUI.transform.parent.SetAsFirstSibling();
                        player1ResultScoreTextUI.text = player1Player.Score.ToString();
                        player2ResultScoreTextUI.text = player2Player.Score.ToString();
                        ToastNotification.instance.ShowToast("You won amount of Rs. " + netWinnings, 3.0f);
                    }
                    else
                    {
                        resultTextUI.text = "You Lose";
                        player2ResultScoreTextUI.transform.parent.SetAsFirstSibling();
                        player1ResultScoreTextUI.text = player1Player.Score.ToString();
                        player2ResultScoreTextUI.text = player2Player.Score.ToString();
                    }
                   
                    Debug.Log("Start Coroutine for Result......");
                }
                scorePanel.SetActive(true);
                playAgain.interactable = true;
            }
            else if (isBotPlayer)
            {
                gameOverBool = false;
              


                if (GameManager.Instance.scoreManager.score > botCurrentScore)
                {
                    resultTextUI.text = "You Win";
                    player1ResultScoreTextUI.transform.parent.SetAsFirstSibling();
                    player1ResultNameTextUI.text = MenuUiManager.Instance.playerName.ToString();
                    player2ResultNameTextUI.text = botPlayerName.ToString();
                    player1ResultScoreTextUI.text = GameManager.Instance.scoreManager.score.ToString();
                    player2ResultScoreTextUI.text = botCurrentScore.ToString();

                    
                    ToastNotification.instance.ShowToast("You won amount of Rs. " + netWinnings, 3.0f);
                }
                else
                {
                    resultTextUI.text = "You Lose";
                    player2ResultScoreTextUI.transform.parent.SetAsFirstSibling();
                    player1ResultNameTextUI.text = MenuUiManager.Instance.playerName.ToString();
                    player2ResultNameTextUI.text = botPlayerName.ToString();
                    player1ResultScoreTextUI.text = GameManager.Instance.scoreManager.score.ToString();
                    player2ResultScoreTextUI.text = botCurrentScore.ToString();
                }
                scorePanel.SetActive(true);
                playAgain.interactable = true;
            }
            else 
            {
                gameOverBool = false;
                scorePanel.SetActive(true);
                playAgain.interactable = true;

                if (GameManager.Instance.scoreManager.score > simulaterFinalScore)
                {
                    resultTextUI.text = "You Win";
                    player1ResultScoreTextUI.transform.parent.SetAsFirstSibling();
                    player1ResultNameTextUI.text = MenuUiManager.Instance.playerName.ToString();
                    player2ResultNameTextUI.text = simulaterName.ToString();
                    player1ResultScoreTextUI.text = GameManager.Instance.scoreManager.score.ToString();
                    player2ResultScoreTextUI.text = simulaterFinalScore.ToString();

                }
                else
                {
                    resultTextUI.text = "You Lose";
                    player2ResultScoreTextUI.transform.parent.SetAsFirstSibling();
                    player1ResultNameTextUI.text = MenuUiManager.Instance.playerName;
                    player2ResultNameTextUI.text = simulaterName.ToString();
                    player1ResultScoreTextUI.text = GameManager.Instance.scoreManager.score.ToString();
                    player2ResultScoreTextUI.text = simulaterFinalScore.ToString();
                }
               

            }
        }

       

    }

    public void OnClickQuit()
    {
        isForFeit = false;
        PlayerPrefs.DeleteKey("BetAmount");
        if (isBotPlayer == false && isSimulater == false)
        {
            runnerObject.UnloadScene(SceneRef.FromIndex(0));
            runnerObject.Shutdown(true);
        }
       
        SceneManager.LoadScene(0);
    }

    public void OnPlayAgainClicked()
    {
        isForFeit = false;
        if(!PlayerPrefs.HasKey("PlayAgain"))
        {
            PlayerPrefs.SetInt("PlayAgain", 0);
        }
        if (isBotPlayer == false && isSimulater == false)
        {
            runnerObject.UnloadScene(SceneRef.FromIndex(0));
            runnerObject.Shutdown(true);
        }
        SceneManager.LoadScene(0);
    }
    public string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }
    public void GetPlayerNameUI()
    {
        string inputText = playerNameUI.text;
        LocalPlayerName = inputText;
        Debug.Log("Input Field Text: " + inputText);
    }
    public void NextSprite()
    {
        if (avatarSprites.Length == 0) return;

        currentAvatarIndex = (currentAvatarIndex + 1) % avatarSprites.Length;
        //profile1.sprite = avatarSprites[currentAvatarIndex];
    }

    public void PreviousSprite()
    {
        if (avatarSprites.Length == 0) return;

        currentAvatarIndex = (currentAvatarIndex - 1 + avatarSprites.Length) % avatarSprites.Length;
        //profile1.sprite = avatarSprites[currentAvatarIndex];
    }
    public async void StartGame(bool joinRandomRoom, GameType gameType)
    {
        canvasGroup.interactable = false;
        var customProps = new Dictionary<string, SessionProperty>() {
        { 
                "type", (int)gameType 
        }};

        StartGameArgs startGameArgs = new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = joinRandomRoom ? string.Empty : LocalRoomName,
            SessionProperties = customProps,
            PlayerCount = maxPlayerCount,
            IsOpen = isRoomOpen
        };

        NetworkRunner newRunner = Instantiate(_networkRunnerPrefab);

        StartGameResult result = await newRunner.StartGame(startGameArgs);

        if (result.Ok)
        {
            //roomName.text = "Room:  " + newRunner.SessionInfo.Name;

            GoToGame();
            waitingOpponentPlayerCoroutine = StartCoroutine(MatchMakingScreenStart());
            Debug.Log("Room Name::: "+LocalRoomName);
        }
        else
        {
            //roomName.text = string.Empty;

            GoToMainMenu();

            errorMessageObject.SetActive(true);
            TextMeshProUGUI gui = errorMessageObject.GetComponentInChildren<TextMeshProUGUI>();
            if (gui)
                gui.text = result.ErrorMessage;
            MenuUiManager.Instance.loadingScreen.SetActive(false);
            Debug.LogError(result.ErrorMessage);
        }

        canvasGroup.interactable = true;
    }

    public void GoToMainMenu()
    {
        mainMenuObject.SetActive(true);
        mainGameObject.SetActive(false);
    }

    public void GoToGame()
    {
        MenuUiManager.Instance.loadingScreen.SetActive(false);
        mainMenuObject.SetActive(false);
        mainGameObject.SetActive(true);

    }

    internal void OnPlayerJoin(NetworkRunner runner)
    {
        // Only set pregame messages if the game hasn't started.
        //if (FusionManager.FusionManagerPresent)
        //{
        //    return;
        //}
        runnerObject = runner;
        connectedPlayerCount += 1;
        if (maxPlayerCount == runner.SessionInfo.PlayerCount)
        {
            Debug.Log("Max Player Count Reached");
            runner.SessionInfo.IsOpen = false;
        }

        if (runner.IsSharedModeMasterClient == true)
        {
            SetPregameMessage("Waiting for Opponent to Join...");
        }
        else
        {

            //SetPregameMessage("Waiting for master client to start game.");
        }

        //IsOpen 
    }

    
    public void SetPregameMessage(string message)
    {
        preGameMessage.text = message;
    }

    public void StartFusionGame()
    {
        //NetworkRunner runner = null;
        //// If no runner has been assigned, we cannot start the game
        //if (NetworkRunner.Instances.Count > 0)
        //{
        //    runner = NetworkRunner.Instances[0];
        //}

        //if (runner == null)
        //{
        //    Debug.Log("No runner found.");
        //    return;
        //}

        StartGame(true, currentGameType);
        // If no trivia manager has been made and we are the master mode client.
        // Redundant but being safe.
        //if (runner.IsSharedModeMasterClient && !FusionManager.FusionManagerPresent)
        //{
        //    runner.Spawn(fusionGamePrefab);
        //    //showGameButton.SetActive(false);
        //}

    }
    internal void OnPlayerLeft(NetworkRunner runner)
    {
        errorMessageObject.SetActive(true);
        TextMeshProUGUI gui = errorMessageObject.GetComponentInChildren<TextMeshProUGUI>();
        if (gui)
        {
            if (gameOverPanel.activeSelf == false)
            {
                gui.text = "Plyer Left You Won...";
            }
            else
            {
                gui.text = "Other Plyer Left...";
            }
        }

        MenuUiManager.Instance.loadingScreen.SetActive(false);
        //if (isBotPlayer == false && isSimulater == false)
        //GameManager.Instance.audioManager.StopCrowedSound();
        //player.
        //ResetValues();
        Debug.Log("Plyer Left You Won...");
        
        runner.UnloadScene(SceneRef.FromIndex(0));
        runner.Shutdown(true);

    }
    #region fusionCallbacks
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.LogWarning("OnObjectExitAOI");
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.LogWarning("OnPlayerJoined");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.LogError("OnPlayerLeft");
        //if(runner.LocalPlayer.get)
        //errorMessageObject.SetActive(true);
        //TextMeshProUGUI gui = errorMessageObject.GetComponentInChildren<TextMeshProUGUI>();
        //if (gui)
        //    gui.text = "Plyer Left You Won...";

        //Debug.Log("Plyer Left You Won...");
        //runner.Shutdown(true);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.LogWarning("OnShutdown");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        errorMessageObject.SetActive(true);
        TextMeshProUGUI gui = FusionConnector.Instance.errorMessageObject.GetComponentInChildren<TextMeshProUGUI>();
        if (gui)
            gui.text = "Disconnected From Server";

        MenuUiManager.Instance.loadingScreen.SetActive(false);
        runner.Shutdown(true);
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        errorMessageObject.SetActive(true);
        TextMeshProUGUI gui = FusionConnector.Instance.errorMessageObject.GetComponentInChildren<TextMeshProUGUI>();
        if (gui)
            gui.text = "Connect Failed";

        MenuUiManager.Instance.loadingScreen.SetActive(false);
        runner.Shutdown(true);
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
#endregion
    
}
[Serializable]
public class BotNames
{
    public List<string> names;
}