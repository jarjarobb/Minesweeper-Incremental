using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

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
    [SerializeField] UnityEngine.UI.Button heavenButton;
    [SerializeField] GameObject moreCoinsIII;
    [SerializeField] GameObject moreCrystalsII;
    [SerializeField] GameObject moreClouds;
    [SerializeField] GameObject crystalsEL;
    [SerializeField] GameObject cloudsPPS;

    [SerializeField] Upgrade cloudsPPSUpgrade;
    [SerializeField] GameObject biggerFieldIII;
    [SerializeField] GameObject coinGeneration;

    [SerializeField] Upgrade coinsELUpgrade;
    [SerializeField] Upgrade crystalsELUpgrade;
    [SerializeField] Upgrade cloudsELUpgrade;
    [SerializeField] Upgrade coinGenerationUpgrade;
    [SerializeField] Upgrade moreCoinsUpgrade;
    [SerializeField] Upgrade moreCoinsIIUpgrade;
    [SerializeField] Upgrade moreCoinsIIIUpgrade;
    [SerializeField] Upgrade crystalsUpgrade;
    [SerializeField] Upgrade cloudsUpgrade;
    [SerializeField] Upgrade crystalsIIUpgrade;
    [SerializeField] GameObject endgame;

    [SerializeField] Upgrade endgameUpgrade;
    [SerializeField] bool endgamed;
    [SerializeField] UnityEngine.UI.Button endgameButton;
    [SerializeField] GameObject globalBoost;
    [SerializeField] GameObject cloudsEL;
    [SerializeField] GameObject crystalGen;
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
        var settingsCanvas = playerStats.transform.Find("SettingsCanvas");
        var statsCanvasOrderer = playerStats.transform.Find("StatsCanvas").Find("Scroll View").Find("Orderer");
        if (mineUpgrade.GetLevel() == 1)
        {
            settingsCanvas.transform.Find("Setting (1)").gameObject.SetActive(true);
            statsCanvasOrderer.transform.Find("Crystals").gameObject.SetActive(true);
        }
        if (portalKeyUpgrade.GetLevel() == 1)
        {
            settingsCanvas.transform.Find("Setting (3)").gameObject.SetActive(true);
            settingsCanvas.transform.Find("Setting (4)").gameObject.SetActive(true);
            statsCanvasOrderer.transform.Find("Diamonds").gameObject.SetActive(true);
            statsCanvasOrderer.transform.Find("Clouds").gameObject.SetActive(true);
        }
        if (endgameUpgrade.GetLevel() >= 1)
        {
            settingsCanvas.transform.Find("Setting (5)").gameObject.SetActive(true);
            statsCanvasOrderer.transform.Find("Endgames").gameObject.SetActive (true);
            statsCanvasOrderer.transform.Find("EndgameTokens").gameObject.SetActive(true);
        }
    }
    private void StartSceneUnlocks()
    {
        if (mineUpgrade.GetLevel() == 1)
        {
            mineButton.gameObject.SetActive(true);
        }
        if (portalKeyUpgrade.GetLevel() == 1)
        {
            heavenButton.gameObject.SetActive(true);
        }
        if (endgameUpgrade.GetLevel() >= 1 || endgamed)
        {
            endgameButton.gameObject.SetActive(true);
        }
    }
    public void EndgamePerformed()
    {
        endgamed = true;
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
        if (portalKeyUpgrade.GetLevel() == 1)
        {
            moreCoinsIII.SetActive(true);
            moreCrystalsII.SetActive(true);
            moreClouds.SetActive(true);
            crystalsEL.SetActive(true);
            cloudsPPS.SetActive(true);
        }
        if (cloudsPPSUpgrade.GetLevel() == 1)
        {
            biggerFieldIII.SetActive(true);
            if (coinsPPSUpgrade.GetLevel() == 1)
            {
                coinGeneration.SetActive(true);
            }
        }
        if (coinGenerationUpgrade.GetLevel() >= 1 && coinsELUpgrade.GetLevel() == 1 &&
            crystalsELUpgrade.GetLevel() == 1 &&
            moreCoinsUpgrade.GetLevel() >= 1 && moreCoinsIIUpgrade.GetLevel() >= 1 &&
            moreCoinsIIIUpgrade.GetLevel() >= 1 && crystalsUpgrade.GetLevel() >= 1 && 
            crystalsIIUpgrade.GetLevel() >= 1 && cloudsUpgrade.GetLevel() >= 1)
        {
            endgame.SetActive(true);
        }
        if (FindObjectOfType<PlayerStats>().GetEndgame() >= 1)
        {
            globalBoost.SetActive(true);
            cloudsEL.SetActive(true);
            crystalGen.SetActive(true);
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
