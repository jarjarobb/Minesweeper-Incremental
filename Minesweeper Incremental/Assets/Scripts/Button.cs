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
    [SerializeField] Sprite diamondTexture;
    [SerializeField] float bombRevealDelayTime = 0.5f;
    Sprite defaultTexture;
    [SerializeField]int currentRow;
    int buttonNumber;
    bool clicked;
    bool revealed;
    void Start()
    {
        defaultTexture = GetComponent<Image>().sprite;
        buttonNumber = int.Parse(name);
        currentRow = (int.Parse(name)-int.Parse(name)%FindObjectOfType<Generate>().GetAmountOfRows())/FindObjectOfType<Generate>().GetAmountOfRows();
        buttons = FindObjectOfType<Generate>().GetButtonsCanvas().gameObject;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
    }
    public int GetCurrentRow()
    {
        return currentRow;
    }    
    public void CountSurroundingBombs()
    {
        if (tag == "isBomb")
        {
            GetComponent<Image>().sprite = bombTexture;
            return;
        }
        if (tag == "isDiamond")
        {
            GetComponent<Image>().sprite = diamondTexture;
            FindObjectOfType<PlayerStats>().AwardDiamond();
        }
        for (int i = 0; i < buttons.transform.childCount; i++)
        {
            if (buttons.transform.GetChild(i).gameObject.tag == "isBomb")
            {
                if (buttons.transform.GetChild(i).GetComponent<Button>().GetCurrentRow() == currentRow)
                {
                    if (int.Parse(buttons.transform.GetChild(i).name)+1 == buttonNumber||
                        int.Parse(buttons.transform.GetChild(i).name)-1 == buttonNumber)
                    {
                        amountOfBombsSurrounding++;
                    }
                }
                if (buttons.transform.GetChild(i).GetComponent<Button>().GetCurrentRow()+1 == currentRow||
                    buttons.transform.GetChild(i).GetComponent<Button>().GetCurrentRow() - 1 == currentRow)
                {
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
        buttonText.text = amountOfBombsSurrounding.ToString();
        if (amountOfBombsSurrounding == 0)
        {
            buttonText.text = "";
        }
    }

    public void Clicked()
    {
        gameObject.GetComponent<Button>().enabled = false;
        Reveal();
    }
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
                buttons.transform.GetChild(i).GetComponent<Button>().SilentReveal();
            }
        }
        for (int i = 0; i < bombs.Count; i++)
        {
            yield return new WaitForSeconds(bombRevealDelayTime);
            bombs[i].Reveal();
        }
        
    }
    private IEnumerator Lose()
    {
        yield return new WaitForSeconds(bombRevealDelayTime*FindObjectOfType<AssignScript>().GetNumOfBombs()+bombRevealDelayTime);
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
    public void SilentReveal()
    {
        clicked = true;
        revealed = true;
    }
    private void Reveal()
    {
        if (!clicked)
        {

            if (tag == "isBomb")
            {
                print("you lose!!!");
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
            if (GetComponent<Image>().sprite ==bombTexture)
            {
                StartCoroutine(RevealAllBombs());
                return;
            }
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
    public void Flag()
    {
        if (revealed) { }
        else
        {
            if (!clicked)
            {
                GetComponent<Image>().sprite = flagTexture;
                clicked = true;

            }
            else
            {
                GetComponent<Image>().sprite = defaultTexture;
                clicked = false;
            }
            FindObjectOfType<AssignScript>().Flagged(clicked);
        }

    }
}