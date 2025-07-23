using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{

    public TextMeshProUGUI coinsText;

    public SkinDatabase skinsDB;
    public Skins skinManager;
    private void Start()
    {
        UpdateCoinsUI();
    }

    public void UpdateCoinsUI()
    {
        coinsText.text = PlayerPrefs.GetInt("coins", 0).ToString();
    }
    public void BuySkin()
    {
        int price = skinsDB.skins[skinManager.currentIndex].price;
        if (price <= PlayerPrefs.GetInt("coins", 0))
        {
            skinsDB.skins[skinManager.currentIndex].unlocked = true;
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins",0) - price);
            skinManager.Unlock();
            UpdateCoinsUI();
        }
    }
}
