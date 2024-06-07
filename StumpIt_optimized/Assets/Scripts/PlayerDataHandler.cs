using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public Sprite playerProfileSprite;
    public int amount;

    public PlayerData(string name, Sprite profileSprite, int startingAmount)
    {
        playerName = name;
        playerProfileSprite = profileSprite;
        amount = startingAmount;
    }
}

public class PlayerDataHandler : MonoBehaviour
{
    public static PlayerDataHandler Instance;
    public PlayerData playerData;

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
        
    }
    
    public void UpdatePlayerData(string newName, Sprite newProfileSprite, int newAmount)
    {
        playerData.playerName = newName;
        playerData.playerProfileSprite = newProfileSprite;
        playerData.amount = newAmount;
    }

    public void DisplayPlayerData()
    {
        //Debug.Log("Player Name: " + playerData.playerName);
        //Debug.Log("Player Profile Sprite: " + playerData.playerProfileSprite);
        //Debug.Log("Amount: " + playerData.amount);
    }
}
