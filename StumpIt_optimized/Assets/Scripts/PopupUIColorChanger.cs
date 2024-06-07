using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupUIColorChanger : MonoBehaviour
{
    [SerializeField]
    private Color[] popupColors;

    public void DisplayPopupWithColor( Image imageObject)
    {
        int colorIndex = Random.Range(0, popupColors.Length);
        if (colorIndex < 0 || colorIndex >= popupColors.Length)
        {
            Debug.LogError("Color index out of range.");
            return;
        }

        imageObject.color = popupColors[colorIndex];
        imageObject.gameObject.SetActive(true);
    }
}
