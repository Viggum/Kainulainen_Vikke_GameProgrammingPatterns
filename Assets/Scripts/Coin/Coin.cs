using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour, IDestroyable
{
    public static event Action OnCoinCollected;

    public int coinValue;

    private void OnDisable()
    {
        OnCoinCollected?.Invoke();
    }

    public void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
