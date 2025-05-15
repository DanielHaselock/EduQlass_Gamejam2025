using System.Collections;
using UnityEngine;

public class TestEvent : MonoBehaviour
{
    StateMachine stateMachine;

    private void Start()
    {
        stateMachine = FindFirstObjectByType<StateMachine>();
        stateMachine.StateEvents.OnGameStart.AddListener(testGameStart);
        stateMachine.StateEvents.OnRoundStart.AddListener(testRoundStart);
        stateMachine.StateEvents.OnRoundStart.AddListener(testRoundStart2);
        stateMachine.StateEvents.OnRoundPlay.AddListener(testRoundPlay);
        stateMachine.StateEvents.OnRoundEnd.AddListener(testRoundEnd);
        stateMachine.StateEvents.OnGameEnd.AddListener(testGameEnd);
    }

    public void testGameStart()
    {
        Debug.Log("Hello world");
    }

    public void testRoundStart()
    {
        Debug.Log("Round starting");
    }

    public void testRoundStart2()
    {
        Debug.Log("Round starting as well");
    }

    public void testRoundPlay()
    {
        Debug.Log("Round Playing");
    }

    public void testRoundEnd()
    {
        Debug.Log("Round Ending");
    }
    public void testGameEnd()
    {
        Debug.Log("Game Ending");
    }

    private void OnDestroy()
    {
        stateMachine.StateEvents.OnGameStart.RemoveListener(testGameStart);
        stateMachine.StateEvents.OnRoundStart.RemoveListener(testRoundStart);
        stateMachine.StateEvents.OnRoundStart.RemoveListener(testRoundStart2);
        stateMachine.StateEvents.OnRoundPlay.RemoveListener(testRoundPlay);
        stateMachine.StateEvents.OnRoundEnd.RemoveListener(testRoundEnd);
        stateMachine.StateEvents.OnGameEnd.RemoveListener(testGameEnd);
    }

}
