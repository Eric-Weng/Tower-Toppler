using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevGameController : MonoBehaviour
{

    #pragma warning disable 0649
    [SerializeField] private BlockSpawnController BlockSpawner;
    [SerializeField] private TowerFallHandler FallHandler;
    #pragma warning restore 0649

    void FallEvent()
    {
        Debug.Log("a block has fallen and it can't get up!");
    }

    void Start()
    {
        FallHandler.RegisterTowerFallHandler(FallEvent);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            BlockSpawner.SpawnRandomBlock();
        }
    }
}
