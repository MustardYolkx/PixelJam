using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_OnionRing : Enemy
{
    [Header("Onion_Attribute")]

    public float attackWaiting = 1.3f;
    public LayerMask playerLayer;

    [Header("Onion_Attack")]
    public GameObject bulletTargetPar;
    public GameObject bullet1TargetPos;
    public GameObject bullet2TargetPos;
    public GameObject bullet3TargetPos;

    public GameObject bullet;
    public float attackCoolDownTime;
    private float attackTimeCount = 10f;
    private int count;
    // Start is called before the first frame update
    new void Start()
    {
        if (isMovable)
        {
            currentEnemyActionState = EnemyActionState.Move;
        }
        else
        {
            currentEnemyActionState = EnemyActionState.Idle;
        }
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        if(isAlive)
        {
            if (isMovable)
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
            }
           
            
        }
        MoveState();
        base.Update();
    }

    IEnumerator AttackDelay()
    {
        currentEnemyState = EnemyState.Shoot;
        anim.SetTrigger("Ready");
        Vector2 direction = (playerScr.transform.position - transform.position).normalized;
        float angle = Vector2.Angle(direction, Vector2.right);
        
        if (playerScr.transform.position.y > transform.position.y)
        {
            bulletTargetPar.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            bulletTargetPar.transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
        yield return new WaitForSeconds(1.5f);
        if (isAlive)
        {
            
        
        GameObject bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);
        bullet1.GetComponent<OnionRingBullet>().direction = (bullet1TargetPos.transform.position - transform.position).normalized;
        GameObject bullet2 = Instantiate(bullet, transform.position, Quaternion.identity);
        bullet2.GetComponent<OnionRingBullet>().direction = (bullet2TargetPos.transform.position - transform.position).normalized;
        GameObject bullet3 = Instantiate(bullet, transform.position, Quaternion.identity);
        bullet3.GetComponent<OnionRingBullet>().direction = (bullet3TargetPos.transform.position - transform.position).normalized;
        }
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Idle");
        if (Vector2.Distance(transform.position, playerScr.transform.position) < detectRange)
        {
            currentEnemyState = EnemyState.Chase;
        }
        else
        {
            currentEnemyState = EnemyState.Patrol;
            if (alertvfxStore != null)
            {
                alertvfxStore.GetComponentInChildren<DestroyMe>().DestroyMyself(0.1f);
            }
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
            if(currentEnemyState!=EnemyState.Shoot)
            {
                currentEnemyState = EnemyState.Patrol;
                
                StartCoroutine(IdleWait( 1f));
                StateChange();
                if (alertvfxStore != null)
                {
                    alertvfxStore.GetComponentInChildren<DestroyMe>().DestroyMyself(0.1f);
                }
                attackTimeCount = 10;
            }

           
            count = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }
}
