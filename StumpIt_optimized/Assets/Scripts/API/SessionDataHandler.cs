using System;
using System.Xml.Serialization;
using UnityEngine;
using System;
public class SessionDataHandler : MonoBehaviour
{
    public static SessionDataHandler Instance;
    public SessionResponseData sessionResponseData;
    public GameStartedData gameStartData;
    public UpdatedScoreData updatedScoreData;
    public MatchResult matchResult;

    public Action sessionCreatedSucssesfully;
    
    private APIManager<SessionResponseData> sessionResponseDataAPI;
    private APIManager<GameStartedData> gameStartAPI;
    private APIManager<UpdatedScoreData> scoreAPI;
    private APIManager<MatchResult> matchEndAPI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the GameObject persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any additional instances
        }
    }
    private void Start()
    {
        // Create an instance of APIManager with the base URL of your API
        sessionResponseDataAPI = new APIManager<SessionResponseData>();
        gameStartAPI = new APIManager<GameStartedData>();
        scoreAPI = new APIManager<UpdatedScoreData>();
        matchEndAPI = new APIManager<MatchResult>();

    }

    public void CreateSession(string playerName,int betAmount)
    {
        SessionRequest request = new SessionRequest(playerName,betAmount);
        //request.playerId = playerName;
        //request.roomAmount = betAmount;
        // Start a coroutine to send the POST request
        sessionResponseDataAPI.Post(EndpointConstants.Session, request, OnCreateSessionSuccess, OnError);
    }

    private void StartGame(GameStartData request)
    {
        // Start a coroutine to send the POST request
        gameStartAPI.Post(EndpointConstants.GameStart, request, OnGameStarted, OnError);
    }
    private void SendScore(ScoreRequestAPI request)
    {
        // Start a coroutine to send the POST request
        scoreAPI.Post(EndpointConstants.Score, request, OnScoreUpdated, OnError);
    }

    private void MatchEnd(MatchEndAPI request)
    {
        // Start a coroutine to send the POST request
        matchEndAPI.Post(EndpointConstants.MatchEnd, request, OnMatchFinished, OnError);
    }

   




    // Method to handle successful creation of a session
    private void OnCreateSessionSuccess(SessionResponseData responseData)
    {
        sessionResponseData = responseData;
        Debug.Log(responseData.player1Id);
        sessionCreatedSucssesfully?.Invoke();
    }
    private void OnGameStarted(GameStartedData data)
    {
        gameStartData = data;
    }
    private void OnScoreUpdated(UpdatedScoreData data)
    {
        updatedScoreData = data;
    }

    private void OnMatchFinished(MatchResult result)
    {
        matchResult = result;
    }

    // Method to handle errors
    private void OnError(string errorMessage)
    {
        // Handle the error
        Debug.LogError("Error: " + errorMessage);
    }
}

[Serializable]
public class RematchRequest
{
    public string sessionId;
    public string playerId;
}

[Serializable]
public class MatchResult
{
    public bool success;
    public MatchResultData data;
}

[Serializable]
public class MatchResultData
{
    public string _id;
    public string player1Id;
    public bool isSessionExpire;
    public string sessionId;
    public int roomAmount;
    public bool isSessionCapacityFull;
    public MatchData[] match;
    public string createdOn;
    public string updatedOn;
    public string createdAt;
    public string updatedAt;
    public int __v;
    public string player2Id;
}

[Serializable]
public class MatchData
{
    public string matchId;
    public bool isMatchOpen;
    public int[] player1Score;
    public int[] player2Score;
    public int player1FinalScore;
    public int player2FinalScore;
    public string _id;
    public string winner;
}
[Serializable]
public class MatchEndAPI
{
    public string matchId;
}
[Serializable]
public class UpdatedScoreData
{
    public bool success;
    public UpdateScoreApiData data;
}

[Serializable]
public class UpdateScoreApiData
{
    public string _id;
    public string player1Id;
    public bool isSessionExpire;
    public string sessionId;
    public int roomAmount;
    public bool isSessionCapacityFull;
    public MatchData[] match;
    public string createdOn;
    public string updatedOn;
    public string createdAt;
    public string updatedAt;
    public int __v;
    public string player2Id;
}




[System.Serializable]
public class ScoreRequestAPI
{
    public string matchId;
    public int player1Score;
    public int player2Score;
}


[Serializable]
public class GameStartedData
{
    public bool success;
    public StartGameData data;
}

[Serializable]
public class StartGameData
{
    public string _id;
    public string player1Id;
    public bool isSessionExpire;
    public string sessionId;
    public int roomAmount;
    public bool isSessionCapacityFull;
    public MatchData[] match;
    public string createdOn;
    public string updatedOn;
    public string createdAt;
    public string updatedAt;
    public int __v;
    public string player2Id;
}


[System.Serializable]
public class GameStartData
{
    public string sessionId;

   
}
[System.Serializable]
public class SessionRequest
{
    public string playerId;
    public int roomAmount;

    public SessionRequest(string playerId, int roomAmount)
    {
        this.playerId = playerId;
        this.roomAmount = roomAmount;
    }
}


[System.Serializable]
public class SessionResponseData
{
    public string _id;
    public string player1Id;
    public bool isSessionExpire;
    public string sessionId;
    public int roomAmount;
    public bool isSessionCapacityFull;
    public string[] match;
    public string createdOn;
    public string updatedOn;
    public string createdAt;
    public string updatedAt;
    public int __v;
    public string player2Id;
}

[System.Serializable]
public class SessionResponse
{
    public bool success;
    public SessionResponseData data;
}
