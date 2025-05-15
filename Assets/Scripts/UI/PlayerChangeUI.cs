using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeUI : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();

        health.hitService.OnSmallHitReceived.AddListener(changePlayersHealth);
        health.hitService.OnBigHitReceived.AddListener(changePlayersHealth);

        if (FindFirstObjectByType<StateMachine>() == null)
            return;

        FindFirstObjectByType<StateMachine>().StateEvents.OnRoundStart.AddListener(resetHealth);
    }

    public void resetHealth()
    {
        if (GameObject.FindGameObjectWithTag("PlayerOneUI") == null)
            return;

        if (gameObject.name.Equals("Player One"))
        {
            ChangeHealth health = GameObject.FindGameObjectWithTag("PlayerOneUI").GetComponent<ChangeHealth>();
            health.changeSlider(100);
        }
        else
        {
            ChangeHealth health = GameObject.FindGameObjectWithTag("PlayerTwoUI").GetComponent<ChangeHealth>();
            health.changeSlider(100);
        }
    }

    public void changePlayersHealth()
    {
        if (GameObject.FindGameObjectWithTag("PlayerOneUI") == null)
            return;

        if (gameObject.name.Equals("Player One"))
        {


            ChangeHealth health2 = GameObject.FindGameObjectWithTag("PlayerOneUI").GetComponent<ChangeHealth>();
            health2.changeSlider(health.getPercentage());
        }
        else
        {
            ChangeHealth health2 = GameObject.FindGameObjectWithTag("PlayerTwoUI").GetComponent<ChangeHealth>();
            health2.changeSlider(health.getPercentage());
        }
    }

}
