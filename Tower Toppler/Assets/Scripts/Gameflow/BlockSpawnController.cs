using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnController : MonoBehaviour
{

    #region Members

    #pragma warning disable 0649
    [SerializeField] private List<GameObject> m_Blocks;
    #pragma warning restore 0649

    #endregion

    #region Public Methods

    public GameObject SpawnRandomBlock()
    {
        return SpawnRandomAtPosition(transform);
    }

    public GameObject SpawnRandomAtPosition(Transform pos)
    {
        return Instantiate(m_Blocks[UnityEngine.Random.Range(0, m_Blocks.Count)], pos);
    }

    #endregion
}
