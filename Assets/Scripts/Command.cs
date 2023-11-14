using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    // All movement commands use this force
    protected float _Force = 10f;

    // Execute must be implemented by each child-class
    public abstract void Execute(Rigidbody rb, GameObject visuals);
    public abstract void Undo(Rigidbody rb, GameObject visuals);
}
