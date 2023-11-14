using UnityEngine;

public class MoveForwardCommand : Command
{
    public override void Execute(Rigidbody rb, GameObject visuals)
    {
        //rb.AddForce(_Force * rb.transform.forward, ForceMode.Acceleration);
        rb.transform.position += rb.transform.forward;
        visuals.transform.forward = rb.transform.forward;
    }

    public override void Undo(Rigidbody rb, GameObject visuals)
    {
        rb.transform.position -= rb.transform.forward;
        visuals.transform.forward = rb.transform.forward;
    }
}
