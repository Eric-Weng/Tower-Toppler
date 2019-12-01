using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawn : MonoBehaviour
{
    public float spawnDelay = 1f;
    private Renderer rend = null;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetFloat("Vector1_F921EB12", Time.time + 1);
    }
}
