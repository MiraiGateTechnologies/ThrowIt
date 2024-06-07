using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FusionPlayer : NetworkBehaviour
{
    [Tooltip("The name of the player")]
    [Networked, OnChangedRender(nameof(OnPlayerNameChanged))]
    public NetworkString<_16> PlayerName { get; set; }

    [Tooltip("Which character has the player chosen.")]
    [Networked, OnChangedRender(nameof(OnAvatarChanged))]
    public int ChosenAvatar { get; set; } = 0;

    [Tooltip("Time to start Match.")]
    [Networked, OnChangedRender(nameof(OnGamePlayTimerChanged))]
    public int gameplayTimerNO { get; set; } = 60;

    [Tooltip("What is the player's score.")]
    [Networked, OnChangedRender(nameof(OnScoreChanged))]
    public int Score { get; set; } = 0;

    public static FusionPlayer LocalPlayer;
    public static List<FusionPlayer> FusionPlayerRefs = new List<FusionPlayer>();

    
   [Tooltip("Reference to the name display object.")]
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI timerText;

    [Tooltip("Reference to the score display object.")]
    public TextMeshProUGUI scoreText;

    [Tooltip("Reference to the avatars a player can use.")]
    public Image avatarRenderer;

    [Tooltip("The sprites used to render the character")]
    public Sprite[] avatarSprites;
    
    
    // Start is called before the first frame update

    public override void Spawned()
    {
        base.Spawned();

        // Adds this player to a list of player refs and then sorts the order by index
        //FusionPlayerRefs.Add(this);
        //FusionPlayerRefs.Sort((x, y) => x.Object.StateAuthority.AsIndex - y.Object.StateAuthority.AsIndex);

        // The OnRenderChanged functions are called during spawn to make sure they are set properly for players who have already joined the room.
        OnScoreChanged();
        OnPlayerNameChanged();
        OnAvatarChanged();
        OnGamePlayTimerChanged();
        avatarRenderer.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        // We assign the local test player a different sprite
        if (Object.HasStateAuthority == true)
        {
            LocalPlayer = this;
            transform.SetParent(FusionConnector.Instance.player1UIObj, false);
            //FusionConnector.Instance.profile1.sprite = avatarRenderer.sprite;
            FusionConnector.Instance.player1Player = transform.GetComponent<FusionPlayer>();
            
        }
        else
        {
            transform.SetParent(FusionConnector.Instance.player2UIObj, false);
            FusionConnector.Instance.searchingObject.SetActive(false);
            FusionConnector.Instance.SetPregameMessage("Starting Game...");
            FusionConnector.Instance.waitForPlayerJoiningSeconds = 5;// FusionConnector.Instance.seconds;
            FusionConnector.Instance.profile2.sprite = avatarRenderer.sprite;
            FusionConnector.Instance.player2Name.text = PlayerName.Value; 
            FusionConnector.Instance.player2Player = transform.GetComponent<FusionPlayer>();
            
            //StartCoroutine(CO_WaitingTimer());
        }
        //FusionConnector.Instance.timerSeconds = FusionConnector.Instance.seconds;
        //seconds = 30;
        //StartCoroutine(CO_WaitingTimer());

        // Sets the master client value on spawn
        if (HasStateAuthority)
        {
            //IsMasterClient = Runner.IsSharedModeMasterClient;
        }
        //masterClientIcon.enabled = IsMasterClient;

        //OnMuteChanged();

        // Hides the avatar selector on spawn
        //avatarSelectableSpriteGameObject.gameObject.SetActive(false);

        // We show the "Start Game Button" for the master client only, regardless of the number of players in the room.
        //bool showGameButton = Runner.IsSharedModeMasterClient && TriviaManager.TriviaManagerPresent == false;
        //FusionConnector.Instance.showGameButton.SetActive(showGameButton);
    }

    void OnPlayerNameChanged()
    {
        nameText.text = PlayerName.Value;
    }

    void OnAvatarChanged()
    {
        // Sets which avatar face and expression to choose
        if (ChosenAvatar >= 0)
            avatarRenderer.sprite = avatarSprites[ChosenAvatar];
        else
            avatarRenderer.sprite = null;
    }
    void OnScoreChanged()
    {
        Debug.Log("Score :::"+Score.ToString());
        scoreText.text = Score.ToString();
    }
    //public int seconds = 30;
    //IEnumerator CO_WaitingTimer()
    //{
    //    while (seconds > 0)
    //    {
    //        //timerText.text = seconds.ToString();
    //        yield return new WaitForSeconds(1);
    //        seconds--;
    //        OnTimerChanged();

    //    }
    //}
    void OnGamePlayTimerChanged()
    {
        timerText.text = gameplayTimerNO.ToString();
    }

    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //public void ShowTimer(int timer)
    //{
    //    // The code inside here will run on the client which owns this object (has state and input authority).
    //    Debug.Log("Received ShowTimer on StateAuthority, modifying Networked variable");
    //    timeToStart = timer;
    //}

    
}
