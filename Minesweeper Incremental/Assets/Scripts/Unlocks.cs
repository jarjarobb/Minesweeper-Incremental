using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Unlocks : MonoBehaviour
{
    //This script deals with unlocking things; no need to explain further
    [SerializeField] UnityEngine.UI.Button mineButton;
    [SerializeField] Upgrade coinsPPSUpgrade;
    [SerializeField] GameObject biggerField;

    [SerializeField] Upgrade mineUpgrade;
    [SerializeField] GameObject moreCoinsII;
    [SerializeField] GameObject moreCrystals;
    [SerializeField] GameObject coinsEL;
    [SerializeField] GameObject crystalsPPS;
    [SerializeField] GameObject portalCoins;
    [SerializeField] GameObject portalCrystals;

    [SerializeField] Upgrade crystalsPPSUpgrade;
    [SerializeField] GameObject biggerFieldII;
    
    [SerializeField] Upgrade portalCoinsUpgrade;
    [SerializeField] Upgrade portalCrystalsUpgrade;
    [SerializeField] GameObject portalKey;

    [SerializeField] Upgrade portalKeyUpgrade;

    [SerializeField] Upgrade coinsELUpgrade;
    [SerializeField] Upgrade crystalsELUpgrade;
    [SerializeField] Upgrade cloudsPPSUpgrade;
    [SerializeField] Upgrade cloudsELUpgrade;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start Scene")
        {
            StartSceneUnlocks();
        }
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            ShopUnlocks();
        }
        PlayerStatsUnlocks();
    }
    private void PlayerStatsUnlocks()
    {
        var playerStats = FindObjectOfType<PlayerStats>();
        if (coinsPPSUpgrade.GetLevel() == 1)
        {
            playerStats.EnableProfitPerSquare("coins");
        }
        if (crystalsPPSUpgrade.GetLevel() == 1)
        {
            playerStats.EnableProfitPerSquare("crystals");
        }
        if (cloudsPPSUpgrade.GetLevel() == 1)
        {
            playerStats.EnableProfitPerSquare("clouds");
        }
        if (crystalsELUpgrade.GetLevel() == 1)
        {
            playerStats.EnableEL("crystals");
        }
        if (coinsELUpgrade.GetLevel() == 1)
        {
            playerStats.EnableEL("coins");
        }
        if (cloudsELUpgrade.GetLevel() == 1)
        {
            playerStats.EnableEL("clouds");
        }
    }
    private void StartSceneUnlocks()
    {
        if (mineUpgrade.GetLevel() == 1)
        {
            mineButton.gameObject.SetActive(true);
        }
    }
    private void ShopUnlocks()
    {
        if (coinsPPSUpgrade.GetLevel() == 1)
        {
            biggerField.SetActive(true);
        }
        if (mineUpgrade.GetLevel()==1)
        {
            moreCoinsII.SetActive(true);
            moreCrystals.SetActive(true);
            coinsEL.SetActive(true);
            crystalsPPS.SetActive(true);
            portalCoins.SetActive(true);
            portalCrystals.SetActive(true);
        }
        if (crystalsPPSUpgrade.GetLevel() == 1)
        {
            biggerFieldII.SetActive(true);
        }
        if (portalCoinsUpgrade.GetLevel() == 1 && portalCrystalsUpgrade.GetLevel() == 1)
        {
            portalKey.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Start Scene")
        {
            StartSceneUnlocks();
        }
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            ShopUnlocks();
        }
        PlayerStatsUnlocks();
    }
}
