using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
public class RTBGameController : GameController
{
    #region Members

    public RTBUIController ui;
    public List<BlockSpawnController> spawnPoints;
    public TowerFallHandler fallHandler;
    public int maxPlayers;
    public float endTurnTimer;

    private List<PlayerPickupController> players = null;
    private List<Spawner> spawnerList = null;
    private List<Rigidbody> blocks = null;
    private float currEndTurnTimer = 0f;

    [SyncVar(hook = "OnTurnUpdate")]
    private int playerTurn = -1;
    [SyncVar(hook = "OnEndTimerUpdate")]
    private string EndTurnTimerText = "";
    [SyncVar(hook = "OnHeightUpdate")]
    public float towerHeight;
    [SyncVar(hook = "OnGameOver")]
    private bool isGameOver = false;

    #endregion

    #region Unity Methods

    void Start()
    {
        if (!isServer)
        {
            return;
        }

        // Create player list
        players = new List<PlayerPickupController>();

        // Create spawn point list
        spawnerList = new List<Spawner>();
        foreach (BlockSpawnController spawnPoint in spawnPoints)
        {
            spawnerList.Add(new Spawner(spawnPoint, null));
        }

        // Create block list
        blocks = new List<Rigidbody>();

        // Register callbacks
        fallHandler.RegisterTowerFallHandler(OnTowerFall);
        ui.RegisterTimerCallback(GameOver);
    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }

        PlayGame();
    }

    #endregion

    #region Public Methods

    public override void EndTurn()
    {
        // Update game state
        if (gameState != GameState.EndTurn)
        {
            base.EndTurn();
            currEndTurnTimer = endTurnTimer;
        }

        // Check for tower moveement
        bool hasChanged = false;
        foreach (Rigidbody block in blocks)
        {
            if (block.velocity.sqrMagnitude > 0.025f)
            {
                hasChanged = true;
                break;
            }
        }

        // Update end turn timer based on tower movement
        if (hasChanged)
        {
            currEndTurnTimer = endTurnTimer;
        }
        else
        {
            currEndTurnTimer -= Time.deltaTime;
        }
        EndTurnTimerText = ((int)Mathf.Ceil(currEndTurnTimer)).ToString();

        // Update game environment when turn ends
        if (currEndTurnTimer <= 0f)
        {
            CalculateTowerHeight();
            UpdatePlayerTurn();
        }
    }

    public override void SelectBlock(NetworkIdentity objNetId)
    {
        // Find block game object
        GameObject block = objNetId.gameObject;

        // Remove block from spawner
        foreach (Spawner spawner in spawnerList)
        {
            if (spawner.spawnedObject == block)
            {
                spawner.spawnedObject = null;
                blocks.Add(block.GetComponent<Rigidbody>());
            }
        }
    }

    public void OnTowerFall()
    {
        Debug.Log("RTBGameController: Tower fell! Player " + (playerTurn % maxPlayers) + " loses!");

        // Move to final game state
        gameState = GameState.GameOver;
    }

    #endregion

    #region Private Methods

    protected override void PlayGame()
    {
        switch (gameState)
        {
            case GameState.Loading:
                WaitingForPlayers();
                break;
            case GameState.GameStart:
                UpdatePlayerTurn();
                break;
            case GameState.WaitTurn:
                break;
            case GameState.StartTurn:
                UpdateSpawners();
                break;
            case GameState.EndTurn:
                EndTurn();
                break;
            case GameState.GameOver:
                GameOver();
                break;
        }
    }

    protected override void GameOver()
    {
        // Display game over UI
        isGameOver = true;
    }

    protected override void WaitingForPlayers()
    {

        // Wait for max players before starting game
        if (netManager.numPlayers == maxPlayers)
        {

            // Save players on game start
            GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject obj in playerObjs)
            {
                PlayerPickupController player = obj.GetComponent<PlayerPickupController>();
                players.Add(player);
                player.gameController = this;
            }
            
            // Move to next game state
            gameState = GameState.GameStart;
        }
    }

    private void UpdateSpawners()
    {
        // Spawn new block at each empty spawner
        foreach (Spawner spawner in spawnerList)
        {
            if (!spawner.spawnedObject)
            {
                GameObject newBlock = spawner.spawnPoint.SpawnRandomBlock();
                spawner.spawnedObject = newBlock;
                NetworkServer.Spawn(newBlock);
            }
        }

        // Move to next game state
        gameState = GameState.WaitTurn;
    }

    private void UpdatePlayerTurn()
    {
        // End current player turn
        players[playerTurn].DisableInteraction();

        // Enable next players turn
        playerTurn = (playerTurn + 1) % netManager.numPlayers;
        players[playerTurn].EnableInteraction();

        // Move to next game state
        gameState = GameState.StartTurn;
    }

    private void CalculateTowerHeight()
    {
        float height = 0;

        // Find highest block in list of blocks
        foreach (Rigidbody block in blocks)
        {
            height = Mathf.Max(height, block.position.y);
        }

        // Save highest point in tower
        towerHeight = height;
    }

    #endregion

    #region Hooks

    private void OnTurnUpdate(int newTurn)
    {
        // Update player turn text
        ui.SetTurn((newTurn + 1).ToString());

        // Reset turn timer
        ui.ResetTimer();
        ui.StartTimer();

        playerTurn = newTurn;
    }

    private void OnEndTimerUpdate(string newText)
    {
        // Update end turn countdown
        ui.SetEndTurnText(newText);

        EndTurnTimerText = newText;
    }

    private void OnHeightUpdate(float newHeight)
    {
        // Update height value
        ui.SetHeight(((int) Mathf.Ceil(newHeight)).ToString());

        towerHeight = newHeight;
    }

    private void OnGameOver(bool newGameOver)
    {
        // Display game over screen
        ui.GameOver();

        isGameOver = newGameOver;
    }

    #endregion
}