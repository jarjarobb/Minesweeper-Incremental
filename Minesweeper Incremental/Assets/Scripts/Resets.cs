using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Resets : MonoBehaviour
{
    public void EndgameReset()
    {
        var playerStats = FindObjectOfType<PlayerStats>();
        List<Upgrade> upgrades = new List<Upgrade>();
        foreach (Upgrade upgrade in playerStats.GetUpgrades())
        {
            if (upgrade.name == "Endgame")
            {
                print("endgame upgrade found");
                if (upgrade.GetLevel() >= 1)
                {
                    playerStats.AddEndgameTokens(upgrade.GetLevel());
                }
                else
                {
                    return;
                }
            }
            if (!upgrade.GetNotResetOnEndgame())
            {
                upgrades.Add(upgrade);
            }
        }
        foreach (Upgrade upgrade in upgrades) 
        {
            upgrade.ResetLevel();
        }
        
        playerStats.GameEnded((float)playerStats.GetCoins(), 0, false, "coins");
        playerStats.GameEnded((float)playerStats.GetCrystals(), 0, false, "crystals");
        while (playerStats.GetDiamonds() >0)
        {
            playerStats.SpendDiamond();
        }
        playerStats.GameEnded((float)playerStats.GetClouds(), 0, false, "clouds");
       
        FindObjectOfType<Unlocks>().EndgamePerformed();
        playerStats.IncreaseEndgame();
        StartCoroutine(FindObjectOfType<Cutscene>().EndgameCutscene(playerStats.GetEndgame()));

    }
}
