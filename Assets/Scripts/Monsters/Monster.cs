using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour, IDestroyable
{
    public abstract Monster Clone();

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
