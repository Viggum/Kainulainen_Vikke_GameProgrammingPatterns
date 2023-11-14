using UnityEngine;

public class MoveLeftCommand : Command
{
    public override void Execute(Rigidbody rb, GameObject visuals)
    {
        //rb.AddForce(_Force * -rb.transform.right, ForceMode.Acceleration);
        rb.transform.position -= rb.transform.right;
        visuals.transform.forward = -rb.transform.right;
    }

    public override void Undo(Rigidbody rb, GameObject visuals)
    {
        rb.transform.position += rb.transform.right;
        visuals.transform.forward = -rb.transform.right;
    }
}
