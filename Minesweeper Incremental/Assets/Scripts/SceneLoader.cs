using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] int minePrice = 25;
    [SerializeField] int heavenPrice = 1;
    
    // This whole script deals with scene loading

    public void LoadLoseScene()
    {
        SceneManager.LoadScene("Lose Scene");
    }
    //this function is ran with true if you want to load the previous scene
    public void LoadStartScene(bool loadPreviousScene)
    {
        FindObjectOfType<PlayerStats>().ShowSettingsButton();
        FindObjectOfType<PlayerStats>().HideEndgameDisplay();
        if (loadPreviousScene)
        {
           if (FindObjectOfType<Cutscene>())
            {
                FindObjectOfType<Cutscene>().SelfDestruct();
            }
            SceneManager.LoadScene(FindObjectOfType<PlayerStats>().GetPreviousScene());
        }
        else
        {
            if (FindObjectOfType<Cutscene>())
            {
                FindObjectOfType<Cutscene>().SelfDestruct();
            }
            FindObjectOfType<PlayerStats>().DimensionCrossed("normal");
            SceneManager.LoadScene("Start Scene");
        }
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game Scene");
    }
    public void LoadWinScene()
    {
        SceneManager.LoadScene("Win Scene");
    }
    public void LoadSettings()
    {
        if (FindObjectOfType<PlayerStats>().GetSettingsCanvas().gameObject.activeSelf)
        {
            FindObjectOfType<PlayerStats>().ShowSettingsButton();
        }
        else
        {
            FindObjectOfType<PlayerStats>().HideSettingsButton(true);
        }
    }
    public void LoadShop()
    {
        //sets the scene before entering the shop
        FindObjectOfType<PlayerStats>().StorePreviousScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Shop");
    }
    public void LoadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    public void LoadTheMineStartScene(bool crossDimension)
    {
       // makes you pay if you are crossing a dimension
        if (FindObjectOfType<PlayerStats>().GetCoins() >= minePrice && crossDimension)
        {
            FindObjectOfType<PlayerStats>().DimensionCrossed("The Mine");
            FindObjectOfType<PlayerStats>().GameEnded(minePrice,0, false, "coins");
            SceneManager.LoadScene("The Mine Start Scene");
        }
        else if (!crossDimension)
        {
            SceneManager.LoadScene("The Mine Start Scene");
        }
    }
    public void LoadTheMineGameScene()
    {
        SceneManager.LoadScene("The Mine Game Scene");
    }
    public void LoadTheMineWinScene()
    {
        SceneManager.LoadScene("The Mine Win Scene");
    }
    public void LoadTheMineLoseScene()
    {
        SceneManager.LoadScene("The Mine Lose Scene");
    }
    public void LoadHeavenStartScene(bool crossDimension)
    {
        //makes you pay if you are crossing a dimension
        if (FindObjectOfType<PlayerStats>().GetDiamonds() >= heavenPrice && crossDimension)
        {
            FindObjectOfType<PlayerStats>().DimensionCrossed("Heaven");
            FindObjectOfType<PlayerStats>().SpendDiamond();
            SceneManager.LoadScene("Heaven Start Scene");
        }
        else if (!crossDimension)
        {
            SceneManager.LoadScene("Heaven Start Scene");
        }
    }
    public void LoadHeavenGameScene()
    {
        SceneManager.LoadScene("Heaven Game Scene");
    }
    public void LoadHeavenWinScene()
    {
        SceneManager.LoadScene("Heaven Win Scene");
    }
    public void LoadHeavenLoseScene()
    {
        SceneManager.LoadScene("Heaven Lose Scene");
    }
    public void LoadEndgame()
    {
        FindObjectOfType<PlayerStats>().ShowEndgameDisplay();
        SceneManager.LoadScene("Endgame");
    }
    public void LoadRealWinScene()
    {
        SceneManager.LoadScene("REAL Win Scene");
    }
}


