using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int PlayerMaxLives;

    [SerializeField] private int PlayerLivesLeft;
    private GameObject PlayerInstance;
    private List<GameObject> EnemyList = new List<GameObject>();


    private void Awake()
    { 
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerLivesLeft = PlayerMaxLives;
    }

    private void OnLevelWasLoaded(int level)
    {
        // Find player in the scene if they exist
        PlayerScript playerScript = FindObjectOfType<PlayerScript>();
        if (playerScript != null) PlayerInstance = playerScript.gameObject;

        // Find enemies in the scene if they exist
        EnemyScript[] enemyScripts = FindObjectsByType<EnemyScript>(FindObjectsSortMode.None);
        EnemyList.Clear();
        foreach (EnemyScript enemy in enemyScripts) EnemyList.Add(enemy.gameObject);
    }

    private void GameOver() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void BattleOver (bool isWinner) 
    {
        if (!isWinner) PlayerLivesLeft--;

        if (PlayerLivesLeft <= 0) 
        { 
            GameOver();
        }
    }

    public void SpawnCharacters() 
    { 
    
    }

    public void DestroyCharacter(GameObject character) 
    {
        if (character == null) return;

        if (character == PlayerInstance)
        {
            PlayerInstance = null;
            foreach (GameObject enemy in EnemyList) 
            { 
                enemy.GetComponent<EnemyScript>().defaultMovement = false;
            }
            BattleOver(false);
        }

        if (character.GetComponent<EnemyScript>() != null) 
        { 
            EnemyList.Remove(character);
            if (EnemyList.Count == 0 ) 
            {
                BattleOver(true);
            }
        }

        Destroy(character);
    }
}
