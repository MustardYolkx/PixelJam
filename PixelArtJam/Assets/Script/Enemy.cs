using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Attribute")]
    public float hp =10;
    public float damage;
    public float mass = 2;
    public float detectRange = 3;
    public float alertTime = 0.5f;
    /// <summary>
    /// Movement
    /// </summary>
    [Header("Movement")]
    public float moveSpeed = 5;
    public float waitTime = 1;
    public GameObject patrolPoint;
    private MovePoint[] patrolArray;
    private List<MovePoint> patrolList = new List<MovePoint>();

    private Vector2 currentTargetPoint;
    private MovePoint firstTargetPoint;
    private int currentPointIndex;

    private float drag = 5;
    private Collider2D col;
    
    [HideInInspector] public PlayerScr playerScr;

    public enum EnemyState
    {
        Idle,
        Patrol,
        Alert,
        Chase,
        Charge,
        readyPhase,
        Shoot,
    }
    public EnemyState currentEnemyState;
    // Start is called before the first frame update
    public void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        col= GetComponentInChildren<Collider2D>();
        if(patrolPoint!= null)
        {
            patrolArray = patrolPoint.GetComponentsInChildren<MovePoint>();
            foreach (MovePoint p in patrolArray)
            {
                patrolList.Add(p);
                if (p.index == 1)
                {
                    firstTargetPoint = p;
                    currentTargetPoint = p.transform.position;
                    currentPointIndex = p.index;
                }
            }
            SetFirstPatrolPoint();
        }
       
        rb.drag= drag;
    }

    // Update is called once per frame
    public void Update()
    {
        
        
    }



    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public void EnemyDie()
    {
        Destroy(col);
        Destroy(gameObject);
    }

    public void TurnOnCollider()
    {
        col.enabled = true;
    }

    public void TurnOffCollider()
    {
        col.enabled = false;
    }
    public void StateChange()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Patrol:
                SetFirstPatrolPoint();
                break;
            case EnemyState.Alert:
                StartCoroutine(AlertTime());
                
                break;
        }
    }

    IEnumerator AlertTime()
    {
        float time = 0;
        while (time < alertTime)
        {
            yield return null;
            time += Time.deltaTime;
        }
        if(currentEnemyState== EnemyState.Alert)
        {
            currentEnemyState = EnemyState.Chase;
        }
        else if(currentEnemyState == EnemyState.Charge)
        {

        }
        else if(currentEnemyState == EnemyState.Shoot)
        {

        }
        else 
        {
            currentEnemyState = EnemyState.Patrol;
        }
    }
    #region EnemyPatrol
    public void SetFirstPatrolPoint()
    {

            foreach (MovePoint p in patrolList)
            {
                if (Vector2.Distance(transform.position, currentTargetPoint) > Vector2.Distance(transform.position, p.transform.position))
                {
                    currentTargetPoint = p.transform.position;
                    currentPointIndex = p.index;
                }
            }
        
    }

    private void SearchNextPatrolPoint()
    {
        if (currentPointIndex == patrolList.Count)
        {
            currentTargetPoint = firstTargetPoint.transform.position;
            currentPointIndex = 1;
        }
        else
        {
            foreach (MovePoint p in patrolList)
            {
                if (p.index - currentPointIndex == 1)
                {
                    currentTargetPoint = p.transform.position;
                    currentPointIndex = p.index;
                    break;
                }
            }
        }
        

    }
    public void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTargetPoint, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(currentTargetPoint, transform.position) < 0.1f)
        {
            StartCoroutine(PatrolWait(EnemyState.Patrol,waitTime));
            SearchNextPatrolPoint();
        }
    }

    public IEnumerator PatrolWait(EnemyState enemyState,float stayTime)
    {
        currentEnemyState = EnemyState.Idle;
        float time = 0;
        while(time<stayTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        if (currentEnemyState != EnemyState.Alert)
        {
            currentEnemyState = enemyState;
        }
        
        
    }

    public IEnumerator AttackWait(EnemyState enemyState, float stayTime)
    {
        currentEnemyState = EnemyState.Idle;
        float time = 0;
        while (time < stayTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        currentEnemyState = enemyState;

    }
    #endregion
    public IEnumerator ReadyWait(EnemyState enemyState, float stayTime)
    {
        currentEnemyState = EnemyState.readyPhase;
        float time = 0;
        while (time < stayTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        currentEnemyState = enemyState;

    }
    #region Chase
    public void ChasePlayer(Vector2 player)
    {
        transform.position = Vector2.MoveTowards(transform.position,player, moveSpeed * Time.deltaTime);
    }
    #endregion
}
