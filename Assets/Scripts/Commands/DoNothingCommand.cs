using UnityEngine;

public class DoNothingCommand : Command
{
    public override void Execute(Rigidbody rb, GameObject Visuals)
    {
    }

    public override void Undo(Rigidbody rb, GameObject Visuals)
    {
    }

}
