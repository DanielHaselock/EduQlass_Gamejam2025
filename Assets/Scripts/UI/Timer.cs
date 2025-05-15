using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TextMeshProUGUI;

    bool started = false;
    float time = 60;

    private void Start()
    {
        StateMachine stateMachine = FindAnyObjectByType<StateMachine>();
        stateMachine.StateEvents.OnRoundPlay.AddListener(TimerStart);
        stateMachine.StateEvents.OnRoundEnd.AddListener(TimerStop);
    }


    private void TimerStart()
    {
        time = 60;
        started = true;
    }

    private void TimerStop()
    {
        started = false;
    }

    private void Update()
    {
        if(started)
            time -= Time.deltaTime;

        setTime(time);
    }

    public void setTime(float time)
    {
        TextMeshProUGUI.SetText(time.ToString());
    }

}
