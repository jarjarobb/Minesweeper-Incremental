using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Numerics;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
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
    
    Upgrade selectedUpgrade;
    BigInteger coins;
    BigInteger crystals;
    int diamonds;
    
    // Start is called before the first frame update
    void Start()
    {
        coins = FindObjectOfType<PlayerStats>().GetCoins();
    }

    // Update is called once per frame
    void Update()
    {

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
    }
    public void ExpandButtonClicked(Upgrade upgrade)
    {
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

    private void UpdateMoreDetails(Upgrade upgrade, bool changeVisibility)
    {
        print("update");
        nameLabel.text = upgrade.name;
        if (upgrade.name == "Profit Per Square")
        {
            FindObjectOfType<PlayerStats>().EnableProfitPerSquare("coins");
        }
        if (upgrade.name == "Coins Everlasting")
        { 
            FindObjectOfType<PlayerStats>().EnableEL("coins");
        }
        descriptionLabel.text = upgrade.GetDescription();
        BigInteger price = (BigInteger)(upgrade.GetPrice() * Mathf.Pow(upgrade.GetScaling(), upgrade.GetLevel()));
        priceLabel.text = FindObjectOfType<PlayerStats>().CheckForSuffix((float)price, false);
        if (upgrade.GetLevel() == upgrade.GetLevelCap())
        {
            priceLabel.text = "Maxed!";
        }
        if (upgrade.GetLevelCap() == -1)
        {
            levelLabel.text = upgrade.GetLevel().ToString();
        }
        else
        {
            levelLabel.text = upgrade.GetLevel().ToString() + " / " + upgrade.GetLevelCap().ToString();
        }
        
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
        selectedUpgrade = upgrade;
        if (changeVisibility)
        {
            upgradeCanvas.gameObject.SetActive(!upgradeCanvas.gameObject.activeInHierarchy);
        }
    }
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
        else if (selectedUpgrade.GetCurrencyNeeded() == "crystals")
        {
            BuyCrystalUpgrade();
        }
        else if (selectedUpgrade.GetCurrencyNeeded() == "diamonds")
        {
            BuyDiamondUpgrade();
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
    public Upgrade[] GetUpgrades()
    {
        return upgrades;
    }
}
