using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{

    [SerializeField] TMP_Text Title;
    [SerializeField] TMP_Text PlayerData;
    [SerializeField] Image VictoryImage;
    [SerializeField] Button LibraryButton;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLibraryClicked()
    {
        GameManager.Instance.LoadLibrary();
    }

    public void OnMainMenuClicked()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void SetUIElements(bool playerWon) 
    { 
        if (playerWon)
        {
            Title.text = "You Won!";
        } else 
        {
            Title.text = "You Lost :(";
        }

        PlayerData.text =   "Lives Left: " + GameManager.Instance.PlayerLivesLeft +
                            "\nRounds Won: " + GameManager.Instance.RoundsCompleted;

        // Force player to exit to main menu when out of lives
        if (GameManager.Instance.PlayerLivesLeft <= 0) LibraryButton.interactable = false;
    }
}
