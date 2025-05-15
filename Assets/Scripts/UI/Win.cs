using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void setWinText(string player)
    {
        text.text = player + " HAS WON";
    }

}
