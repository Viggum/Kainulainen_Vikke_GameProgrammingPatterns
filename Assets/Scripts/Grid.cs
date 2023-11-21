using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int Width = 10;
    public int Depth = 10;
    public Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        startPos = new Vector3(transform.position.x + -Width * 0.5f, transform.position.y, transform.position.z + Depth * 0.5f);
    }


    private void OnDrawGizmos()
    {
        for (int i = 0; i < Width; i+=2)
        {        
            for (int j = 0; j < Depth; j+=2)
            {
                Gizmos.DrawSphere(new Vector3(startPos.x + i, startPos.y, startPos.z - j), 0.1f);
            }
        }
    }
}
