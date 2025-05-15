using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    private int currMovePriority = 0;
    private Animator playerAnimator;
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    public void setMoving(bool pMoving)
    {
        playerAnimator.SetBool("IsRunning", pMoving);
    }

    public void PlayMove(Moves move, int movePriority)
    {
        if (currMovePriority > movePriority)
            return;

        currMovePriority = movePriority;
        ResetComboTriggers();
        playerAnimator.SetTrigger(move.ToString());
        currMovePriority = 0;
    }

    private void ResetComboTriggers()
    {
        foreach (AnimatorControllerParameter parameter in playerAnimator.parameters)
        {
            if(parameter.name != "IsRunning")
                playerAnimator.ResetTrigger(parameter.name);
        }
    }

    public void PlayBeingAttacked(bool bigHit)
    {
        currMovePriority = 0;
        ResetComboTriggers();

        if(bigHit)
            playerAnimator.SetTrigger("BeingAttacked");
        else
            playerAnimator.SetTrigger("BeingAttackedSmall");
    }

    public void PlayDeath()
    {
        currMovePriority = 0;
        ResetComboTriggers();
        playerAnimator.SetTrigger("Death");
    }

    public void ResetDeath()
    {
        currMovePriority = 0;
        ResetComboTriggers();
        Debug.Log("Resetting");
        playerAnimator.SetTrigger("Reset");
    }
}
