using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SadGarbage : Enemy
{
    [Header("SadGarbage_Attribute")]

    public float hitForce;
    public float attackWaiting = 1.3f;
    public LayerMask playerLayer;

    [Header("SadGarbage_Attack")]
    public float chargeRange;
    public float chargeForce;
    public float attackCoolDownTime;
    private float attackTimeCount = 10f;
    private int count;
    // Start is called before the first frame update
    new void Start()
    {
        currentEnemyActionState = EnemyActionState.Move;
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        if (currentEnemyState == EnemyState.Patrol)
        {
            Patrol();
        }
        if (currentEnemyState == EnemyState.Chase)
        {
            ChasePlayer(playerScr.transform.position);
        }
        if (attackTimeCount > attackCoolDownTime)
        {
            if(currentEnemyState== EnemyState.Chase)
            {
                Charge();               
            }
            
        }
        attackTimeCount += Time.deltaTime;
        SearchPlayer();
        MoveState();
    }

    IEnumerator ChargeDelay()
    {
        currentEnemyState = EnemyState.Charge;
        Vector2 direction = (playerScr.transform.position - transform.position).normalized;
        attackTimeCount = 0;
        yield return new WaitForSeconds(0.5f);
        rb.AddForce(direction * chargeForce, ForceMode2D.Force);
        yield return new WaitForSeconds(1f);
        if(currentEnemyState!=EnemyState.Patrol)
        {
            currentEnemyState = EnemyState.Chase;
        }
        
    }
    private void Charge()
    {
        if(playerScr!=null)
        {
            if (Vector2.Distance(playerScr.transform.position, transform.position) < chargeRange)
            {
                StartCoroutine(ChargeDelay());
              
            }
        }
       
    }
    private void SearchPlayer()
    {

        Collider2D player = Physics2D.OverlapCircle(transform.position, detectRange, playerLayer);
        if (player != null)
        {
            PlayerScr pl = player.GetComponentInChildren<PlayerScr>();
            if (pl != null && count == 0)
            {
                playerScr = pl;

                currentEnemyState = EnemyState.Alert;
                StateChange();
                count = 1;
            }

        }
        if (player == null && count == 1)
        {
            currentEnemyState = EnemyState.Patrol;

            count = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerScr player = collision.GetComponentInChildren<PlayerScr>();
        if (player != null)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            if (currentEnemyState!= EnemyState.Charge)
            {
                player.rb.AddForce(direction * hitForce, ForceMode2D.Force);
                player.TakeDamage(damage);
                rb.AddForce(-direction * hitForce, ForceMode2D.Force);
                StartCoroutine(IdleWait( attackWaiting));
            }
            else if(currentEnemyState == EnemyState.Charge)
            {
                player.rb.AddForce(direction * Mathf.Clamp(hitForce * rb.velocity.magnitude, hitForce, chargeForce+hitForce), ForceMode2D.Force);
                player.TakeDamage(damage);
                rb.AddForce(-direction * hitForce, ForceMode2D.Force);
                StartCoroutine(IdleWait( attackWaiting));
            }
            
        }
    }
}