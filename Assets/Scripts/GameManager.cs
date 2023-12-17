using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    // Represents spawn of one enemy
    [Serializable]
    public struct EnemySpawnInfo {
        public Vector2 SpawnPoint;
        public GameObject EnemyPrefab;
    }

    // Represents player and enemy spawns for a round of battle
    [Serializable]
    public struct SpawnInfo {
        public Vector2 PlayerSpawn;
        public GameObject PlayerPrefab;
        public List<EnemySpawnInfo> Enemies;
    }

    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject AttackCanvasPrefab;
    [SerializeField] private GameObject EnemeyAttacksPrefab;
    [SerializeField] private GameObject EndBattleUI;
    [SerializeField] private int PlayerMaxLives;

    // List the player and enemy spawns for each round
    // Add a new element in the Unity editor to easily add a new round to the game
    public List<SpawnInfo> Rounds;
    public SpawnInfo LibrarySpawnInfo;
    public int PlayerLivesLeft { get; private set; }
    public int RoundsCompleted { get; private set; }

    private GameObject PlayerInstance;
    private List<GameObject> EnemyInstances = new List<GameObject>();


    private void Awake()
    {
        // Destroy this instance if GameManager already exists
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Set defaults for persistent data
        ResetData();
    }

    private void OnLevelWasLoaded(int level)
    {
        // Destroy this instance if GameManager already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (level == 2)
        {
            // Loaded into combat level; spawn player & enemies
            SpawnCharacters();
        }
        else 
        {
            // Find player in the scene if they exist
            PlayerScript playerScript = FindObjectOfType<PlayerScript>();
            if (playerScript != null) PlayerInstance = playerScript.gameObject;
        }
    }

    // Reset persistent data to their default values
    void ResetData() 
    {
        PlayerLivesLeft = PlayerMaxLives;
        RoundsCompleted = 0;
        PlayerInstance = null;
        EnemyInstances.Clear();
    }

    private void GameOver(bool isWinner) 
    {
        UnityEngine.Debug.Log("Game is over. Did the player win?: " + isWinner);
        LoadMainMenu();
    }

    private void BattleOver (bool isWinner) 
    {
        if (!isWinner)
        {
            PlayerLivesLeft--;
            if (PlayerLivesLeft <= 0)
            {
                GameOver(false);
                return;
            }
        }
        else 
        {
            RoundsCompleted++;
        }
        BattleUIManager battleUI = Instantiate(EndBattleUI).GetComponent<BattleUIManager>();
        battleUI.SetUIElements(isWinner);

    }

    void AttachAttackImages() 
    {
        if (AttackCanvasPrefab == null) return;

        GameObject AttackCanvasInstance = Instantiate(AttackCanvasPrefab);
        PlayerInstance.GetComponent<PlayerScript>().keybindImages = AttackCanvasInstance.GetComponentsInChildren<Image>();
    }

    public void SpawnCharacters() 
    {
        // Spawn player
        SpawnInfo thisRound = Rounds[RoundsCompleted];
        PlayerInstance = Instantiate(thisRound.PlayerPrefab, thisRound.PlayerSpawn, Quaternion.identity);
        AttachAttackImages();

        // Spawn enemies
        EnemySpawnInfo thisEnemy = thisRound.Enemies[0];
        EnemyInstances.Add(Instantiate(thisEnemy.EnemyPrefab, thisEnemy.SpawnPoint, Quaternion.identity));
        EnemyInstances[0].GetComponent<EnemyScript>().EnemyAttacksDisplay = Instantiate(EnemeyAttacksPrefab).transform;

        EnemyInstances[0].GetComponent<EnemyScript>().playMusic = true; // Only have first enemy play their music

        for (int i = 1; i < thisRound.Enemies.Count; i++) 
        {
            thisEnemy = thisRound.Enemies[i];
            EnemyInstances.Add(Instantiate(thisEnemy.EnemyPrefab, thisEnemy.SpawnPoint, Quaternion.identity));
            EnemyInstances[i].GetComponent<EnemyScript>().EnemyAttacksDisplay = Instantiate(EnemeyAttacksPrefab).transform;
        }
    }

    public void LoadMainMenu()
    {
        ResetData();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLibrary() 
    {
        // Go to library if won or still have lives left after losing
        PlayerInstance = null;
        EnemyInstances.Clear();
        SceneManager.LoadScene("LibraryScene");
    }

    public void LoadCombat() 
    {
        if (RoundsCompleted >= Rounds.Count)
        {
            GameOver(true);
        }
        else
        {
            SceneManager.LoadScene("BattleScene1");
        }
    }

    public void DestroyCharacter(GameObject character) 
    {
        if (character == null) return;

        if (character == PlayerInstance)
        {
            PlayerInstance = null;
            foreach (GameObject enemy in EnemyInstances) 
            { 
                enemy.GetComponent<EnemyScript>().defaultMovement = false;
            }
            BattleOver(false);
        }

        if (character.GetComponent<EnemyScript>() != null) 
        {
            EnemyInstances.Remove(character);
            if (EnemyInstances.Count == 0 ) 
            {
                BattleOver(true);
            }
        }

        Destroy(character);
    }
}
