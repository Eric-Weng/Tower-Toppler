using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
public class EscapeGameController : GameController
{
    #region Members
    
    public List<BlockSpawnController> spawnPoints;
    public int maxPlayers;

    private List<PlayerPickupController> players = null;
    private List<Spawner> spawnerList = null;
    private List<Rigidbody> blocks = null;

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

    #endregion

    #region Private Methods

    protected override void PlayGame()
    {
        switch (gameState)
        {
            case GameState.Loading:
                WaitingForPlayers();
                break;
            default:
                break;
        }
    }

    protected override void GameOver()
    {
        gameState = GameState.GameOver;
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
                player.EnableInteraction();
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
    }

    #endregion
}