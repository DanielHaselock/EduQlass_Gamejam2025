using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StateEventController
{
    public Action BaseEvent = null;
    public void AddListener(Action listener) => BaseEvent += listener;
    public void RemoveListener(Action listener) => BaseEvent -= listener;
    public void RemoveAllListeners() => BaseEvent = null;
    public void InvokeEvent() => BaseEvent?.Invoke();
}

public class StateEventService
{
    public StateEventService()
    {
        OnGameStart = new StateEventController();
        OnRoundStart = new StateEventController();
        OnRoundPlay = new StateEventController();
        OnRoundEnd = new StateEventController();
        OnGameEnd = new StateEventController();
    }

    public StateEventController OnGameStart { get; private set; } //Only fires once at the start of the scene
    public StateEventController OnRoundStart { get; private set; } //Fires at every beginning on round
    public StateEventController OnRoundPlay { get; private set; } //Fires at every Play of round
    public StateEventController OnRoundEnd { get; private set; } //Fires at every End of round
    public StateEventController OnGameEnd { get; private set; } //Fires once at the very end of the game
}

public enum States
{
    None,
    Game_Start,
    Round_Init,
    Round_Play,
    Round_End,
    Game_End
}
public class StateMachine : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private GameObject player1Spawn;
    [SerializeField] private GameObject player2Spawn;
    [SerializeField] private int maxNumberWins = 3;
    [SerializeField] private List<Sprite> backgrounds;
    [SerializeField] private SpriteRenderer currBackground;
    private int backgroundInt = 0;

    [Header("Game Start options -- fires at beginning of scene")]
    [SerializeField] private float GameStartTime = 2.0f; //Time before the round starts
    //Put UI/Sounds here

    [Header("Round Initialisation options -- fires before round starts")]
    [SerializeField] private float RoundStartTime = 2.0f;

    [Header("Round Play options -- fires at every round start")]
    [SerializeField] private float RoundPlayTime = 60.0f;

    [Header("Round End options -- fires at every round end")]
    [SerializeField] private float RoundEndTime = 5.0f;

    [Header("Game end options -- fires at the end of the game")]
    [SerializeField] private float GameEndTime = 2.0f;

    private States currentState = States.None;

    private List<GameObject> players = null;

    public States getCurrentState() { return currentState; }

    [HideInInspector] public StateEventService StateEvents;

    [SerializeField] private GameObject WinScreen;



    private void Awake()
    {
        StateEvents = new StateEventService();
    }
    public void setPlayers(List<GameObject> p)
    {
        players = p;
    }
    public void BeginGame() //called from playerSpawnManager
    {
        backgroundInt = 0;
        currentState = States.Game_Start;
        StartCoroutine(BeginGame_Coroutine());

        //we assume both players have loaded
        players[0].SetActive(true);
        players[0].transform.position = player1Spawn.transform.position;

        players[1].SetActive(true);
        players[1].transform.position = player2Spawn.transform.position;
        players[1].transform.Rotate(new Vector3(0, 180, 0));

        foreach (GameObject player in players)
        {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player.GetComponent<PlayerStats>().resetRoundWins(); //reset stats here just to be sure
            player.GetComponent<PlayerAnimations>().ResetDeath();
        }
    }

    private void InitRound()
    {
        currBackground.sprite = backgrounds[backgroundInt];
        backgroundInt++;

        currentState = States.Round_Init;
        
        //reset player positions
        players[0].transform.position = player1Spawn.transform.position;
        players[1].transform.position = player2Spawn.transform.position;

        foreach (var player in players)
        {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player.GetComponent<Health>().resetHealth();
            player.GetComponent<PlayerAnimations>().ResetDeath();

        }

        StartCoroutine(InitRound_Coroutine());
    }

    private void PlayRound()
    {
        currentState = States.Round_Play;

        foreach (var player in players)
        {
            player.GetComponent<Rigidbody2D>().isKinematic = false;
            player.GetComponent<Main_Input_Player>().EnableInputs();
        }

        StartCoroutine(RoundPlay_Coroutine());

        FindFirstObjectByType<QuestionsManager>().startQuestions();
    }

    public void EndRoundEarly(GameObject lostPlayer) //Player knocks out other player
    {
        StopAllCoroutines();

        foreach (var player in players)
        {
            if(player != lostPlayer)
            {
                player.GetComponent<PlayerStats>().addRoundWin();
            }

        }

        EndRound();
    }

    private void EndRound() //TODO logic if players don't knock each other out
    {
        currentState = States.Round_End;

        FindFirstObjectByType<QuestionsManager>().stopQuestions();

        foreach (var player in players)
        {
            //player.GetComponent<Rigidbody2D>().isKinematic = true;
            player.GetComponent<Main_Input_Player>().DisableInputs();
        }

        GameObject Winplayer = checkIfWin();

        if (Winplayer != null) //TODO winscreen
        {
            WinScreen.SetActive(true);
            GameObject.FindGameObjectWithTag("WinText").GetComponent<TextMeshProUGUI>().text = Winplayer.name + " WON";

            EndGame();
        }
        else
        {
            StartCoroutine(RoundEnd_Coroutine());
        }
    }
    
    private GameObject checkIfWin()
    {
        foreach (var player in players)
        {
            if(player.GetComponent<PlayerStats>().getRoundWins() == maxNumberWins)
            {
                return player;
            }
        }

        return null;
    }

    private void EndGame()
    {
        backgroundInt = 0;
        currentState = States.Game_End;

        foreach (var player in players)
        {
            player.GetComponent<PlayerStats>().resetRoundWins();
        }

        StartCoroutine(EndGame_Coroutine());
    }


    //Coroutines ----------------------------------------------

    private IEnumerator BeginGame_Coroutine()
    {
        //Debug.Log("Beginning Game");
        //Show some UI
        yield return new WaitForSeconds(GameStartTime);
        StateEvents.OnGameStart.InvokeEvent(); //has to be here because starts have not triggered yet
        //Hide UI
        InitRound();
    }

    private IEnumerator InitRound_Coroutine()
    {
        //Debug.Log("Init Round");
        //Show some UI
        StateEvents.OnRoundStart.InvokeEvent();
        yield return new WaitForSeconds(RoundStartTime);
        //Hide UI
        PlayRound();
    }

    private IEnumerator RoundPlay_Coroutine()
    {
        //Debug.Log("Playing Round");
        //Show some UI
        StateEvents.OnRoundPlay.InvokeEvent();
        yield return new WaitForSeconds(RoundPlayTime);
        //Hide UI
        //EndRound();
    }

    private IEnumerator RoundEnd_Coroutine()
    {
        //Debug.Log("Ending Round");
        //Show some UI
        StateEvents.OnRoundEnd.InvokeEvent();
        yield return new WaitForSeconds(RoundEndTime);
        //Hide UI
        InitRound();
    }
    private IEnumerator EndGame_Coroutine()
    {
        //Show some UI
        //Debug.Log("Ending Game");
        StateEvents.OnGameEnd.InvokeEvent();
        yield return new WaitForSeconds(GameEndTime);
        //Hide UI
        //Change to another scene
        FindFirstObjectByType<PauseMenu>().OnQuit(); //debug
    }

}