using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] int minePrice = 25;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadLoseScene()
    {
        SceneManager.LoadScene("Lose Scene");
    }
    public void LoadStartScene(bool loadPreviousScene)
    {
        FindObjectOfType<PlayerStats>().ShowSettingsButton();
        if (loadPreviousScene)
        {
           
            SceneManager.LoadScene(FindObjectOfType<PlayerStats>().GetPreviousScene());
        }
        else
        {
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
        FindObjectOfType<PlayerStats>().StorePreviousScene(SceneManager.GetActiveScene().name);
        FindObjectOfType<PlayerStats>().HideSettingsButton();
        SceneManager.LoadScene("Settings");
    }
    public void LoadShop()
    {
        FindObjectOfType<PlayerStats>().StorePreviousScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Shop");
    }
    public void LoadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    public void LoadTheMineStartScene(bool crossDimension)
    {
       
        if (FindObjectOfType<PlayerStats>().GetCoins() >= minePrice && crossDimension)
        {
            FindObjectOfType<PlayerStats>().DimensionCrossed("The Mine");
            FindObjectOfType<PlayerStats>().GameEnded(25,0, false, "coins");
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
}
