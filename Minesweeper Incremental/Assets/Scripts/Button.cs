using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Button : MonoBehaviour
{
    
    [SerializeField] GameObject buttons;
    [SerializeField] int amountOfBombsSurrounding;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] Sprite flagTexture;
    [SerializeField] Sprite bombTexture;
    [SerializeField] Sprite rockTexture;
    [SerializeField] Sprite skyTexture;
    [SerializeField] Sprite diamondTexture;
    [SerializeField] float bombRevealDelayTime = 0.5f;
    Sprite defaultTexture;
    [SerializeField]int currentRow;
    int buttonNumber;
    bool clicked;
    bool revealed;
    //This script deals with the individual buttons
    // Sets the default texture, row, number, canvas the buttons are stored in, and the starting text of the button
    void Start()
    {
        defaultTexture = GetComponent<Image>().sprite;
        buttonNumber = int.Parse(name);
        currentRow = (int.Parse(name)-int.Parse(name)%FindObjectOfType<Generate>().GetAmountOfRows())/FindObjectOfType<Generate>().GetAmountOfRows();
        buttons = FindObjectOfType<Generate>().GetButtonsCanvas().gameObject;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }
    //Counts the bombs surrounding the square
    public void CountSurroundingBombs()
    {
        //if it's a bomb then skip the function
        if (tag == "isBomb")
        {
            GetComponent<Image>().sprite = bombTexture;
            return;
        }
        //if it's a diamond then give a diamond
        if (tag == "isDiamond")
        {
            GetComponent<Image>().sprite = diamondTexture;
            FindObjectOfType<PlayerStats>().AwardDiamond();
        }
        // yeah i'm not explaining this (runs through all the squares and check if it's a bomb)
        for (int i = 0; i < buttons.transform.childCount; i++)
        {
            if (buttons.transform.GetChild(i).gameObject.tag == "isBomb")
            {
                //if the bomb's current row is the same as the square's
                if (buttons.transform.GetChild(i).GetComponent<Button>().GetCurrentRow() == currentRow)
                {
                    //if the bomb's number has a difference of 1 then increase the surrounding bombs by 1
                    if (int.Parse(buttons.transform.GetChild(i).name)+1 == buttonNumber||
                        int.Parse(buttons.transform.GetChild(i).name)-1 == buttonNumber)
                    {
                        amountOfBombsSurrounding++;
                    }
                }
                // if the row has a difference of 1
                if (buttons.transform.GetChild(i).GetComponent<Button>().GetCurrentRow()+1 == currentRow||
                    buttons.transform.GetChild(i).GetComponent<Button>().GetCurrentRow() - 1 == currentRow)
                {
                    // if the button has a difference of 1 after adding or subtracting the number of squares in a row
                    if (int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name)+FindObjectOfType<Generate>().GetAmountOfColumns() == buttonNumber||
                        int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) - FindObjectOfType<Generate>().GetAmountOfColumns() == buttonNumber||
                        int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) + FindObjectOfType<Generate>().GetAmountOfColumns()+1 == buttonNumber||
                        int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) + FindObjectOfType<Generate>().GetAmountOfColumns()-1 == buttonNumber||
                        int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) - FindObjectOfType<Generate>().GetAmountOfColumns() + 1 == buttonNumber ||
                        int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) - FindObjectOfType<Generate>().GetAmountOfColumns() - 1 == buttonNumber) 
                    {
                        amountOfBombsSurrounding++;
                    }
                }
                


            }
        }
        // sets the text showing how many bombs are surrounding
        buttonText.text = amountOfBombsSurrounding.ToString();
        // if there are no bombs surrounding then keep the default text
        if (amountOfBombsSurrounding == 0)
        {
            buttonText.text = "";
        }
    }
    // When a square is clicked
    public void Clicked()
    {
        gameObject.GetComponent<Button>().enabled = false;
        Reveal();
    }
    //Reveals all the bombs
    private IEnumerator RevealAllBombs()
    {
        List<Button> bombs = new List<Button>();
        for (int i = 0; i < buttons.transform.childCount; i++)
        {
            if (buttons.transform.GetChild(i).gameObject.tag == "isBomb")
            {
                
                bombs.Add(buttons.transform.GetChild(i).GetComponent<Button>());
            }
            else
            {
                // prevents the bug where the player can fast-click all the non-bombs and force a win
                buttons.transform.GetChild(i).GetComponent<Button>().SilentReveal();
            }
        }
        //reveals all the bombs
        for (int i = 0; i < bombs.Count; i++)
        {
            yield return new WaitForSeconds(bombRevealDelayTime);
            bombs[i].Reveal();
        }
        
    }
    // What happens when you lose
    private IEnumerator Lose()
    {
        //lets all the bombs be revealed before executing the code below
        yield return new WaitForSeconds(bombRevealDelayTime*FindObjectOfType<AssignScript>().GetNumOfBombs()+bombRevealDelayTime);
        //The rest of this function is dedicated to calculating how much coins to give and sending it to PlayerStats to give coins
        int amountOfButtonsRevealed = FindObjectOfType<AssignScript>().GetButtonsRevealed();
        int amountOfCoinsToGive = Mathf.RoundToInt(10f * (float)amountOfButtonsRevealed/
            (float)FindObjectOfType<AssignScript>().GetTotalNonBombs());
        
        if (SceneManager.GetActiveScene().name == "The Mine Game Scene")
        {
            FindObjectOfType<PlayerStats>().GameEnded(amountOfCoinsToGive, amountOfButtonsRevealed, true, "crystals");
            FindObjectOfType<SceneLoader>().LoadTheMineLoseScene();
            
        }
        else
        {
            FindObjectOfType<PlayerStats>().GameEnded(amountOfCoinsToGive,amountOfButtonsRevealed, true, "coins");
            FindObjectOfType<SceneLoader>().LoadLoseScene();
            
        }
    }
    // Reveals the square without showing to prevent player losing forcing a win bug and player winning accidentaly lose bug
    public void SilentReveal()
    {
        clicked = true;
        revealed = true;
    }
    // Reveals the square
    private void Reveal()
    {
        // makes sure it was never clicked before
        if (!clicked)
        {
            // if it is a bomb then lose; if it is not a bomb then tell Assign to transfer an unclicked square to a clicked square
            if (tag == "isBomb")
            {
                StartCoroutine(Lose());
            }
            else
            {
                StartCoroutine(FindObjectOfType<AssignScript>().nonBombClicked());
            }
            GetComponent<Image>().sprite = rockTexture;
            clicked = true;
            revealed = true;
            CountSurroundingBombs();
            // if it is a bomb then reveal all bombs
            if (tag == "isBomb")
            {
                StartCoroutine(RevealAllBombs());
                return;
            }
            // if there are no bombs surrounding then reveal all the squares surrounding this one
            if (GetComponentInChildren<TextMeshProUGUI>().text == "")
            {
                for (int i = 0; i < buttons.transform.childCount; i++)
                {
                    
                        
                            if (buttons.transform.GetChild(i).GetComponent<Button>().GetCurrentRow() == currentRow)
                            {
                                if (int.Parse(buttons.transform.GetChild(i).name) + 1 == buttonNumber ||
                                    int.Parse(buttons.transform.GetChild(i).name) - 1 == buttonNumber)
                                {
                                buttons.transform.GetChild(i).GetComponent<Button>().Reveal();
                                }
                            }
                            if (buttons.transform.GetChild(i).GetComponent<Button>().GetCurrentRow() + 1 == currentRow ||
                                buttons.transform.GetChild(i).GetComponent<Button>().GetCurrentRow() - 1 == currentRow)
                            {
                                if (int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) + FindObjectOfType<Generate>().GetAmountOfColumns() == buttonNumber ||
                                    int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) - FindObjectOfType<Generate>().GetAmountOfColumns() == buttonNumber ||
                                    int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) + FindObjectOfType<Generate>().GetAmountOfColumns() + 1 == buttonNumber ||
                                    int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) + FindObjectOfType<Generate>().GetAmountOfColumns() - 1 == buttonNumber ||
                                    int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) - FindObjectOfType<Generate>().GetAmountOfColumns() + 1 == buttonNumber ||
                                    int.Parse(buttons.transform.GetChild(i).GetComponent<Button>().name) - FindObjectOfType<Generate>().GetAmountOfColumns() - 1 == buttonNumber)
                                {
                                buttons.transform.GetChild(i).GetComponent<Button>().Reveal();
                            }


                            }
                        
                    
                }
            }
        }

    }
    //Function for flagging
    public void Flag()
    {
        //it can't be revealed
        if (!revealed)
        {
            // clicked is used as flagged (switches between the flag sprite and the default sprite)
            if (!clicked)
            {
                GetComponent<Image>().sprite = flagTexture;
            }
            else
            {
                GetComponent<Image>().sprite = defaultTexture;
            }
            clicked = !clicked;
            // tells assign that a square has been flagged
            FindObjectOfType<AssignScript>().Flagged(clicked);
        }

    }
    //return function
    public int GetCurrentRow()
    {
        return currentRow;
    }
}