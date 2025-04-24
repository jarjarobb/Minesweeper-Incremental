using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //This script handles a lot of things that has to persist through scenes

    BigInteger coins = 0;
    BigInteger crystals = 0;
    BigInteger clouds = 0;
    [SerializeField] int diamonds;
    [SerializeField] int endgame;
    [SerializeField] int endgameTokens;
    [SerializeField] TextMeshProUGUI coinsDisplay;
    [SerializeField] TextMeshProUGUI crystalsDisplay;
    [SerializeField] Sprite coinsImage;
    [SerializeField] Sprite crystalImage;
    [SerializeField] Sprite diamondImage;
    [SerializeField] Sprite cloudImage;
    [SerializeField] Sprite endgameTokenImage;
    [SerializeField] TextMeshProUGUI currencyDisplay;
    [SerializeField] Image currencyImageDisplay;
    [SerializeField] Image currencyDisplayBackground;
    [SerializeField] string currentDisplayCurrency = "coins";
    [SerializeField] string currentDimension = "normal";
    [SerializeField] Upgrade[] upgrades;
    [SerializeField] string[] suffixes = { "", "K", "M", "B", "T", "Qa" };
    [SerializeField] UnityEngine.UI.Button settingsButton;
    [SerializeField] Canvas settingsCanvas;
    [SerializeField]float coinsMultis = 1;
    [SerializeField] float baseAdditiveMulti =1;
    [SerializeField] float finalMulti;
    [SerializeField] Canvas statsCanvas;
    [SerializeField] TextMeshProUGUI statsCoinsDisplay;
    [SerializeField] TextMeshProUGUI statsCrystalsDisplay;
    [SerializeField] TextMeshProUGUI statsDiamondsDisplay;
    [SerializeField] TextMeshProUGUI statsCloudsDisplay;
    [SerializeField] TextMeshProUGUI statsEndgameDisplay;
    [SerializeField] TextMeshProUGUI statsEndgameTokensDisplay;
    string previousScene = "Start Scene";
    [SerializeField]bool coinsProfitPerSquare;
    [SerializeField] bool coinsEL;
    [SerializeField] bool crystalsPPS;
    [SerializeField] bool crystalsEL;
    [SerializeField] bool cloudsPPS;
    [SerializeField] bool cloudsEL;
    int baseCoins = 1;
    [SerializeField] List<float> additiveMultis;
    [SerializeField] TextMeshProUGUI endgameDisplay;
    BigInteger coinsOnWin;
    BigInteger crystalsOnWin;

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
            currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
            currencyDisplay.text = CheckForSuffix((float)coins, false);
        }
        else if (currentDisplayCurrency == "crystals")
        {
            currencyImageDisplay.overrideSprite = crystalImage;
            currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
            currencyDisplay.text = CheckForSuffix((float)crystals, false);
        }
        else if (currentDisplayCurrency == "diamonds")
        {
            currencyImageDisplay.overrideSprite = diamondImage;
            currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
            currencyDisplay.text = CheckForSuffix((float)diamonds, false);
        }
        else if (currentDisplayCurrency == "clouds")
        {
            currencyImageDisplay.overrideSprite = cloudImage;
            currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 0, 1);
            currencyDisplay.text = CheckForSuffix((float)clouds, false);

        }
        else if (currentDisplayCurrency == "endgame tokens")
        {
            currencyImageDisplay.overrideSprite = endgameTokenImage;
            currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
            currencyDisplay.text = CheckForSuffix((float)endgameTokens, false);
        }
        else if (currentDisplayCurrency == "automatic")
        {
            if (currentDimension == "normal")
            {
                currencyImageDisplay.overrideSprite = coinsImage;
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
                currencyDisplay.text = CheckForSuffix((float)coins, false);
            }
            else if (currentDimension == "the mine")
            {
                currencyImageDisplay.overrideSprite = crystalImage;
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
                currencyDisplay.text = CheckForSuffix((float)crystals, false);
            }
            else if (currentDimension == "heaven")
            {
                currencyImageDisplay.overrideSprite = cloudImage;
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 0, 1);
                currencyDisplay.text = CheckForSuffix((float)clouds, false);
            }
        }
    }
    // hides the settings button and show all the settings if wanted
    public void HideSettingsButton(bool showSettings)
    {
        if (showSettings)
        {
            settingsCanvas.gameObject.SetActive(true);
        }
    }
    //shows the settings button and hides all the settings
    public void ShowSettingsButton()
    {
        settingsCanvas.gameObject.SetActive(false);
    }
    public Canvas GetSettingsCanvas()
    {
        return settingsCanvas;
    }
    //sets the current display
    private void Start()
    {
        var currentScene = SceneManager.GetActiveScene();
        var currentSceneSplitted = currentScene.name.Split(' ');
        string previousWord = "";
        foreach (var s in currentSceneSplitted)
        {
            if ((previousWord + " "+s).ToLower() == "the mine")
            {
                DimensionCrossed("The Mine");
                break;
            }
            else if (s.ToLower() == "heaven")
            {
                DimensionCrossed("Heaven");
                break;
            }
            previousWord = s;
        }
        ChangeCurrencyDisplay(currentDisplayCurrency);
        if (coins != 0 && currentDisplayCurrency == "coins" ||
            crystals != 0 && currentDisplayCurrency == "crystals"||
            diamonds != 0 && currentDisplayCurrency == "diamonds"||
            clouds != 0 && currentDisplayCurrency == "clouds"||
            endgameTokens != 0 && currentDisplayCurrency == "endgame tokens")
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
            if (currentDisplayCurrency == "diamonds")
            {
                coinsDisplay.text = diamonds.ToString();
            }
            if (currentDisplayCurrency =="clouds")
            {
                coinsDisplay.text =clouds.ToString();
            }
            if (currentDisplayCurrency =="endgame tokens")
            {
                coinsDisplay.text = endgameTokens.ToString();
            }
        }
        foreach (var upgrade in upgrades)
        {
            if (upgrade.name == "Endgame")
            {
                ChangeEndgameDisplay(upgrade.GetLevel());
            }
        }
        StartCoroutine(GenerateCoins());
        StartCoroutine(GenerateCrystals());
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
        if (currency =="clouds")
        {
            cloudsEL = true;
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
        if (currency =="clouds")
        {
            cloudsPPS = true;
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
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
                currencyDisplay.text = CheckForSuffix((float)crystals, false);
            }
            else if (dimension.ToLower() == "normal")
            {
                currencyImageDisplay.overrideSprite = coinsImage;
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
                currencyDisplay.text = CheckForSuffix((float)coins, false);
            }
            else if (currentDimension == "heaven")
            {
                currencyImageDisplay.overrideSprite = cloudImage;
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 0, 1);
                currencyDisplay.text = CheckForSuffix((float)clouds, false);
            }
        }
    }
    // Handles not only when a game ends, but also spending a currency
    public void GameEnded(float addToCoinsAmount, int squaresRevealed, bool addOrSpend, string currency)
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
                    coins += Mathf.RoundToInt((float)baseCoins*(((float)baseAdditiveMulti)*(float)coinsMultis)*(float)squaresRevealed);
                }
                else
                {
                    coins += Mathf.RoundToInt((float)addToCoinsAmount * (((float)baseAdditiveMulti)*(float)coinsMultis));
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
                    
                    crystals += Mathf.RoundToInt((float)baseCoins * (((float)baseAdditiveMulti) * (float)coinsMultis) * (float)squaresRevealed);
                }
                else
                {
                    
                    crystals += Mathf.RoundToInt((float)addToCoinsAmount * (((float)baseAdditiveMulti) * (float)coinsMultis));
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
        if (currency == "clouds")
        {
            if (addOrSpend)
            {
                GetEffectMulti("clouds");
                if (cloudsPPS)
                {

                    print("awarded");
                    clouds += Mathf.RoundToInt((float)baseCoins * (((float)baseAdditiveMulti) * (float)coinsMultis) * (float)squaresRevealed);
                }
                else
                {
                    clouds += Mathf.RoundToInt((float)addToCoinsAmount * (((float)baseAdditiveMulti) * (float)coinsMultis));
                }
            }
            else
            {
                if (!cloudsEL)
                {
                    clouds -= (int)((float)addToCoinsAmount);
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
        statsEndgameDisplay.text = endgame.ToString();
        if (endgameTokens != 0)
        {
            statsEndgameTokensDisplay.text = CheckForSuffix((float)endgameTokens, false);
        }
        else
        {
            statsEndgameTokensDisplay.text ="0";
        }

        if (!onlyChangeStats)
        {
            if (currentDisplayCurrency == "coins")
            {
                currencyImageDisplay.overrideSprite = coinsImage;
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
                currencyDisplay.text = CheckForSuffix((float)coins, false);


            }
            else if (currentDisplayCurrency == "crystals")
            {

                currencyImageDisplay.overrideSprite = crystalImage;
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
                currencyDisplay.text = CheckForSuffix((float)crystals, false);

            }
            else if (currentDisplayCurrency == "diamonds")
            {
                currencyImageDisplay.overrideSprite = diamondImage;
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
                currencyDisplay.text = CheckForSuffix((float)diamonds, false);
            }
            else if (currentDisplayCurrency == "clouds")
            {
                currencyImageDisplay.overrideSprite = cloudImage;
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 0, 1);
                currencyDisplay.text = CheckForSuffix((float)clouds, false);
            }
            else if (currentDisplayCurrency == "endgame tokens")
            {
                currencyImageDisplay.overrideSprite = endgameTokenImage;
                currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
                currencyDisplay.text = CheckForSuffix((float)endgameTokens, false);
            }
            else if (currentDisplayCurrency == "automatic")
            {
                if (currentDimension == "normal")
                {

                    currencyImageDisplay.overrideSprite = coinsImage;
                    currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
                    currencyDisplay.text = CheckForSuffix((float)coins, false);
                }
                else if (currentDimension == "the mine")
                {

                    currencyImageDisplay.overrideSprite = crystalImage;
                    currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 255, 1);
                    currencyDisplay.text = CheckForSuffix((float)crystals, false);
                }
                else if (currentDimension == "heaven")
                {
                    currencyImageDisplay.overrideSprite = cloudImage;
                    currencyDisplayBackground.color = new Color(currencyDisplayBackground.color.r, currencyDisplayBackground.color.g, 0, 1);
                    currencyDisplay.text = CheckForSuffix((float)clouds, false);
                }
            }
            // if shop can be found then update the currencies of the shop
            if (FindObjectOfType<Shop>())
            {
                FindObjectOfType<Shop>().UpdatePlayerCoins(coins, "coins");
                FindObjectOfType<Shop>().UpdatePlayerCoins(crystals, "crystals");
                FindObjectOfType<Shop>().UpdatePlayerCoins(diamonds, "diamonds");
                FindObjectOfType<Shop>().UpdatePlayerCoins(clouds, "clouds");

            }
        }
    }
    //Gets the multi for the given currency to apply to get the currency
    private void GetEffectMulti(string currency)
    {
        baseAdditiveMulti = 1;
        coinsMultis = 1;
        for (int i = 0; i < upgrades.Length; i++)
        {
            var upgrade = upgrades[i];
            if (upgrade.GetIsMulti())
            {
                string currencyAffected = upgrade.GetCurrencyAffected();
                if (currencyAffected.ToLower() == currency || currencyAffected.ToLower() == "global")
                {
                    float effect = upgrade.GetEffect();
                    coinsMultis *= Mathf.Pow(effect, upgrade.GetLevel());
                }
            }
            if (upgrade.GetIsAdder())
            {
                string currencyAffected = upgrade.GetCurrencyAffected();
                if (currencyAffected.ToLower() == currency || currencyAffected.ToLower() == "global")
                {
                    float effect = upgrade.GetEffect();
                    baseAdditiveMulti*=1+effect*upgrade.GetLevel();
                }
            }
        }
        print(baseAdditiveMulti);
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

    private IEnumerator GenerateCoins()
    {
        while (true)
        {
            if (coinsProfitPerSquare)
            {
                foreach (Upgrade upgrade in upgrades)
                {
                    if (upgrade.name == "Bigger Field")
                    {

                        float totalSquares = Mathf.Pow((float)(5 + upgrade.GetLevel()), 2f);
                        int bombs = 3 + 2 * upgrade.GetLevel();     
                        float totalNonBombSquares = totalSquares - bombs;
                        GetEffectMulti("coins");
                        coinsOnWin = (BigInteger)((float)baseCoins * ((1 + (float)baseAdditiveMulti) * (float)coinsMultis) * (float)totalNonBombSquares);
                        break;
                    }
                }
                foreach (Upgrade upgrade in upgrades)
                {
                    if (upgrade.name == "Coin Generation")
                    {
                        coins += (BigInteger)((float)coinsOnWin / 100 * upgrade.GetLevel());
                        UpdateDisplay(false, false);
                    }
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
    private IEnumerator GenerateCrystals()
    {
        while (true)
        {
            if (crystalsPPS)
            {
                foreach (Upgrade upgrade in upgrades)
                {
                    if (upgrade.name == "Bigger Field II")
                    {

                        float totalSquares = Mathf.Pow((float)(5 + upgrade.GetLevel()), 2f);
                        int bombs = 3 + 2 * upgrade.GetLevel();     
                        float totalNonBombSquares = totalSquares - bombs;
                        GetEffectMulti("crystals");
                        crystalsOnWin = (BigInteger)((float)baseCoins * ((1 + (float)baseAdditiveMulti) * (float)coinsMultis) * (float)totalNonBombSquares);
                        break;
                    }
                }
                foreach (Upgrade upgrade in upgrades)
                {
                    if (upgrade.name == "Crystal Generation")
                    {
                        crystals += (BigInteger)((float)crystalsOnWin / 100 * upgrade.GetLevel());
                        UpdateDisplay(false, false);
                    }
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
    public void EndgameReset()
    {
        
        
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.name == "Endgame")
            {
                print("endgame upgrade found");
                if (upgrade.GetLevel() >= 1)
                {
                    AddEndgameTokens(upgrade.GetLevel());
                }
            }
            if (!upgrade.GetNotResetOnEndgame())
            {
                upgrade.ResetLevel();
            }
        }


        coins = 0;
        crystals = 0;
        diamonds = 0;
        clouds = 0;
        DisableEL();
        DisablePPS();
        FindObjectOfType<Unlocks>().EndgamePerformed();
        IncreaseEndgame();
        StartCoroutine(FindObjectOfType<Cutscene>().EndgameCutscene(endgame));
        UpdateDisplay(false, false);

    }
    public void DisableEL()
    {
        coinsEL = false;
        crystalsEL = false;
    }
    public void DisablePPS()
    {
        coinsProfitPerSquare = false;
        crystalsPPS = false;
        cloudsPPS = false;
    }
    public void AddEndgameTokens(int amount)
    {
        endgameTokens += amount;
    }
    public void IncreaseEndgame()
    {
        endgame++;
    }
    public int GetEndgame()
    {
        return endgame;
    }
    //diamond functions
    public int GetDiamonds()
    {
        return diamonds;
    }
    public void SpendDiamond()
    {
        diamonds--;
        UpdateDisplay(false, false);
    }
    public void AwardDiamond()
    {
        diamonds++;
    }
    public void SpendEndgameTokens(int amount)
    {
        endgameTokens -= amount;
        UpdateDisplay(false, false);
    }
    public void ChangeEndgameDisplay(int endgameTokensToEarn)
    {
        endgameDisplay.text = endgameTokensToEarn.ToString();
    }
    public void HideEndgameDisplay()
    {
        endgameDisplay.gameObject.SetActive(false);
    }
    public void ShowEndgameDisplay()
    {
        endgameDisplay.gameObject.SetActive(true);
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
    public int GetEndgameTokens()
    {
        return endgameTokens;
    }
    public Upgrade[] GetUpgrades()
    {
        return upgrades;
    }
}
