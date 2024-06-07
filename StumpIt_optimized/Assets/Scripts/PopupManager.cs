using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static string awesomeText = "Awesome";
    public static string outstandingText = "Outstanding";
    public static string amazingText = "Amazing";
    public static string bullsEyeText = "Bulls-Eye";
    public static string insaneText = "Insane";


    public static string twoScoreText = "2 Score Bonus";
    public static string threeScoreText = "3 Score Bonus";
    public static string fourScoreText = "4 Score Bonus";

    public string GetRandomPopupMessage()
    {
        // Adding all static texts to a list
        List<string> messages = new List<string>
        {
            awesomeText,
            outstandingText,
            amazingText,
            bullsEyeText,
            insaneText
        };

        // Randomly select a message
        int index = Random.Range(0, messages.Count);
        return messages[index];
    }

}
