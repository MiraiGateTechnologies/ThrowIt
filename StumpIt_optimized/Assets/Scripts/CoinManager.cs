using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private GameObject CoinsPileParent;
    [SerializeField] private TextMeshProUGUI coinsCounter;
    [SerializeField] private TextMeshProUGUI plusSymbol;
    private Vector3[] initialCoinsPositions;
    private Quaternion[]  initialCoinsRotations;

    private void Start()
    {
        plusSymbol.text = "";
        coinsCounter.text = "";
        CoinsPileParent.SetActive(false);
        initialCoinsPositions = new Vector3[CoinsPileParent.transform.childCount];
        initialCoinsRotations = new Quaternion[CoinsPileParent.transform.childCount];
        for(int i = 0;i<CoinsPileParent.transform.childCount;i++)
        {
            initialCoinsPositions[i] = CoinsPileParent.transform.GetChild(i).position;
            initialCoinsRotations[i] = CoinsPileParent.transform.GetChild(i).rotation;
        }
    }

    private void Reset()
    {
        for (int i = 0; i < CoinsPileParent.transform.childCount; i++)
        {
            CoinsPileParent.transform.GetChild(i).position = initialCoinsPositions[i];
            CoinsPileParent.transform.GetChild(i).rotation = initialCoinsRotations[i];
        }
    }
    public void ResetCoinManagerTextsAndImages()
    {
        coinsCounter.text = "";
        CoinsPileParent.SetActive(false);
        plusSymbol.text = "";
    }

    public void RewardPileCoin()
    {
        Reset();
        plusSymbol.text = "+";
        var delay = 0f;
        CoinsPileParent.SetActive(true);

        for (int i = 0;i<CoinsPileParent.transform.childCount;i++)
        {
            CoinsPileParent.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            CoinsPileParent.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(197f, 640f), 1f).SetDelay(delay+0.4f).SetEase(Ease.InBack);

            CoinsPileParent.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay+0.5f).SetEase(Ease.Flash);

            CoinsPileParent.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay+2f).SetEase(Ease.OutBack);

            delay += 0.1f;
           
        }

    }
}
