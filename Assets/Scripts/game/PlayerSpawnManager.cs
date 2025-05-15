using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PlayerSpawnManager : MonoBehaviour
{
    private List<GameObject> players;

    // Store the scene that should trigger start
    [SerializeField] private string gameSceneName;
    [SerializeField] private string mainMenuSceneName;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void setPlayers(List<GameObject> inputs)
    {
        players = inputs;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // return if not the start calling scene
        if (string.Equals(scene.name, gameSceneName))
        {
            StateMachine stateMachine = FindFirstObjectByType<StateMachine>();
            stateMachine.setPlayers(players);
            stateMachine.BeginGame();

            PauseMenu pause = FindFirstObjectByType<PauseMenu>();
            pause.setPlayers(players);
            pause.setPauseInput();

            QuestionsManager questions = FindFirstObjectByType<QuestionsManager>();
            questions.setPlayers(players);

            players[1].GetComponent<HueShift>().shiftHue(1);

        }
        else if(string.Equals(scene.name, mainMenuSceneName))
        {
            foreach (var player in players)
            {
                player.GetComponent<PlayerInputs>().BindControls();
                player.GetComponent<Rigidbody2D>().isKinematic = true;
                player.GetComponent<Main_Input_Player>().DisableInputs();
                player.transform.position = new Vector3(100, 100, 0); //away from screen
                player.transform.rotation = Quaternion.identity;
            }

            MainMenuManager menuManager = FindAnyObjectByType<MainMenuManager>(); //expensive but only called once
            menuManager.setPlayers(players);
            menuManager.setMenuInput();
        }

    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
