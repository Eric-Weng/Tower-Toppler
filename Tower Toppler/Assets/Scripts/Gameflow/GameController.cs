using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
public abstract class GameController : NetworkBehaviour
{

    #region Declarations

    protected enum GameState
    {
        Loading,
        GameStart,
        StartTurn,
        WaitTurn,
        EndTurn,
        GameOver
    };

    public class Spawner
    {
        public BlockSpawnController spawnPoint;
        public GameObject spawnedObject;

        public Spawner(BlockSpawnController _spawnPoint, GameObject _spawnedObject)
        {
            spawnPoint = _spawnPoint;
            spawnedObject = _spawnedObject;
        }
    }

    #endregion

    #region Members

    public NetworkManager netManager;

    [SyncVar]
    protected GameState gameState = GameState.Loading;

    #endregion

    #region Public Methods

    public abstract void SelectBlock(NetworkIdentity objNetId);
    public virtual void EndTurn()
    {
        gameState = GameState.EndTurn;
    }

    #endregion

    #region Private Methods

    protected abstract void PlayGame();
    protected abstract void GameOver();
    protected abstract void WaitingForPlayers();

    #endregion
}
