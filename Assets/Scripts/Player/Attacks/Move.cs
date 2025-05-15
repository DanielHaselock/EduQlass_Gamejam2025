using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Moves //All the Available Moves
{
    None,
    Punch,
    UpPunch,
    UpperCut,
    DownKick,
    UpKick,
    RoundKick,
    FireBreath,
    Knife,
    Hook
};


[CreateAssetMenu(fileName = "New Move", menuName = "New Move")]
public class Move : ScriptableObject
{
    [SerializeField] List<string> movesKeyCodes; //the List and order of the Moves
    [SerializeField] public Moves moveType; //The kind of the move
    [SerializeField] int ComboPriorty = 0; //the more complicated the move the higher the Priorty
    [SerializeField] public bool SmallAttack = false; //makes player vunerable to being hit again
    [SerializeField] public Vector2 knockbackForce = Vector2.zero;
    [SerializeField] public float minDamage;
    [SerializeField] public float maxDamage;

    public bool isMoveAvailable(List<string> playerKeyCodes) //Check if we can perform this move from the entered keys
    {
        int comboIndex = 0;

        if (playerKeyCodes.Count != movesKeyCodes.Count)
        {
            return false;
        }

        for (int i = 0; i < playerKeyCodes.Count; i++)
        {
            if (playerKeyCodes[i].Equals(movesKeyCodes[comboIndex]))
            {
                comboIndex++;
                if (comboIndex == movesKeyCodes.Count) //The end of the Combo List
                    return true;
            }
            else
                comboIndex = 0;
        }
        return false;
    }

    //Getters
    public int GetMoveComboCount()
    {
        return movesKeyCodes.Count;
    }
    public int GetMoveComboPriorty()
    {
        return ComboPriorty;
    }
    public Moves GetMove()
    {
        return moveType;
    }
}