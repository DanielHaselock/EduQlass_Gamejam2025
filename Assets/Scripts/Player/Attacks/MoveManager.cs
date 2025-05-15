using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimations))]
public class MoveManager : MonoBehaviour
{
    [SerializeField] List<Move> availableMoves; //All the Available Moves
    PlayerAnimations playerAnimations;
    NextCollisionData nextCollisionData;

    void Awake()
    {
        availableMoves.Sort(Compare); //Sort all the moves based on thier prioraty
    }

    private void Start()
    {
        playerAnimations = GetComponent<PlayerAnimations>();
        nextCollisionData = GetComponent<NextCollisionData>();
    }

    public bool CanMove(List<string> attacks) //return true if the list contain a move
    {
        foreach (Move move in availableMoves)
        {
            if (move.isMoveAvailable(attacks))
                return true;
        }
        return false;
    }

    public void PlayMove(List<string> attacks) //Send the moves to the player starting from the highest priorty
    {
        foreach (Move move in availableMoves)
        {
            if (move.isMoveAvailable(attacks)) //checks if move exists
            {
                nextCollisionData.isOpportunistic = move.SmallAttack;
                nextCollisionData.knockbackForce = move.knockbackForce;
                nextCollisionData.minDamage = move.minDamage;
                nextCollisionData.maxDamage = move.maxDamage;
                playerAnimations.PlayMove(move.GetMove(), move.GetMoveComboPriorty());
                break;
            }
        }
    }

    public int Compare(Move move1, Move move2)
    {
        return Comparer<int>.Default.Compare(move2.GetMoveComboPriorty(), move1.GetMoveComboPriorty());
    }

}