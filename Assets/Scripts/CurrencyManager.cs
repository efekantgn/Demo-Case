using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    public TextMeshProUGUI CoinText;
    private void OnEnable()
    {
        playerData.OnCoinChanged += ChangeCoinText;
    }
    private void OnDisable()
    {
        playerData.OnCoinChanged -= ChangeCoinText;
    }
    private void Start()
    {
        ChangeCoinText();
    }
    private void ChangeCoinText()
    {
        CoinText.text = playerData.Coin.ToString();
    }

    //Testing
    [ContextMenu(nameof(Add1Coin))]
    public void Add1Coin()
    {
        playerData.Coin++;
    }
}
