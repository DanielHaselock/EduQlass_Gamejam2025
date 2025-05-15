using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combo : MonoBehaviour
{
    [SerializeField] private float comboResetMaxTime = 3.0f;

    [SerializeField] private float comboResetNoInput = 0.5f;

    [SerializeField] private float attackCooldownTime = 0.5f;

    private InputActionAsset input = null;
    private bool inputsChanged = false;
    private bool canAttack = true;

    private List<string> attacksDone;

    //TODO Combo freeze on attack? or duel screen
    void Start()
    {
        attacksDone = new List<string>();
    }

    public void setInputActions(InputActionAsset _input)
    {
        input = _input;
        setAttacks();
        input.FindActionMap("Attacks").Disable();
    }

    public void enableInputs()
    {
        input.FindActionMap("Attacks").Enable();
    }
    public void disableInputs()
    {
        resetAttacks();
        input.FindActionMap("Attacks").Disable();
    }

    private void setAttacks()
    {
        if (input == null)
            return;

        input.FindActionMap("Attacks").FindAction("ShortAttack").started += generalCallback;
        input.FindActionMap("Attacks").FindAction("ShortAttack").started += ShortAttackStarted;
        input.FindActionMap("Attacks").FindAction("ShortAttack").canceled += ShortAttackCancelled;

        input.FindActionMap("Attacks").FindAction("LongAttack").started += generalCallback;
        input.FindActionMap("Attacks").FindAction("LongAttack").started += LongAttackStarted;
        input.FindActionMap("Attacks").FindAction("LongAttack").canceled += LongAttackCancelled;
        input.FindActionMap("Attacks").Enable();
    }

    private void generalCallback(InputAction.CallbackContext context)
    {
        if (!canAttack)
            return;

        if(attacksDone.Count == 0)
        {
            StartCoroutine(CheckAndAttack());
        }

        if(attacksDone.Count > 0)
        {
            inputsChanged = true;
        }
    }

    private void ShortAttackStarted(InputAction.CallbackContext context)
    {
        attacksDone.Add("ShortAttack");
    }

    private void ShortAttackCancelled(InputAction.CallbackContext context)
    {
    }

    private void LongAttackStarted(InputAction.CallbackContext context)
    {
        attacksDone.Add("LongAttack");
    }

    private void LongAttackCancelled(InputAction.CallbackContext context)
    {
    }

    private IEnumerator CheckAndAttack()
    {
        yield return new WaitForSeconds(comboResetNoInput);

        if(!inputsChanged)
        {
            StopAllCoroutines();
            StartCoroutine(attackCooldown());
            GetComponent<MoveManager>().PlayMove(attacksDone);
        }
        else
        {
            inputsChanged = false;
            StartCoroutine(CheckAndAttack());
        }
    }

    private IEnumerator attackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownTime);
        canAttack = true;
        resetAttacks();
    }

    private void resetAttacks()
    {
        attacksDone.Clear();
    }

    private void OnDestroy()
    {
        removeAttacks();
    }

    private void removeAttacks()
    {
        if (input == null)
            return;
        input.FindActionMap("Attacks").FindAction("ShortAttack").started -= generalCallback;
        input.FindActionMap("Attacks").FindAction("ShortAttack").started -= ShortAttackStarted;
        input.FindActionMap("Attacks").FindAction("ShortAttack").canceled -= ShortAttackCancelled;

        input.FindActionMap("Attacks").FindAction("LongAttack").started -= generalCallback;
        input.FindActionMap("Attacks").FindAction("LongAttack").started -= LongAttackStarted;
        input.FindActionMap("Attacks").FindAction("LongAttack").canceled -= LongAttackCancelled;
    }
}
