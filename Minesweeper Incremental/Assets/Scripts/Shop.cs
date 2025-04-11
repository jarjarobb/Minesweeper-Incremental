using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Numerics;

public class Shop : MonoBehaviour
{
    [SerializeField] Canvas upgradeCanvas;
    [SerializeField] TextMeshProUGUI nameLabel;
    [SerializeField] TextMeshProUGUI descriptionLabel;
    [SerializeField] TextMeshProUGUI levelLabel;
    [SerializeField] TextMeshProUGUI priceLabel;
    [SerializeField] TextMeshProUGUI effectLabel;
    [SerializeField] Upgrade[] upgrades;
    
    Upgrade selectedUpgrade;
    BigInteger coins;
    BigInteger crystals;
    
    // Start is called before the first frame update
    void Start()
    {
        coins = FindObjectOfType<PlayerStats>().GetCoins();
        foreach(Upgrade upgrade in upgrades)
        {
            if (upgrade.name == "Profit Per Square")
            {
                if (upgrade.GetLevel() == 1)
                {
                    FindObjectOfType<PlayerStats>().EnableProfitPerSquare("coins");
                }
            }
        }
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
    }
    public void ExpandButtonClicked(Upgrade upgrade)
    {
        if (selectedUpgrade != null)
        {
            UpdateMoreDetails(upgrade, false);
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
        float effect = Mathf.Pow(upgrade.GetEffect(), upgrade.GetLevel());
        if (upgrade.GetIsMulti())
        {
            print(effect);
            effectLabel.text = "Effect: x" + FindObjectOfType<PlayerStats>().CheckForSuffix(effect, true);
        }
        else
        {
            effectLabel.text = "";
        }
        selectedUpgrade = upgrade;
        if (changeVisibility)
        {
            upgradeCanvas.gameObject.SetActive(!upgradeCanvas.gameObject.activeInHierarchy);
        }
    }
    public void BuyUpgrade()
    {
        print("upgrade bought");
        UpdatePlayerCoins(FindObjectOfType<PlayerStats>().GetCoins(), "coins");
        UpdatePlayerCoins(FindObjectOfType<PlayerStats>().GetCrystals(), "crystals");
        if ((float)coins >= selectedUpgrade.GetPrice() * Mathf.Pow(selectedUpgrade.GetScaling(), selectedUpgrade.GetLevel()))
        {
            
            // level cap -1 means uncapped
            if (selectedUpgrade.GetLevel() + 1 <= selectedUpgrade.GetLevelCap() || selectedUpgrade.GetLevelCap() == -1)
            {
                
                FindObjectOfType<PlayerStats>().GameEnded(Mathf.RoundToInt(selectedUpgrade.GetPrice() * Mathf.Pow(selectedUpgrade.GetScaling(), selectedUpgrade.GetLevel())),0, false, "coins");
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
