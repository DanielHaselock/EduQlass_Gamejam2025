using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int roundWins;

    public void addRoundWin()
    {
        roundWins++;
        Debug.Log("Add round win: " + roundWins);
    }

    public void resetRoundWins()
    {
        roundWins = 0;
    }

    public int getRoundWins()
    {
        return roundWins;
    }
}
