using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
public class MultiplayerGameManager : NetworkBehaviour
{

    public GameObject BlockPrefab;

    void Start()
    {
        if (!isServer)
        {
            return;
        }

        GameObject instantiatedBlock = Instantiate(BlockPrefab);

        NetworkServer.Spawn(instantiatedBlock);
    }
}
