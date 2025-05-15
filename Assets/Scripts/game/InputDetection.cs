using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.DefaultInputActions;


public enum PlayerCount
{
    Single_DEBUG_ONLY = 1,
    Double = 2,
}

[RequireComponent(typeof(MultiplayerInitUI))]
public class InputDetection : MonoBehaviour
{
    public List<InputDevice> inputDevices;
    int joinedCount = 0;

    [SerializeField] PlayerCount maxPlayers;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private MultiplayerInitUI UI;

    private PlayerSpawnManager playerSpawnManager;
    List<GameObject> players = new List<GameObject>();
    InputAction joinAction;
    void Awake()
    {
        inputDevices = new List<InputDevice>();

        // Bind joinAction to any button press.
        joinAction = new InputAction(binding: "/*/<button>");
        joinAction.started += OnJoinPressed;
        BeginJoining();
    }

    private void Start()
    {
        UI = GetComponent<MultiplayerInitUI>();
        playerSpawnManager = FindFirstObjectByType<PlayerSpawnManager>();

       
        UI.setPlayerOneColour(Color.white);
        UI.setPlayerTwoColour(Color.white);
    }

    void OnJoinPressed(InputAction.CallbackContext context)
    {
        JoinPlayer(context.control.device);
    }

    void JoinPlayer(InputDevice device)
    {
        if (inputDevices.Contains(device) || device.name == "Mouse")
            return;

        inputDevices.Add(device);


        string CurrcontrolScheme = "Controller";

        
        PlayerInput player = PlayerInput.Instantiate(playerPrefab, controlScheme: CurrcontrolScheme, pairWithDevice: device);
        GameObject go = player.gameObject;

        player.actions.devices = new[] { device };

        if (joinedCount == 0)
        {
            player.gameObject.name = "Player One";
        }
        else
        {
            player.gameObject.name = "Player Two";
        }

        DontDestroyOnLoad(go); //ensure Players never get destroyed
        go.GetComponent<PlayerInputs>().SaveControls(player.actions, device, player.playerIndex);
        players.Add(go);

        joinedCount++;

        //Update UI
        if (joinedCount == 1)
        {
            UI.setPlayerOneText("Player one connected");
            UI.setPlayerOneColour(Color.green);
        }
            
        else if(joinedCount == 2)
        {
            UI.setPlayerTwoText("Player two connected");
            UI.setPlayerTwoColour(Color.green);
        }
            

        if(maxPlayers == PlayerCount.Single_DEBUG_ONLY) //spawn dummy player 2 in case of 1 player DEBUG TESTING ONLY
        {
            GameObject go2 = Instantiate(playerPrefab, new Vector3(100, 200, 0), Quaternion.identity); //spawn far offscreen
            DontDestroyOnLoad(go2);
            players.Add(go2);
        }

        if (joinedCount == (int)maxPlayers)
            EndJoining();
    }

    void BeginJoining()
    {
        joinAction.Enable();
    }

    void EndJoining()
    {
        joinAction.Disable();
        playerSpawnManager.setPlayers(players); //this keeps the data so that the player are spawned in the next scene
        playerSpawnManager.loadMainMenu();
    }

    void OnDestroy()
    {
        joinAction.started -= OnJoinPressed;
    }
}