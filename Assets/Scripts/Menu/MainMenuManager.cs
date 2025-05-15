using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenuManager : MonoBehaviour
{
    private enum MenuStates
    {
        MainMenu,
        Options,
        Credits,
        Controls,
    };

    [Header("MainMenu buttons")]
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private GameObject OptionsButton;
    [SerializeField] private GameObject CreditsButton;
    [SerializeField] private GameObject ControlsButton;
    [SerializeField] private string playScene;


    [Header("Options Menu")]
    [SerializeField] private GameObject OptionsObject;
    [SerializeField] private GameObject OptionsReturnButton;


    [Header("Credits Menu")]
    [SerializeField] private GameObject CreditsObject;
    [SerializeField] private GameObject CreditsReturnButton;


    [Header("Controls Menu")]
    [SerializeField] private GameObject ControlsObject;
    [SerializeField] private GameObject ControlsReturnButton;

    private List<GameObject> players;
    private GameObject currentButton;
    private MenuStates currentState;

    public void setPlayers(List<GameObject> p_players)//called by PlayerSpawnManager
    {
        players = p_players;
    }

    public void setMenuInput() //called by PlayerSpawnManager
    {

        foreach(GameObject p in players)
        {
            PlayerInput input = p.GetComponent<PlayerInput>();
            input.actions.FindActionMap("MainMenu").FindAction("Movement").started += onMove;
            input.actions.FindActionMap("MainMenu").FindAction("Select").started += onSelect;

            input.actions.FindActionMap("Attacks").Disable();
            input.actions.FindActionMap("Player").Disable();
            input.actions.FindActionMap("MainMenu").Enable();
        }

        openMenu();
    }

    public void disableMenuInput() //called by UI on Play
    {

        foreach (GameObject p in players)
        {
            PlayerInput input = p.GetComponent<PlayerInput>();
            input.actions.FindActionMap("MainMenu").FindAction("Movement").started -= onMove;
            input.actions.FindActionMap("MainMenu").FindAction("Select").started -= onSelect;

            input.actions.FindActionMap("Attacks").Enable();
            input.actions.FindActionMap("Player").Enable();
            input.actions.FindActionMap("MainMenu").Disable();

        }
    }

    public void openMenu()
    {
        currentButton = PlayButton;
        currentState = MenuStates.MainMenu;

        //Must do this before accessing buttons
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(currentButton);
    }


    private void changeSelected(int upDown)
    {
        if(upDown == 1)
        {
            Selectable selec = currentButton.GetComponent<Button>().FindSelectableOnDown();

            if(selec != null)
                currentButton = selec.gameObject;
        }
        else if(upDown == -1)
        {
            Selectable selec = currentButton.GetComponent<Button>().FindSelectableOnUp();

            if (selec != null)
                currentButton = selec.gameObject;
        }

        EventSystem.current.SetSelectedGameObject(currentButton);
    }


    public void onMove(InputAction.CallbackContext context)
    {

        Debug.Log("MOVE"+ this.gameObject);
        Vector2 move = context.ReadValue<Vector2>();

        if(move.y > 0)
        {
            changeSelected(-1);
        }
        else if(move.y < 0)
        {
            changeSelected(1);
        }
    }

    public void onSelect(InputAction.CallbackContext context)
    {
        currentButton.GetComponent<Button>().onClick.Invoke();
    }

    public void clickPlay()
    {
        FindFirstObjectByType<MainMenuManager>().disableMenuInput();
        SceneManager.LoadScene(playScene);
    }

    //options menu
    public void toggleOptionsMenu()
    {
        if (currentState == MenuStates.MainMenu) //open options
        {
            currentState = MenuStates.Options;
            OptionsObject.SetActive(true);
            currentButton = OptionsReturnButton;
        }
        else if(currentState == MenuStates.Options)
        {
            currentState = MenuStates.MainMenu;
            OptionsObject.SetActive(false);
            currentButton = PlayButton;
        }

        EventSystem.current.SetSelectedGameObject(currentButton);
    }

    //add options functionality here

    public void toggleCreditsMenu()
    {
        if (currentState == MenuStates.MainMenu) //open credits
        {
            currentState = MenuStates.Credits;
            CreditsObject.SetActive(true);
            currentButton = CreditsReturnButton;
        }
        else if (currentState == MenuStates.Credits)
        {
            currentState = MenuStates.MainMenu;
            CreditsObject.SetActive(false);
            currentButton = PlayButton;
        }

        EventSystem.current.SetSelectedGameObject(currentButton);
    }

    //add credits functionality here

    public void toggleControlsMenu()
    {
        if (currentState == MenuStates.MainMenu) //open credits
        {
            currentState = MenuStates.Controls;
            ControlsObject.SetActive(true);
            currentButton = ControlsReturnButton;
        }
        else if (currentState == MenuStates.Controls)
        {
            currentState = MenuStates.MainMenu;
            ControlsObject.SetActive(false);
            currentButton = PlayButton;
        }

        EventSystem.current.SetSelectedGameObject(currentButton);
    }

    //add controls functionality here

    public void Quit()
    {
        #if UNITY_STANDALONE
        Application.Quit();
        #endif

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
