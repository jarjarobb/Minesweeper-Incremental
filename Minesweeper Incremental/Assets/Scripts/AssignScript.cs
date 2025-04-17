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
    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (SceneManager.GetActiveScene().name == "Game Scene")
        {
            numOfBombs += 2 * largerCoinBoardUpgrade.GetLevel();
        }
        bombsLeft = numOfBombs;
        bombsLeftText.text = bombsLeft.ToString();
        AddToButtons();
        yield return new WaitForEndOfFrame();
        SetBombs();
        SetDiamond();
        StartCoroutine(SetNonBombsLeftUnclicked());
    }

    private void SetDiamond()
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

    private IEnumerator SetNonBombsLeftUnclicked()
    {
        yield return new WaitForEndOfFrame();
        nonBombsLeftUnclicked = gameCanvas.transform.childCount - numOfBombs;
        totalNonBombTiles = nonBombsLeftUnclicked;
    }
    private void AddToButtons()
    {
        buttons = FindObjectOfType<Generate>().GetButtons();
    }
    private void SetBomb()
    {
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
    private void SetBombs()
    {
        for (int i = 0; i < numOfBombs; i++)
        {
            SetBomb();
        }
        /*Button[] buttonss = FindObjectsOfType<Button>();
        for (int i = 0; i < buttonss.Length; i++)
        {
            buttonss[i].CountSurroundingBombs();
        }*/
    }
    // Update is called once per frame
    void Update()
    {

    }
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
    public IEnumerator nonBombClicked()
    {
        nonBombsLeftUnclicked--;
        nonBombsClicked++;
        if (nonBombsLeftUnclicked <= 0)
        {
            foreach (var bomb in bombs)
            {
                bomb.SilentReveal();
            }
            yield return new WaitForSeconds(defaultWinWaitTime);
            
            if (SceneManager.GetActiveScene().name == "The Mine Game Scene")
            {
                FindObjectOfType<PlayerStats>().GameEnded(defaultAmountOfCoins, nonBombsClicked,true, "crystals");
                FindObjectOfType<SceneLoader>().LoadTheMineWinScene();
            }
            else
            {
                FindObjectOfType<PlayerStats>().GameEnded(defaultAmountOfCoins, nonBombsClicked, true, "coins");
                FindObjectOfType<SceneLoader>().LoadWinScene();
            }
        }
    }
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