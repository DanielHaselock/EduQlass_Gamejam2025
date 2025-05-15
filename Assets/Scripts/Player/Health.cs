using System;
using System.Collections;
using UnityEngine;

public class HitEventController
{
    public Action BaseEvent = null;
    public void AddListener(Action listener) => BaseEvent += listener;
    public void RemoveListener(Action listener) => BaseEvent -= listener;
    public void RemoveAllListeners() => BaseEvent = null;
    public void InvokeEvent() => BaseEvent?.Invoke();
}

public class HitEventService
{
    public HitEventService()
    {
        OnSmallHitReceived = new HitEventController();
        OnBigHitReceived = new HitEventController();
        OnDeath = new HitEventController();
    }

    public HitEventController OnSmallHitReceived;
    public HitEventController OnBigHitReceived;
    public HitEventController OnDeath;
}

public class Health : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] private float maxHealth;

    [Header("Big hit settings")]
    [SerializeField] private float bigHitCooldownMovement;
    [SerializeField] private float bigHitCooldownTime = 2.0f;


    [Header("Small hit settings")]
    [SerializeField] private int maxAmountSmallHits = 3;
    [SerializeField] private float hitOpportunisticTime = 1.0f;
    [SerializeField] private float smallHitCooldownMovement;

    private int amountOfSmallhits = 0;
    private float currHealth;
    private bool isHittable = true;

    [HideInInspector] public HitEventService hitService;

    [SerializeField] private float deathHitCooldownMovement;


    void Start()
    {
        hitService = new HitEventService();
        resetHealth();
    }

    public float getPercentage()
    {
        return currHealth / maxHealth;
    }

    public void resetHealth()
    {
        currHealth = maxHealth;
        GetComponent<PlayerChangeUI>().changePlayersHealth();
        GetComponent<Power>().resetPower();
    }

    public void receiveHit(float dmg, Vector2 knockback, bool isOpportunistic)
    {
        if (!isHittable)
            return;

        StopAllCoroutines();

        currHealth -= dmg;

        if(currHealth <= 0) //Death
        {
            GetComponent<PlayerAnimations>().PlayDeath();
            StartCoroutine(GetComponent<Main_Input_Player>().setFreezed(deathHitCooldownMovement));
            hitService.OnBigHitReceived.InvokeEvent();
            FindFirstObjectByType<StateMachine>().EndRoundEarly(this.gameObject);
            return;
        }

        if(!isOpportunistic || amountOfSmallhits == maxAmountSmallHits - 1) //big hit
        {
            GetComponent<PlayerAnimations>().PlayBeingAttacked(true);
            StartCoroutine(hitCooldown(bigHitCooldownTime));
            StartCoroutine(GetComponent<Main_Input_Player>().setFreezed(bigHitCooldownMovement));
            hitService.OnBigHitReceived.InvokeEvent();
        }
        else //small hit
        {
            amountOfSmallhits++;
            StartCoroutine(GetComponent<Main_Input_Player>().setFreezed(smallHitCooldownMovement));
            GetComponent<PlayerAnimations>().PlayBeingAttacked(false);
            StartCoroutine(opportunisticHit());
            hitService.OnSmallHitReceived.InvokeEvent();
        }

        //GetComponent<Rigidbody2D>().AddForce(knockback, ForceMode2D.Impulse);

        Debug.Log("CURR HEALTH" + currHealth);
    }

    private IEnumerator opportunisticHit()
    {
        yield return new WaitForSeconds(hitOpportunisticTime);
        StartCoroutine(hitCooldown(0));
    }

    private IEnumerator hitCooldown(float time)
    {
        amountOfSmallhits = 0;
        isHittable = false;
        yield return new WaitForSeconds(time);
        isHittable = true;
    }
}
