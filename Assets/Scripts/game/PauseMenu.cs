using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("PauseMenu")]
    [SerializeField] private GameObject PauseMenuObj;
    [SerializeField] private GameObject ResumeButton;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private string MainMenuName;

    private GameObject currentButton;
    private List<GameObject> players;
    private bool active = false;
    public void setPlayers(List<GameObject> p_players)//called by PlayerSpawnManager
    {
        players = p_players;
    }

    public void setPauseInput()
    {
        foreach (GameObject p in players)
        {
            PlayerInput input = p.GetComponent<PlayerInput>();
            input.actions.FindActionMap("Pause").FindAction("TogglePause").started += _ => togglePauseMenu();//little hack to avoid the useless inputActionContext
            input.actions.FindActionMap("Pause").Enable();
        }
    }

    public void disablePauseInput()
    {
        foreach (GameObject p in players)
        {
            PlayerInput input = p.GetComponent<PlayerInput>();
            input.actions.FindActionMap("Pause").FindAction("TogglePause").started -= _ => togglePauseMenu();//little hack to avoid the useless inputActionContext
            input.actions.FindActionMap("Pause").Disable();
        }
    }


    public void togglePauseMenu()
    {
        if (!PauseMenuObj) //this gets called with null?
            return;

        active = !active;
        PauseMenuObj.SetActive(active);
        setInputs(active);

        if(active)
        {
            currentButton = ResumeButton;
            EventSystem.current.SetSelectedGameObject(currentButton);
        }


        //FREEZE PLAYERS HERE
        if (active)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        FindFirstObjectByType<QuestionsManager>().Pause(active);
    }

    private void setInputs(bool setPauseMenu)
    {

        foreach(GameObject p in players)
        {
            PlayerInput input = p.GetComponent<PlayerInput>();

            if (setPauseMenu)
            {
                input.actions.FindActionMap("Attacks").Disable();
                input.actions.FindActionMap("Player").Disable();
                input.actions.FindActionMap("MainMenu").Enable();
                input.actions.FindActionMap("MainMenu").FindAction("Select").started += onSelect;
                input.actions.FindActionMap("MainMenu").FindAction("Movement").started += onMove;
            }
            else
            {
                input.actions.FindActionMap("Attacks").Enable();
                input.actions.FindActionMap("Player").Enable();
                input.actions.FindActionMap("MainMenu").Disable();

                input.actions.FindActionMap("MainMenu").FindAction("Select").started -= onSelect;
                input.actions.FindActionMap("MainMenu").FindAction("Movement").started -= onMove;
            }

        }
    }
    public void onMove(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();

        if (move.y > 0)
        {
            changeSelected(-1);
        }
        else if (move.y < 0)
        {
            changeSelected(1);
        }
    }

    public void onSelect(InputAction.CallbackContext context)
    {
        currentButton.GetComponent<Button>().onClick.Invoke();
    }

    private void changeSelected(int upDown)
    {
        if (upDown == 1)
        {
            Selectable selec = currentButton.GetComponent<Button>().FindSelectableOnDown();

            if (selec != null)
                currentButton = selec.gameObject;
        }
        else if (upDown == -1)
        {
            Selectable selec = currentButton.GetComponent<Button>().FindSelectableOnUp();

            if (selec != null)
                currentButton = selec.gameObject;
        }

        EventSystem.current.SetSelectedGameObject(currentButton);
    }


    public void OnResume()
    {
        togglePauseMenu();
    }

    public void OnQuit()
    {
        Time.timeScale = 1;
        disablePauseInput();
        setInputs(false);
        SceneManager.LoadScene(MainMenuName);
    }
}
