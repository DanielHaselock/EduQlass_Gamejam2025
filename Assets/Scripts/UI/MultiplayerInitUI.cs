using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplayerInitUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_PlayerOneUI;
    [SerializeField] private TextMeshProUGUI m_PlayerTwoUI;



    private void Start()
    {
        setPlayerOneText("Player one please connect");
        setPlayerTwoText("Player two please connect");
    }


    public void setPlayerOneText(string text)
    {
        m_PlayerOneUI.text = text;
    }

    public void setPlayerTwoText(string text)
    {
        m_PlayerTwoUI.text = text;
    }
    public void setPlayerOneColour(Color col)
    {
        m_PlayerOneUI.color = col;
    }

    public void setPlayerTwoColour(Color col)
    {
        m_PlayerTwoUI.color = col;
    }

}
