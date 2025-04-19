using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AssignScript : MonoBehaviour
{
    [SerializeField] Canvas gameCanvas;
    [SerializeField] List<Button> buttons;
    [SerializeField] int numOfBombs;
    [SerializeField] TextMeshProUGUI bombsLeftText;
    [SerializeField] int bombsLeft;
    [SerializeField] int nonBombsLeftUnclicked;
    [SerializeField] int nonBombsClicked;
    [SerializeField] int bombNum;
    [SerializeField] int diamondNum;
    [SerializeField] int defaultAmountOfCoins = 10;
    [SerializeField] int totalNonBombTiles;
    [SerializeField] float defaultWinWaitTime = 0.5f;
    [SerializeField] List<Button> bombs;
    [SerializeField] Button diamond;
    [SerializeField] Upgrade largerCoinBoardUpgrade;
    [SerializeField] Upgrade largerCrystalsBoardUpgrade;
    [SerializeField] Upgrade largerCloudsBoardUpgrade;
    [SerializeField] Upgrade portalCoinsUpgrade;
    [SerializeField] Upgrade portalCrystalsUpgrade;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Checks the current scene and assigns uses the upgrade for the scene for more bombs
        if (SceneManager.GetActiveScene().name == "Game Scene")
        {
            numOfBombs += 2 * largerCoinBoardUpgrade.GetLevel();
        }
        else if (SceneManager.GetActiveScene().name == "The Mine Game Scene")
        {
            numOfBombs += 2 * largerCrystalsBoardUpgrade.GetLevel();
        }
        else if (SceneManager.GetActiveScene().name == "Heaven Game Scene")
        {
            numOfBombs += 2 * largerCloudsBoardUpgrade.GetLevel();
        }
        bombsLeft = numOfBombs;
        bombsLeftText.text = bombsLeft.ToString();
        AddToButtons();
        yield return new WaitForEndOfFrame();
        SetBombs();
        SetDiamond();
        StartCoroutine(SetNonBombsLeftUnclicked());
    }
    // Sets the diamond in The Mine
    private void SetDiamond()
    {
        if (portalCoinsUpgrade.GetLevel() == 1 && portalCrystalsUpgrade.GetLevel() == 1)
        {
            if (SceneManager.GetActiveScene().name == "The Mine Game Scene")
            {
                int chance = Random.Range(1, 5);
                if (chance == 1)
                {
                    buttons = FindObjectOfType<Generate>().GetButtons();
                    diamondNum = Random.Range(0, buttons.Count);
                    if (buttons[diamondNum].gameObject.tag != "isDiamond")
                    {
                        buttons[diamondNum].gameObject.tag = "isDiamond";
                    }
                    else
                    {
                        SetDiamond();
                    }
                    diamond = buttons[diamondNum];
                }
            }
        }
    }
    // Really obvious
    private IEnumerator SetNonBombsLeftUnclicked()
    {
        yield return new WaitForEndOfFrame();
        nonBombsLeftUnclicked = gameCanvas.transform.childCount - numOfBombs;
        totalNonBombTiles = nonBombsLeftUnclicked;
    }
    // Takes the buttons from Generate and stores it in a variable
    private void AddToButtons()
    {
        buttons = FindObjectOfType<Generate>().GetButtons();
    }
    // Sets a bomb for the game
    private void SetBomb()
    {
        // just in case
        buttons = FindObjectOfType<Generate>().GetButtons();
        bombNum = Random.Range(0, buttons.Count);
        if (buttons[bombNum].gameObject.tag != "isBomb")
        {
            buttons[bombNum].gameObject.tag = "isBomb";
        }
        else
        {
            SetBomb();
        }
        bombs.Add(buttons[bombNum]);
    }
    // Repeatedly sets bombs depending on the variable numOfBombs
    private void SetBombs()
    {
        for (int i = 0; i < numOfBombs; i++)
        {
            SetBomb();
        }
    }
    // When a player flags a square, it will change the display
    public void Flagged(bool flagging)
    {
        if (flagging)
        {
            bombsLeft -= 1;
        }
        else
        {
            bombsLeft += 1;
        }
        bombsLeftText.text = bombsLeft.ToString();
    }
    // What happens when a square that is not a bomb clicked
    public IEnumerator nonBombClicked()
    {
        // Transfers a square from unclicked to clicked
        nonBombsLeftUnclicked--;
        nonBombsClicked++;
        // If all the squares are clicked
        if (nonBombsLeftUnclicked <= 0)
        {
            foreach (var bomb in bombs)
            {
                // Reveals the bomb without showing to prevent loss from random click
                bomb.SilentReveal();
            }
            //waits a few seconds (probably for a cutscene or something idk)
            yield return new WaitForSeconds(defaultWinWaitTime);
            //awards different currencies based on the scene
            if (SceneManager.GetActiveScene().name == "The Mine Game Scene")
            {
                FindObjectOfType<PlayerStats>().GameEnded(defaultAmountOfCoins, nonBombsClicked,true, "crystals");
                FindObjectOfType<SceneLoader>().LoadTheMineWinScene();
            }
            else if (SceneManager.GetActiveScene().name == "Heaven Game Scene")
            {
                FindObjectOfType<PlayerStats>().GameEnded(defaultAmountOfCoins, nonBombsClicked, true, "clouds");
                FindObjectOfType<SceneLoader>().LoadHeavenWinScene();
            }
            else if (SceneManager.GetActiveScene().name == "Game Scene")
            {
                FindObjectOfType<PlayerStats>().GameEnded(defaultAmountOfCoins, nonBombsClicked, true, "coins");
                FindObjectOfType<SceneLoader>().LoadWinScene();
            }
        }
    }
    //return functions
    public int GetBombNum()
    {
        return bombNum;
    }
    public int GetButtonsRevealed()
    {
        return nonBombsClicked;
    }
    public int GetNumOfBombs()
    {
        return numOfBombs;
    }
    public int GetTotalNonBombs()
    {
        return totalNonBombTiles;
    }
}