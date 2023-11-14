using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDestroyable
{
    public static event Action OnEnemyKilled;


    private void OnDisable()
    {
        OnEnemyKilled?.Invoke();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    
}
