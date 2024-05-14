using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Can : Enemy
{
    [Header("Can_Attribute")]
    
    public float hitForce;
    public float attackWaiting =1.3f;
    public LayerMask playerLayer;
    
    private int count;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        if (currentEnemyState == EnemyState.Patrol)
        {
            Movement();
        }
        if( currentEnemyState == EnemyState.Chase)
        {            
            ChasePlayer(playerScr.transform.position);
        }
        SearchPlayer();
        
    }

   private void SearchPlayer()
    {
        
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectRange, playerLayer);
        if(player != null)
        {
            PlayerScr pl = player.GetComponentInChildren<PlayerScr>();
            if (pl != null && count == 0)
            {
                playerScr = pl;
                
                currentEnemyState= EnemyState.Alert;
                StateChange();
                count = 1;
            }
            
        }
        if(player == null&&count == 1)         
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
            player.rb.AddForce(direction * hitForce,ForceMode2D.Force);
            player.TakeDamage(damage);
            rb.AddForce(-direction * hitForce,ForceMode2D.Force);
            StartCoroutine(PatrolWait(EnemyState.Chase,attackWaiting));
        }
    }
}
