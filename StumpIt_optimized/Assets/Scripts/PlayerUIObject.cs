using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUIObject : MonoBehaviour
{
    [SerializeField]
    Image playerProfile;
    [SerializeField]
    TextMeshProUGUI playerName;
    // Start is called before the first frame update
    public void SetSpriteValue(int index)
    {
        this.playerProfile.sprite = FusionConnector.Instance.avatarSprites[index];
    }

    public void SetPlayerName(string playerName) 
    {
        this.playerName.text = playerName;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
