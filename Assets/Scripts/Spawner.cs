using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Monster my_Monster;

    public float X = 1f;
    public float Y = 1f;
    public float Z = 1f;

    public float temp = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temp += Time.deltaTime;
        if(temp >= 3 && my_Monster != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.5f * X, 0.5f * X), Random.Range(-0.5f * Y, 0.5f * Y), Random.Range(-0.5f * Z, 0.5f * Z));
            Instantiate(my_Monster, spawnPos, Quaternion.identity);
            temp = 0;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(X, Y, Z));
    }
}
