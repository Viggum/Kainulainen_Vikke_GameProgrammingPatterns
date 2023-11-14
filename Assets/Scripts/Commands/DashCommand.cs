using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DashCommand : MonoBehaviour
{
    public bool onCooldown = false;
    public float c_t = 7;
    public float temp = 0;

    public TextMeshProUGUI cd_txt;


    // Update is called once per frame
    void Update()
    {
        if (onCooldown)
        {
            temp += Time.deltaTime;
            cd_txt.text = (c_t - temp).ToString("F2");
            if(temp > c_t)
            {
                onCooldown = false;
                temp = 0;
                cd_txt.gameObject.SetActive(false);
            }
        }          
    }

    public void Dash()
    {  
        if (!onCooldown)
        {
            cd_txt.gameObject.SetActive(true);
            onCooldown = true;
            // Dash
        }

    }
}
