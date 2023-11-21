using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUpSystem : MonoBehaviour
{
    public enum PowerUpState
    {
        POWERUP,
        POWERDOWN
    }

    public float powerUpTime = 10f;
    public float powerUpTemp = 0f;

    public PowerUpState myPowerUpState;

    // Start is called before the first frame update
    void Start()
    {
        myPowerUpState = PowerUpState.POWERDOWN;
    }

    // Update is called once per frame
    void Update()
    {
        switch (myPowerUpState)
        {
            case PowerUpState.POWERUP:
                powerUpTemp += Time.deltaTime;
                if(powerUpTemp >= powerUpTime)
                {
                    transform.localScale = Vector3.one;
                    powerUpTemp = 0f;
                    myPowerUpState = PowerUpState.POWERDOWN;
                }
                break;

            case PowerUpState.POWERDOWN:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Simple way to do this...
        if(other.gameObject.tag == "Powerup")
        {
            Destroy(other.gameObject);
            powerUpTemp = 0f;
            transform.localScale = Vector3.one * 3;
            myPowerUpState = PowerUpState.POWERUP;

        }
    }
}
