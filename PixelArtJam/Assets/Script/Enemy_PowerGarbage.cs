using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_PowerGarbage : Enemy
{
    [Header("PowerGarbage_Attribute")]

    public float attackWaiting = 1.3f;
    public LayerMask playerLayer;

    [Header("PowerGarbage_Attack")]

    public GameObject bullet;

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

        }
        if (attackTimeCount > attackCoolDownTime)
        {
            if (currentEnemyState == EnemyState.Chase)
            {
                Attack();
                attackTimeCount = 0;
            }

        }
        attackTimeCount += Time.deltaTime;
        SearchPlayer();
        MoveState();
    }

    IEnumerator AttackDelay()
    {
        currentEnemyState = EnemyState.Shoot;
        Vector2 direction = (playerScr.transform.position - transform.position).normalized;
        
        
        yield return new WaitForSeconds(0.5f);
        GameObject bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);
        bullet1.GetComponent<PowerGarbageBullet>().direction = direction;
        
        yield return new WaitForSeconds(1.5f);
        if (Vector2.Distance(transform.position, playerScr.transform.position) < detectRange)
        {
            currentEnemyState = EnemyState.Chase;
        }
        else
        {
            currentEnemyState = EnemyState.Patrol;
        }

    }
    private void Attack()
    {
        if (playerScr != null)
        {

            StartCoroutine(AttackDelay());

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
            if (currentEnemyState != EnemyState.Shoot)
            {
                currentEnemyState = EnemyState.Patrol;
                StartCoroutine(IdleWait(1f));
                attackTimeCount = 10;
            }


            count = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}