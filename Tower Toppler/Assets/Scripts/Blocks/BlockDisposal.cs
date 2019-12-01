using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDisposal : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == 8)
        {
            Destroy(other.gameObject);
        }
    }
}
