using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;


public class PlayerInputs : MonoBehaviour
{
    //public InputUser user;
    public InputActionAsset gameplayActionsNEW;
    public int playerIndex = 0;
    private InputDevice InputDevice;

    public void SaveControls(InputActionAsset iam, InputDevice dev, int index)
    {
        InputDevice = dev;
        playerIndex = index;
        gameplayActionsNEW = iam;
    }

    public void BindControls()
    {
        GetComponent<Main_Input_Player>().setInputAction(gameplayActionsNEW);
    }
}