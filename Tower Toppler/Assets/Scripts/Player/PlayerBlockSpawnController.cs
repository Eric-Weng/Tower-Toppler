using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
public class PlayerBlockSpawnController : NetworkBehaviour
{

    public GameObject blockPrefab;

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            CmdSpawnBlock();
        }
    }

    [Command]
    void CmdSpawnBlock()
    {
        GameObject block = Instantiate(blockPrefab, new Vector3(0, 0, 0), new Quaternion());
        NetworkServer.SpawnWithClientAuthority(block, connectionToClient);
    }
}
