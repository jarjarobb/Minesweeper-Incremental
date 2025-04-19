using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //This script handles a lot of things that has to persist through scenes

    BigInteger coins = 0;
    BigInteger crystals = 0;
    BigInteger clouds = 0;
    [SerializeField] int diamonds;
    [SerializeField] TextMeshProUGUI coinsDisplay;
    [SerializeField] TextMeshProUGUI crystalsDisplay;
    [SerializeField] Sprite coinsImage;
    [SerializeField] Sprite crystalImage;
    [SerializeField] Sprite diamondImage;
    [SerializeField] Sprite cloudImage;
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
    [SerializeField] TextMeshProUGUI statsDiamondsDisplay;
    [SerializeField] TextMeshProUGUI statsCloudsDisplay;
    string previousScene = "Start Scene";
    [SerializeField]bool coinsProfitPerSquare;
    [SerializeField] bool coinsEL;
    [SerializeField] bool crystalsPPS;
    [SerializeField] bool crystalsEL;
    int baseCoins = 1;

    //Creates the Singleton
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
    //changes the currency display based on the setting
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
        else if (currentDisplayCurrency == "diamonds")
        {
            currencyImageDisplay.overrideSprite = diamondImage;
            currencyDisplay.text = CheckForSuffix((float)diamonds, false);
        }
        else if (currentDisplayCurrency == "clouds")
        {
            currencyImageDisplay.overrideSprite = cloudImage;
            currencyDisplay.text = CheckForSuffix((float)clouds, false);
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
            else if (currentDimension =="heaven")
            {
                currencyImageDisplay.overrideSprite = cloudImage;
                currencyDisplay.text = CheckForSuffix((float)clouds, false);
            }    
        }
    }
    // hides the settings button and show all the settings if wanted
    public void HideSettingsButton(bool showSettings)
    {
        settingsButton.gameObject.SetActive(false);
        if (showSettings)
        {
            settingsCanvas.gameObject.SetActive(true);
        }
    }
    //shows the settings button and hides all the settings
    public void ShowSettingsButton()
    {
        settingsButton.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
    }
    //sets the current display
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
       
    }
    // enables EL (everlasting) on a certain currency
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
    // Enables PPS (profit per square) on a certain currency
    public void EnableProfitPerSquare(string currency)
    {
        if (currency == "coins")
        {
            coinsProfitPerSquare = true;
        }
        if (currency == "crystals")
        {
            crystalsPPS = true;
        }
    }
    //What happens when a dimension is crossed
    public void DimensionCrossed(string dimension)
    {
        // display currency changes if it is automatic
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
            else if (currentDimension == "heaven")
            {
                currencyImageDisplay.overrideSprite = cloudImage;
                currencyDisplay.text = CheckForSuffix((float)clouds, false);
            }
        }
    }
    // Handles not only when a game ends, but also spending a currency
    public void GameEnded(int addToCoinsAmount, int squaresRevealed, bool addOrSpend, string currency)
    {
        // add = true; spend = false for addOrSpend
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
    //changes the stat menu visibility
    public void ChangeStatsVisibility()
    {
        statsCanvas.gameObject.SetActive(!statsCanvas.gameObject.activeSelf);
    }
    //updates the stat menu display
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
        if (diamonds != 0)
        {
            statsDiamondsDisplay.text = CheckForSuffix((float)diamonds, false);
        }
        else
        {
            statsDiamondsDisplay.text = "0";
        }
        if (clouds != 0)
        {
            statsCloudsDisplay.text = CheckForSuffix((float)clouds, false);
        }
        else
        {
            statsCloudsDisplay.text = "0";
        }
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
            else if (currentDisplayCurrency == "diamonds")
            {
                currencyImageDisplay.overrideSprite = diamondImage;
                currencyDisplay.text = CheckForSuffix((float)diamonds, false);
            }
            else if (currentDisplayCurrency == "clouds")
            {
                currencyImageDisplay.overrideSprite = cloudImage;
                currencyDisplay.text = CheckForSuffix((float)clouds, false);
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
                else if (currentDimension == "heaven")
                {
                    currencyImageDisplay.overrideSprite = cloudImage;

                    currencyDisplay.text = CheckForSuffix((float)clouds, false);
                }
            }
            // if shop can be found then update the currencies of the shop
            if (FindObjectOfType<Shop>())
            {
                FindObjectOfType<Shop>().UpdatePlayerCoins(coins, "coins");
                FindObjectOfType<Shop>().UpdatePlayerCoins(crystals, "crystals");
                FindObjectOfType<Shop>().UpdatePlayerCoins(clouds, "clouds");

            }
        }
    }
    //Gets the multi for the given currency to apply to get the currency
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
    }
    // Checks for a suffix to simplify the number
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
    //diamond functions
    public int GetDiamonds()
    {
        return diamonds;
    }
    public void SpendDiamond()
    {
        diamonds--;
    }
    // gives a diamond
    public void AwardDiamond()
    {
        diamonds++;
    }
    // Stores the previous scene
    public void StorePreviousScene(string sceneName)
    {
        previousScene = sceneName;
    }
    //Gets the previous scene
    public string GetPreviousScene()
    {
        return previousScene;
    }
    // other return functions
    public string GetCurrentDisplayCurrency()
    {
        return currentDisplayCurrency;
    }
    public BigInteger GetCoins()
    {
        return coins;
    }
    public BigInteger GetCrystals()
    {
        return crystals;
    }
    public BigInteger GetClouds()
    {
        return clouds;
    }
}
