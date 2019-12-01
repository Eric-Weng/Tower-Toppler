using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnController : MonoBehaviour
{

    #region Members
    
    public List<GameObject> m_Blocks;

    private bool isEmpty = true;

    #endregion

    #region Unity Methods

    void FixedUpdate()
    {
        isEmpty = true;
    }

    void OnTriggerStay(Collider other)
    {
        isEmpty = false;
    }

    void Update()
    {
        if (isEmpty)
        {
            isEmpty = false;
            SpawnRandomBlock();
        }
    }

    #endregion

    #region Public Methods

    public GameObject SpawnRandomBlock()
    {
        if (m_Blocks.Count == 0)
        {
            return null;
        }

        GameObject newBlock = Instantiate(m_Blocks[UnityEngine.Random.Range(0, m_Blocks.Count)], transform);
        newBlock.transform.SetParent(null);

        return newBlock;
    }

    #endregion
}