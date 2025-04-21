using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Numerics;
using UnityEngine.UI;
using UnityEditor.VersionControl;
using System.Runtime.InteropServices.WindowsRuntime;

public class Shop : MonoBehaviour
{
    //This script deals with operating the shop

    [SerializeField] Canvas upgradeCanvas;
    [SerializeField] TextMeshProUGUI nameLabel;
    [SerializeField] TextMeshProUGUI descriptionLabel;
    [SerializeField] TextMeshProUGUI levelLabel;
    [SerializeField] TextMeshProUGUI priceLabel;
    [SerializeField] TextMeshProUGUI effectLabel;
    [SerializeField] Image currencyNeededImage;
    [SerializeField] Upgrade[] upgrades;
    [SerializeField] Sprite coinImage;
    [SerializeField] Sprite crystalImage;
    [SerializeField] Sprite diamondImage;
    [SerializeField] Sprite cloudImage;
    
    Upgrade selectedUpgrade;
    BigInteger coins;
    BigInteger crystals;
    BigInteger clouds;
    int diamonds;
    

    // Start is called before the first frame update
    void Start()
    {
        //Sets the initial value of coins
        coins = FindObjectOfType<PlayerStats>().GetCoins();
    }

    public void UpdatePlayerCoins(BigInteger newAmount, string currency)
    {
        if (currency == "coins")
        {
            coins = newAmount;
        }
        else if (currency == "crystals")
        {
            crystals = newAmount;
        }
        else if (currency == "diamonds")
        {
            diamonds = (int)newAmount;
        }
        else if (currency == "clouds")
        {
            clouds = newAmount;
        }
    }
    //What happens when the expand button of an upgrade is clicked
    public void ExpandButtonClicked(Upgrade upgrade)
    {
        //updates the details canvas based on if the selected upgrade is null and if the selcted upgrade is the same
        if (selectedUpgrade != null)
        {
            if (selectedUpgrade == upgrade)
            {
                UpdateMoreDetails(upgrade, true);
            }
            else if (selectedUpgrade != upgrade && !upgradeCanvas.isActiveAndEnabled)
            {
                UpdateMoreDetails(upgrade, true);
            }
            else
            {
                UpdateMoreDetails(upgrade, false);
            }
        }
        else
        {
            UpdateMoreDetails(upgrade, true);
        }
    }
    //Updates the details canvas
    private void UpdateMoreDetails(Upgrade upgrade, bool changeVisibility)
    {
        print("update");
        nameLabel.text = upgrade.name;
        descriptionLabel.text = upgrade.GetDescription();
        BigInteger price = (BigInteger)(upgrade.GetPrice() * Mathf.Pow(upgrade.GetScaling(), upgrade.GetLevel()));
        priceLabel.text = FindObjectOfType<PlayerStats>().CheckForSuffix((float)price, false);
        if (upgrade.GetLevel() == upgrade.GetLevelCap())
        {
            priceLabel.text = "Maxed!";
        }
        // level cap -1 means uncapped
        if (upgrade.GetLevelCap() == -1)
        {
            levelLabel.text = upgrade.GetLevel().ToString();
        }
        else
        {
            levelLabel.text = upgrade.GetLevel().ToString() + " / " + upgrade.GetLevelCap().ToString();
        }
        //calculates the effect and displays the effect if the upgrade has one
        if (upgrade.GetIsMulti())
        {
            float effect = Mathf.Pow(upgrade.GetEffect(), upgrade.GetLevel());
            print(effect);
            effectLabel.text = "Effect: x" + FindObjectOfType<PlayerStats>().CheckForSuffix(effect, true);
        }
        if (upgrade.GetIsAdder())
        {
            float effect = 1+upgrade.GetEffect() *upgrade.GetLevel();
            effectLabel.text = "Effect: x" + FindObjectOfType<PlayerStats>().CheckForSuffix(effect, true);
        }
        else
        {
            effectLabel.text = "";
        }
        //displays the image of the currency needed to buy this upgrade
        if (upgrade.GetCurrencyNeeded() == "coins")
        {
            currencyNeededImage.sprite = coinImage;
        }
        else if (upgrade.GetCurrencyNeeded() == "crystals")
        {
            currencyNeededImage.sprite = crystalImage;
        }
        else if (upgrade.GetCurrencyNeeded() == "diamonds")
        {
            currencyNeededImage.sprite = diamondImage;
        }
        else if (upgrade.GetCurrencyNeeded() == "clouds")
        {
            currencyNeededImage.sprite = cloudImage;
        }
        selectedUpgrade = upgrade;
        if (changeVisibility)
        {
            upgradeCanvas.gameObject.SetActive(!upgradeCanvas.gameObject.activeInHierarchy);
        }
    }
    //What happens when an upgrade is bought
    public void BuyUpgrade()
    {
        if (selectedUpgrade.GetCurrencyNeeded() == "coins")
        {
            UpdatePlayerCoins(FindObjectOfType<PlayerStats>().GetCoins(), "coins");
            if ((float)coins >= selectedUpgrade.GetPrice() * Mathf.Pow(selectedUpgrade.GetScaling(), selectedUpgrade.GetLevel()))
            {

                // level cap -1 means uncapped
                if (selectedUpgrade.GetLevel() + 1 <= selectedUpgrade.GetLevelCap() || selectedUpgrade.GetLevelCap() == -1)
                {

                    FindObjectOfType<PlayerStats>().GameEnded(Mathf.RoundToInt(selectedUpgrade.GetPrice() * Mathf.Pow(selectedUpgrade.GetScaling(), selectedUpgrade.GetLevel())), 0, false, "coins");
                    selectedUpgrade.LevelUp();
                    
                    UpdateMoreDetails(selectedUpgrade, false);
                }
            }
        }
        // Branches off into three similar functions depending on the currency needed
        else if (selectedUpgrade.GetCurrencyNeeded() == "crystals")
        {
            BuyCrystalUpgrade();
        }
        else if (selectedUpgrade.GetCurrencyNeeded() == "diamonds")
        {
            BuyDiamondUpgrade();
        }
        else if (selectedUpgrade.GetCurrencyNeeded() == "clouds")
        {
            BuyCloudUpgrade();
        }
    }
    public void BuyDiamondUpgrade()
    {
        UpdatePlayerCoins((BigInteger)FindObjectOfType<PlayerStats>().GetDiamonds(), "diamonds");
        if ((float)diamonds >= selectedUpgrade.GetPrice() * Mathf.Pow(selectedUpgrade.GetScaling(), selectedUpgrade.GetLevel()))
        {
            
            // level cap -1 means uncapped
            if (selectedUpgrade.GetLevel() + 1 <= selectedUpgrade.GetLevelCap() || selectedUpgrade.GetLevelCap() == -1)
            {

                FindObjectOfType<PlayerStats>().SpendDiamond();
                selectedUpgrade.LevelUp();
                if (selectedUpgrade.name == "THE KEY")
                {
                    print("cutscene");
                    StartCoroutine(FindObjectOfType<Cutscene>().HeavenCutscene());
                }
                UpdateMoreDetails(selectedUpgrade, false);
            }
        }
    }
    public void BuyCrystalUpgrade()
    {
        UpdatePlayerCoins(FindObjectOfType<PlayerStats>().GetCrystals(), "crystals");
        if ((float)crystals >= selectedUpgrade.GetPrice() * Mathf.Pow(selectedUpgrade.GetScaling(), selectedUpgrade.GetLevel()))
        {
            
            // level cap -1 means uncapped
            if (selectedUpgrade.GetLevel() + 1 <= selectedUpgrade.GetLevelCap() || selectedUpgrade.GetLevelCap() == -1)
            {
                
                FindObjectOfType<PlayerStats>().GameEnded(Mathf.RoundToInt(selectedUpgrade.GetPrice() * Mathf.Pow(selectedUpgrade.GetScaling(), selectedUpgrade.GetLevel())),0, false, "crystals");
                selectedUpgrade.LevelUp();
                UpdateMoreDetails(selectedUpgrade, false);
            }
        }
    }
    public void BuyCloudUpgrade()
    {
        UpdatePlayerCoins(FindObjectOfType<PlayerStats>().GetCrystals(), "clouds");
        if ((float)clouds >= selectedUpgrade.GetPrice() * Mathf.Pow(selectedUpgrade.GetScaling(), selectedUpgrade.GetLevel()))
        {
            
            // level cap -1 means uncapped
            if (selectedUpgrade.GetLevel() + 1 <= selectedUpgrade.GetLevelCap() || selectedUpgrade.GetLevelCap() == -1)
            {
                
                FindObjectOfType<PlayerStats>().GameEnded(Mathf.RoundToInt(selectedUpgrade.GetPrice() * Mathf.Pow(selectedUpgrade.GetScaling(), selectedUpgrade.GetLevel())),0, false, "clouds");
                selectedUpgrade.LevelUp();
                UpdateMoreDetails(selectedUpgrade, false);
            }
        }
    }
    //return function
    public Upgrade[] GetUpgrades()
    {
        return upgrades;
    }
}
