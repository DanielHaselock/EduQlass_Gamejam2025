using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody2D))]
public class Main_Input_Player : MonoBehaviour
{
    private InputActionAsset inputActions = null;
    private Vector2 movementDirection = Vector3.zero;

    private Rigidbody2D rb = null;
    private On_ground ground = null;

    [SerializeField] private float moveSpeed = 200.0f;
    [SerializeField] private float jumpForce = 2.0f;

    private PlayerAnimations playerAnimations;

    private Combo combo;

    private bool freeze = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<On_ground>();
        playerAnimations = GetComponent<PlayerAnimations>();

        ground.addCollisionEnterEvent(OnGround);
        ground.addCollisionExitEvent(leaveGround);
    }


    public void setInputAction(InputActionAsset input)
    {
        this.inputActions = input;
        //bind movement
        inputActions.FindActionMap("Player").FindAction("Movement").started += Movement_Start;
        inputActions.FindActionMap("Player").FindAction("Movement").canceled += Movement_Cancelled;
        //bind jump
        inputActions.FindActionMap("Player").FindAction("Jump").started += Jump_Start;

        combo = GetComponent<Combo>();
        combo.setInputActions(input);
    }

    public void EnableInputs()
    {
        if (inputActions == null)
            return;

        inputActions.FindActionMap("Player").FindAction("Movement").Enable();
        inputActions.FindActionMap("Player").FindAction("Jump").Enable();
        inputActions.FindActionMap("Pause").Enable();

        inputActions.FindActionMap("Player").FindAction("Movement").started += Movement_Start;
        inputActions.FindActionMap("Player").FindAction("Movement").canceled += Movement_Cancelled;

        combo.enableInputs();
    }

    public void DisableInputs()
    {
        if (inputActions == null)
            return;

        inputActions.FindActionMap("Player").FindAction("Movement").Disable();
        inputActions.FindActionMap("Player").FindAction("Jump").Disable();
        inputActions.FindActionMap("Pause").Disable();

        inputActions.FindActionMap("Player").FindAction("Movement").started -= Movement_Start;
        inputActions.FindActionMap("Player").FindAction("Movement").canceled -= Movement_Cancelled;

        combo.disableInputs();
    }

    private void FixedUpdate()
    {
        float xVelocity = movementDirection.x * moveSpeed * Time.fixedDeltaTime;

        if(!freeze)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void Movement_Start(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        movementDirection = input.normalized;
        playerAnimations.setMoving(true);
    }

    public void Movement_Cancelled(InputAction.CallbackContext context)
    {
        movementDirection = Vector2.zero;
        playerAnimations.setMoving(false);
    }

    public void Jump_Start(InputAction.CallbackContext context)
    {
        if (ground.onGround)
        {
            rb.AddForce(new Vector2(0.0f, 1.0f) * this.jumpForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Not on ground");
        }
    }

    private void OnGround()
    {
        //Debug.Log("HIT GROUND");
    }

    private void leaveGround()
    {
        //Debug.Log("Leaving GROUND");
    }


    void OnDisable()
    {
        if (inputActions != null)
        {
            //inputActions.Player.Movement.Disable();
            //inputActions.Player.Jump.Disable();
        }
    }

    private void OnDestroy() //clear subscribed actions
    {
        if (inputActions != null)
        {
            //inputActions.Player.Movement.started -= Movement_Start;
            //inputActions.Player.Movement.canceled -= Movement_Cancelled;
            //inputActions.Player.Jump.started -= Jump_Start;
        }
    }


    public IEnumerator setFreezed(float time)
    {
        freeze = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
        DisableInputs();
        yield return new WaitForSeconds(time);
        EnableInputs();
        freeze = false;
    }

}
