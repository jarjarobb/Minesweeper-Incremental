using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    BigInteger coins = 1000000000;
    BigInteger crystals = 100000000;
    [SerializeField] int diamonds;
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
    [SerializeField]float coinsMultis = 1;
    [SerializeField] float baseAdditiveMulti;
    [SerializeField] float finalMulti;
    [SerializeField] Canvas statsCanvas;
    [SerializeField] TextMeshProUGUI statsCoinsDisplay;
    [SerializeField] TextMeshProUGUI statsCrystalsDisplay;
    string previousScene = "Start Scene";
    [SerializeField]bool coinsProfitPerSquare;
    [SerializeField] bool coinsEL;
    [SerializeField] bool crystalsPPS;
    [SerializeField] bool crystalsEL;
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
    public void AwardDiamond()
    {
        diamonds++;
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
    public void HideSettingsButton(bool showSettings)
    {
        settingsButton.gameObject.SetActive(false);
        if (showSettings)
        {
            for (int i = 0; i < settingsCanvas.transform.childCount; i++)
            {
                settingsCanvas.transform.GetChild(i).gameObject.SetActive(true);
            }
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
            UpdateDisplay(true, false);
        }
        else if (crystals != 0 && currentDisplayCurrency == "crystals")
        {
            UpdateDisplay(true, false);
        }
        else
        {
            UpdateDisplay(false, true);
            if (currentDisplayCurrency == "coins")
            {
                coinsDisplay.text = coins.ToString();
            }
            if (currentDisplayCurrency == "crystals")
            {
                coinsDisplay.text = crystals.ToString();
            }
        }
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.name == "Profit Per Square")
            {
                if (upgrade.GetLevel() == 1)
                {
                    EnableProfitPerSquare("coins");
                }
            }
            if (upgrade.name == "CrystalsPPS")
            {
                if (upgrade.GetLevel() == 1)
                {
                    EnableProfitPerSquare("crystals");
                }
            }
            if (upgrade.name == "Coins Everlasting")
            {
                if (upgrade.GetLevel() == 1)
                {
                    EnableEL("coins");
                }
            }
            if (upgrade.name == "CrystalsEL")
            {
                if (upgrade.GetLevel() == 1)
                {
                    EnableEL("crystals");
                }
            }
        }
    }

    public void EnableEL(string currency)
    {
        if (currency == "coins")
        {
            coinsEL = true;
        }
        if (currency == "crystals")
        {
            crystalsEL = true;
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
        if (currency =="crystals")
        {
            crystalsPPS = true;
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
                    coins += Mathf.RoundToInt((float)baseCoins*((1+(float)baseAdditiveMulti)*(float)coinsMultis)*(float)squaresRevealed);
                }
                else
                {
                    coins += Mathf.RoundToInt((float)addToCoinsAmount * ((1 + (float)baseAdditiveMulti) * (float)coinsMultis));
                }
            }
            else
            {
                if (!coinsEL)
                {
                    coins -= (int)((float)addToCoinsAmount);
                }
            }
        }
        if (currency == "crystals")
        {
            if (addOrSpend)
            {
                GetEffectMulti("crystals");
                if (crystalsPPS)
                {

                    print("awarded");
                    crystals += Mathf.RoundToInt((float)baseCoins * ((1 + (float)baseAdditiveMulti) * (float)coinsMultis) * (float)squaresRevealed);
                }
                else
                {
                    crystals += Mathf.RoundToInt((float)addToCoinsAmount * ((1 + (float)baseAdditiveMulti) * (float)coinsMultis));
                }
            }
            else
            {
                if (!crystalsEL)
                {
                    crystals -= (int)((float)addToCoinsAmount);
                }
            }
        }
        UpdateDisplay(false, false);
    }
    public void ChangeStatsVisibility()
    {
        statsCanvas.gameObject.SetActive(!statsCanvas.gameObject.activeSelf);
    }
    private void UpdateDisplay(bool changeIcon, bool onlyChangeStats)
    {
        if (coins != 0)
        {
            statsCoinsDisplay.text = CheckForSuffix((float)coins, false);
        }
        else
        {
            statsCoinsDisplay.text = "0";
        }
        if (crystals != 0)
        {
            statsCrystalsDisplay.text = CheckForSuffix((float)crystals, false);
        }
        else
        {
            statsCrystalsDisplay.text = "0";
        }

        print("updated display");
        if (!onlyChangeStats)
        {
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
            if (FindObjectOfType<Shop>())
            {
                FindObjectOfType<Shop>().UpdatePlayerCoins(coins, "coins");
                FindObjectOfType<Shop>().UpdatePlayerCoins(crystals, "crystals");


            }
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
        coinsMultis = 1;
        for (int i = 0; i < upgrades.Length; i++)
        {
            var upgrade = upgrades[i];
            if (upgrade.GetIsMulti() == true)
            {
                string currencyAffected = upgrade.GetCurrencyAffected();
                if (currencyAffected.ToLower() == currency)
                {
                    float effect = upgrade.GetEffect();
                    coinsMultis *= Mathf.Pow(effect, upgrade.GetLevel());
                }
            }
            if (upgrade.GetIsAdder() == true)
            {
                string currencyAffected = upgrade.GetCurrencyAffected();
                if (currencyAffected.ToLower() == currency)
                {
                    float effect = upgrade.GetEffect();
                    baseAdditiveMulti += effect*upgrade.GetLevel();
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

    public int GetDiamonds()
    {
        return diamonds;
    }
    public void SpendDiamond()
    {
        diamonds--;
    }
}
