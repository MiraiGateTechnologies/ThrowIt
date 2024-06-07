using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Include this if you are displaying the number in a UI Text component

public class NumberRunner : MonoBehaviour
{
    public float duration = 2.0f; 
    private int currentNumber = 0; 

   

    public void StartRunNumber(int targetNumber,TextMeshProUGUI numberText)
    {
        StartCoroutine(RunNumber(targetNumber,numberText));
    }

    IEnumerator RunNumber(int targetNumber, TextMeshProUGUI numberText)
    {
        float elapsedTime = 0;
        int startNumber = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentNumber = Mathf.RoundToInt(Mathf.Lerp(startNumber, targetNumber, elapsedTime / duration));
            if (numberText != null)
            {
                numberText.text = currentNumber.ToString();
            }
            yield return null; 
        }
        numberText.GetComponent<Animator>().SetTrigger("zoom");
        // Ensure the final value is set correctly
        currentNumber = targetNumber;
        if (numberText != null)
        {
            numberText.text = currentNumber.ToString();
        }
    }
}
