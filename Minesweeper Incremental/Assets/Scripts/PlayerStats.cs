using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Numerics;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    BigInteger coins = 250;
    BigInteger crystals = 0;
    [SerializeField] int crystal;
    [SerializeField] TextMeshProUGUI coinsDisplay;
    [SerializeField] TextMeshProUGUI crystalsDisplay;
    [SerializeField] Sprite coinsImage;
    [SerializeField] Sprite crystalImage;
    [SerializeField] TextMeshProUGUI currencyDisplay;
    [SerializeField] Image currencyImageDisplay;
    [SerializeField] string currentDisplayCurrency = "coins";
    [SerializeField] string currentDimension = "normal";
    [SerializeField] Upgrade[] upgrades;
    [SerializeField] string[] suffixes = { "", "K", "M", "B", "T", "Qa" };
    [SerializeField] UnityEngine.UI.Button settingsButton;
    [SerializeField] Canvas settingsCanvas;
    float coinsMultis = 1;
    string previousScene = "Start Scene";
    bool coinsProfitPerSquare;
    int baseCoins = 1;
    private void Awake()
    {
        int NumberOfPlayerStatuses = FindObjectsOfType<PlayerStats>().Length;
        if (NumberOfPlayerStatuses > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ChangeCurrencyDisplay(string currency)
    {
        currentDisplayCurrency = currency.ToLower();
        if (currentDisplayCurrency == "coins")
        {
            currencyImageDisplay.overrideSprite = coinsImage;
            currencyDisplay.text = CheckForSuffix((float)coins, false);
        }
        else if (currentDisplayCurrency == "crystals")
        {
            currencyImageDisplay.overrideSprite = crystalImage;
            currencyDisplay.text = CheckForSuffix((float)crystals, false); 
        }
        else if (currentDisplayCurrency == "automatic")
        {
            if (currentDimension == "normal")
            {
                currencyImageDisplay.overrideSprite = coinsImage;
                currencyDisplay.text = CheckForSuffix((float)coins, false);
            }
            else if (currentDimension == "the mine")
            {
                currencyImageDisplay.overrideSprite = crystalImage;
                currencyDisplay.text = CheckForSuffix((float)crystals, false);
            }
        }
    }
    public void HideSettingsButton()
    {
        settingsButton.gameObject.SetActive(false);
        for(int i = 0; i < settingsCanvas.transform.childCount; i++)
        {
            settingsCanvas.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void ShowSettingsButton()
    {
        settingsButton.gameObject.SetActive(true);
        for (int i = 0; i < settingsCanvas.transform.childCount; i++)
        {
            settingsCanvas.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        ChangeCurrencyDisplay(currentDisplayCurrency);
        if (coins != 0 && currentDisplayCurrency == "coins")
        {
            UpdateDisplay();
        }
        else if (crystals != 0 && currentDisplayCurrency == "crystals")
        {
            UpdateDisplay();
        }
        else
        {
            if (currentDisplayCurrency == "coins")
            {
                coinsDisplay.text = coins.ToString();
            }
            if (currentDisplayCurrency == "crystals")
            {
                coinsDisplay.text = crystals.ToString();
            }
        }
        
    }
    public void DimensionCrossed(string dimension)
    {
        currentDimension = dimension.ToLower();
        if (currentDisplayCurrency == "automatic")
        {
            if (dimension.ToLower() == "the mine")
            {
                currencyImageDisplay.overrideSprite = crystalImage;
                currencyDisplay.text = CheckForSuffix((float)crystals, false);
            }
            else if (dimension.ToLower() == "normal")
            {
                currencyImageDisplay.overrideSprite = coinsImage;
                currencyDisplay.text = CheckForSuffix((float)coins, false);
            }
        }
    }
    public string GetCurrentDisplayCurrency()
    {
        return currentDisplayCurrency;
    }
    public void EnableProfitPerSquare(string currency)
    {
        if (currency == "coins")
        {
            coinsProfitPerSquare = true;
        }
    }
    public void StorePreviousScene(string sceneName)
    {
        print(sceneName);
        previousScene = sceneName;
    }
    public string GetPreviousScene()
    {
        return previousScene;
    }
    private void Update()
    {
        crystal = (int)crystals;
    }
    public void GameEnded(int addToCoinsAmount, int squaresRevealed, bool addOrSpend, string currency)
    {
        
        if (currency == "coins")
        {
            if (addOrSpend)
            {
                GetEffectMulti("coins");
                if (coinsProfitPerSquare)
                {
                    print("awarded");
                    coins += Mathf.RoundToInt((float)baseCoins*(float)coinsMultis *(float)squaresRevealed);
                }
                else
                {
                    coins += Mathf.RoundToInt((float)addToCoinsAmount * (float)coinsMultis);
                }
            }
            else
            {
                coins -= (int)((float)addToCoinsAmount);
            }
        }
        if (currency == "crystals")
        {
            if (addOrSpend)
            {
                GetEffectMulti("crystals");
                crystals += Mathf.RoundToInt((float)addToCoinsAmount * (float)coinsMultis);
            }
            else
            {
                crystals -= (int)((float)addToCoinsAmount);
            }
        }
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        print("updated display");
        if (currentDisplayCurrency == "coins")
        {
            currencyImageDisplay.overrideSprite = coinsImage;
            currencyDisplay.text = CheckForSuffix((float)coins, false);
        }
        else if (currentDisplayCurrency == "crystals")
        {
            currencyImageDisplay.overrideSprite = coinsImage;
            currencyDisplay.text = CheckForSuffix((float)coins, false);
        }
        else if (currentDisplayCurrency == "automatic")
        {
            if (currentDimension == "normal")
            {
                currencyImageDisplay.overrideSprite = coinsImage;
                currencyDisplay.text = CheckForSuffix((float)coins, false);
            }
            else if (currentDimension == "the mine")
            {
                
                currencyImageDisplay.overrideSprite = crystalImage;
                currencyDisplay.text = CheckForSuffix((float)crystals, false);
            }
        }
        if (FindObjectOfType<Shop>())
        {
            FindObjectOfType<Shop>().UpdatePlayerCoins(coins, "coins");
            FindObjectOfType<Shop>().UpdatePlayerCoins(crystals, "crystals");
           
            
        }
        
    }
    public BigInteger GetCoins()
    {
        return coins;
    }
    public BigInteger GetCrystals ()
    {
        return crystals ;
    }

    private void GetEffectMulti(string currency)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            var upgrade = upgrades[i];
            if (upgrade.GetIsMulti() == true)
            {
                string currencyAffected = upgrade.GetCurrencyAffected();
                if (currencyAffected.ToLower() == currency)
                {
                    float effect = upgrade.GetEffect();
                    coinsMultis = Mathf.Pow(effect, upgrade.GetLevel());
                }
            }
        }
        print(coinsMultis);
    }
    public string CheckForSuffix(float amount, bool applyEndingPreFirstSuffix)
    {
        int ooMs = (int)Mathf.Log10((float)amount);
        int suffixIndex = ooMs / 3;
        int remaining = ooMs % 3;
        if (amount == 0)
        {
            return 0.ToString();
        }
        string suffix = suffixes[suffixIndex];
        if (suffixIndex > 0 || applyEndingPreFirstSuffix)
        {
            if (remaining == 0)
            {
                return ((float)amount / Mathf.Pow(10, (suffixIndex * 3))).ToString("#.000") + suffix;
            }
            else if (remaining == 1)
            {
                return ((float)amount / Mathf.Pow(10, (suffixIndex * 3))).ToString("#.00") + suffix;
            }
            else
            {
                return ((float)amount / Mathf.Pow(10, (suffixIndex * 3))).ToString("#.0") + suffix;
            }
        }
        else
        {
            return ((float)amount / Mathf.Pow(10, (suffixIndex * 3))).ToString() + suffix;
        }
    }
}
