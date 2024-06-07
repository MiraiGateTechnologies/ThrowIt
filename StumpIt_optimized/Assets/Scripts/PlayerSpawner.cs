using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Basic player spawn based on the main shared mode sample.
/// </summary>
public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            var resultingPlayer = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);

            FusionConnector connector = GameObject.FindObjectOfType<FusionConnector>();
            if (connector != null)
            {
                var testPlayer = resultingPlayer.GetComponent<FusionPlayer>();

                string playerName = connector.LocalPlayerName;

                if (string.IsNullOrEmpty(playerName))
                    testPlayer.PlayerName = "Player " + resultingPlayer.StateAuthority.PlayerId;
                else
                    testPlayer.PlayerName = playerName;

                // Assigns a random avatar
                testPlayer.ChosenAvatar = connector.currentAvatarIndex;
                testPlayer.Score = connector.scoreVariable;
                testPlayer.gameplayTimerNO = connector.gameplayTimerVariable;
                //testPlayer.timeToStart = connector.timerSeconds;
            }
        }
        else
        {
            
        }

        FusionConnector.Instance?.OnPlayerJoin(Runner);
    }

    public void PlayerLeft(PlayerRef player)
    {

        FusionConnector.Instance?.OnPlayerLeft(Runner);
        

        //if (FusionPlayer.LocalPlayer != null)
        //    FusionPlayer.LocalPlayer.IsMasterClient = Runner.IsSharedModeMasterClient;
    }
}
