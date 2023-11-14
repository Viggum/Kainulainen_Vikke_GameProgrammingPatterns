using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    // Collector is for coins and enemies
    private void OnTriggerEnter(Collider other)
    {
        IDestroyable destroyable = other.gameObject.GetComponent<IDestroyable>();
        if(destroyable != null )
            destroyable.Destroy();

    }
}
