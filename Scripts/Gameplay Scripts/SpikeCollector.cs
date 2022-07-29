using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "SpikeScore")
        {
            Destroy(target.gameObject);
        }
    }
}
