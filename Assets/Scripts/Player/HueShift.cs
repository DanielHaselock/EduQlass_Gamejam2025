using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class HueShift : MonoBehaviour
{
    // Start is called before the first frame update

     private Texture2D origTex;
    [SerializeField] private Color playerOneColor;
    [SerializeField] private Color playerTwoColor;

    public void shiftHue(int player)
    {
        if(player == 0)
            GetComponent<SpriteRenderer>().color = playerOneColor;
        else
            GetComponent<SpriteRenderer>().color = playerTwoColor;
    }
}
