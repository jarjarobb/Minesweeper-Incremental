using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Generate : MonoBehaviour
{
    [SerializeField] Button fieldButton;
    [SerializeField] int amountOfColumns = 5;
    [SerializeField] int amountOfRows = 5;
    [SerializeField] Transform buttonsCanvas;
    [SerializeField] Canvas SettingsCanvas;
    [SerializeField] string align;
    [SerializeField] Upgrade largerCoinBoardUpgrade;
    [SerializeField] Upgrade largerCrystalBoardUpgrade;
    [SerializeField] Upgrade largerCloudBoardUpgrade;
    float buttonOffset = 25;
    float buttonDistance = 37.5f;
    Button newButton;
    [SerializeField] List<Button> buttons;
    bool gameStarted;

    //This script deals with generating the squares for the game

    // Start is called before the first frame update
    void Start()
    {
        //Changes the amount of columns and rows depending on the respecting upgrade for the scene, then generates a board
        if (SceneManager.GetActiveScene().name == "Game Scene")
        {
            amountOfColumns += largerCoinBoardUpgrade.GetLevel();
            amountOfRows = amountOfColumns;
        }
        if (SceneManager.GetActiveScene().name == "The Mine Game Scene")
        {
            amountOfColumns += largerCrystalBoardUpgrade.GetLevel();
            amountOfRows = amountOfColumns;
        }
        if (SceneManager.GetActiveScene().name == "Heaven Game Scene")
        {
            amountOfColumns += largerCloudBoardUpgrade.GetLevel();
            amountOfRows = amountOfColumns;
        }
        GenerateBoard();

    }
    
    private void GenerateBoard()
    {
        // default is align to the bottom left, but the align variable will determine if it needs to be in the center
        if (align == "center")
        {
            // runs through the columns and rows to create a square
            for (int i = 0; i < amountOfRows; i++)
            {
                for (int j = 0; j < amountOfColumns; j++)
                {
                    newButton = Instantiate(fieldButton, buttonsCanvas);
                    Vector3 canvasCenter = newButton.transform.parent.position;
                    Vector3 startingAnchorPosition = canvasCenter - new Vector3((float)amountOfRows / 2f*buttonDistance, (float)amountOfColumns / 2f*buttonDistance, 0);
                    Vector3 newButtonPos = startingAnchorPosition + new Vector3(
                        j * buttonDistance + buttonOffset,
                        i * buttonDistance + buttonOffset,
                        fieldButton.transform.position.z);
                    newButton.transform.position = newButtonPos;
                    // Sets the name of the button to be the iterator numbers of both j and i
                    newButton.name = (j + i * amountOfRows).ToString();
                    buttons.Add(newButton);
                }

            }
        }
        //exactly the same except the anchor will be (0,0)
        else
        {
            for (int i = 0; i < amountOfColumns; i++)
            {
                for (int j = 0; j < amountOfRows; j++)
                {
                    newButton = Instantiate(fieldButton, buttonsCanvas);
                    Vector3 newButtonPos = new Vector3(
                        j * buttonDistance + buttonOffset,
                        i * buttonDistance + buttonOffset,
                        fieldButton.transform.position.z);
                    newButton.transform.position = newButtonPos;
                    newButton.name = (j + i * amountOfRows).ToString();
                    if (int.Parse(newButton.name) % 2 == 0)
                    {
                        
                        print("even");
                    }
                    else
                    {
                        
                        print("odd");
                    }
                    buttons.Add(newButton);
                }

            }
        }
    }
    //return functions
    public int GetAmountOfColumns()
    {
        return amountOfColumns;
    }
    public List<Button> GetButtons()
    {
        return buttons;
    }
    public Transform GetButtonsCanvas()
    {
        return buttonsCanvas;
    }
    public int GetAmountOfRows()
    {
        return amountOfRows;
    }
}