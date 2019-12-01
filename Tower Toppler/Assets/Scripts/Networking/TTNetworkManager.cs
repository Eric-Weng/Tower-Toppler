using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0618
public class TTNetworkManager : NetworkManager
{
    private GameController controller = null;

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controller.netManager = this;
    }
}
