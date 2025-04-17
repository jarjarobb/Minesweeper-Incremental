using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Unlocks : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button mineButton;
    [SerializeField] GameObject biggerFieldUpgrade;
    [SerializeField] Upgrade mineUpgrade;
    [SerializeField] Upgrade coinsPPSUpgrade;
    [SerializeField] GameObject moreCoinsIIUpgrade;
    [SerializeField] GameObject crystalsUpgrade;
    [SerializeField] GameObject coinsELUpgrade;
    [SerializeField] Upgrade crystalsPPSUpgrade;
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
            biggerFieldUpgrade.SetActive(true);
        }
        if (mineUpgrade.GetLevel()==1)
        {
            moreCoinsIIUpgrade.SetActive(true);
            crystalsUpgrade.SetActive(true);
            coinsELUpgrade.SetActive(true);
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
    }
}
