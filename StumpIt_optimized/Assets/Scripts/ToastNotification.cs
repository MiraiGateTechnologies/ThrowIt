using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastNotification : MonoBehaviour
{
    public GameObject toastPanel; 
    public TextMeshProUGUI toastText; 
    public Animator toastAnimator;


    public static ToastNotification instance;

    Coroutine toastCO;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        toastPanel.SetActive(false);
    }

    public void ShowToast(string message, float duration)
    {

        toastText.text = message;

        toastPanel.SetActive(true);
        toastAnimator.Play("ToastFadeIn");

        toastCO = StartCoroutine(HideToast(duration));
    }

    private IEnumerator HideToast(float duration)
    {
        yield return new WaitForSeconds(duration);

        toastAnimator.Play("ToastFadeOut");

        yield return new WaitForSeconds(toastAnimator.GetCurrentAnimatorStateInfo(0).length);

        toastPanel.SetActive(false);
        StopCoroutine(toastCO);
    }
}
