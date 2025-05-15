using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangePower : MonoBehaviour
{

    public void changeplayerPower(float percentage)
    {
        if (GameObject.FindGameObjectWithTag("PlayerOnePower") == null)
            return;

        if (gameObject.name.Equals("Player One"))
        {

            GameObject.FindGameObjectWithTag("PlayerOnePower").GetComponent<ChangePower>().changeSlider(percentage);
        }
        else
        {
            GameObject.FindGameObjectWithTag("PlayerTwoPower").GetComponent<ChangePower>().changeSlider(percentage);
        }
    }
}
