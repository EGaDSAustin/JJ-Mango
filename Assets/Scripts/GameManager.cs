using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct EnemySpawnInfo {
        public Vector2 SpawnPoint;
        public GameObject EnemyPrefab;
    }

    [Serializable]
    public struct SpawnInfo {
        public Vector2 PlayerSpawn;
        public GameObject PlayerPrefab;
        public List<EnemySpawnInfo> Enemies;
    }

    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject AttackCanvasPrefab;
    [SerializeField] private int PlayerMaxLives;
    public List<SpawnInfo> Rounds;
    public SpawnInfo LibrarySpawnInfo;

    private int PlayerLivesLeft;
    private int RoundsCompleted;

    private GameObject PlayerInstance;
    private List<GameObject> EnemyInstances = new List<GameObject>();


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
        RoundsCompleted = 0;
    }

    private void OnLevelWasLoaded(int level)
    {
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

    private void GameOver(bool isWinner) 
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void BattleOver (bool isWinner) 
    {
        if (isWinner)
        {
            RoundsCompleted++;
            SceneManager.LoadScene("LibraryScene");
        }
        else 
        {
            PlayerLivesLeft--;
            if (PlayerLivesLeft <= 0)
            {
                GameOver(false);
            }
        }
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
        EnemyInstances[0].GetComponent<EnemyScript>().playMusic = true; // Only have first enemy play their music
        for (int i = 1; i < thisRound.Enemies.Count; i++) 
        {
            thisEnemy = thisRound.Enemies[i];
            EnemyInstances.Add(Instantiate(thisEnemy.EnemyPrefab, thisEnemy.SpawnPoint, Quaternion.identity));
        }
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
