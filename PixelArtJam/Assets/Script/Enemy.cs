using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public abstract class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Attribute")]
    public float hp =10;
    public float damage;
    public float mass = 2;
    public float detectRange = 3;
    private float originDetectRange;
    public float alertTime = 0.5f;
    /// <summary>
    /// Movement
    /// </summary>
    [Header("Movement")]
    public float moveSpeed = 5;
    private float currenSpeed;
    public float waitTime = 1;
    public GameObject patrolPoint;
    private MovePoint[] patrolArray;
    private List<MovePoint> patrolList = new List<MovePoint>();

    private Vector2 currentTargetPoint;
    private MovePoint firstTargetPoint;
    private int currentPointIndex;

    private float drag = 5;
    private Collider2D[] col ;

    private Animator anim;
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public PlayerScr playerScr;
    private SpriteRenderer sprite;
    private float localScaleX;

    [Header("VFX")]
    public GameObject dieEffect;
    public GameObject alertEffect;
    public GameObject alerttoChaseEffect;
    public GameObject alertvfxStore;

    public enum EnemyState
    {
        Patrol,
        Alert,
        Chase,
        Charge,
        readyPhase,
        Shoot,
        die,
    }
    public EnemyState currentEnemyState;

    public enum EnemyActionState
    {
        Idle,
        Move,
    }
    public EnemyActionState currentEnemyActionState;
    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        localScaleX = sprite.transform.localScale.x;
        rb = GetComponentInChildren<Rigidbody2D>();
        col= GetComponentsInChildren<Collider2D>();
        originDetectRange = detectRange;
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
        currenSpeed = moveSpeed;
        rb.drag= drag;
    }

    // Update is called once per frame
    public void Update()
    {
        if (hp <= 0&&isAlive)
        {
            EnemyDie(2);
        }

        if(currentEnemyState == EnemyState.Chase)
        {
            detectRange = originDetectRange + 2;
        }
        else
        {
            detectRange = originDetectRange;
        }
    }
    public void FaceToPatrolTarget()
    {
        if (transform.position.x < currentTargetPoint.x)
        {
            sprite.transform.localScale = new Vector2(localScaleX,sprite.transform.localScale.y);
        }
        else
        {
            sprite.transform.localScale = new Vector2(-localScaleX, sprite.transform.localScale.y);
        }
    }

    public void FaceToPlayer()
    {
        if (playerScr != null)
        {
            if (transform.position.x < playerScr.transform.position.x)
            {
                sprite.transform.localScale = new Vector2(localScaleX, sprite.transform.localScale.y);
            }
            else
            {
                sprite.transform.localScale = new Vector2(-localScaleX, sprite.transform.localScale.y);
            }
        }
        
    }
    public void MoveState()
    {
        if(currentEnemyActionState == EnemyActionState.Idle)
        {
            currenSpeed = 0;

                anim.SetBool("isMove", false);
            
            
        }
        else if (currentEnemyActionState == EnemyActionState.Move)
        {

                anim.SetBool("isMove", true);
            
            currenSpeed = moveSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        CameraShake.Instance.ShakeCamera(2);
        anim.SetTrigger("underAtk");
        hp -= damage;
    }

    public void EnemyDie(float aniTime)
    {
        
        StartCoroutine(DiePorcess(aniTime));

    }
    IEnumerator DiePorcess(float aniTime)
    {
        isAlive = false;
        CameraShake.Instance.ShakeCamera(3);
        currentEnemyActionState = EnemyActionState.Idle;
        if (alertvfxStore != null)
        {
            alertvfxStore.GetComponentInChildren<DestroyMe>().DestroyMyself(0.1f);
        }
        TurnOffCollider();
        Instantiate(dieEffect,transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.4f);
        anim.SetTrigger("die");
        yield return new WaitForSeconds(aniTime);
        
        Destroy(gameObject);
    }
    public void TurnOnCollider()
    {
        foreach(Collider2D co in col)
        {
            co.enabled = true;
        }
       
    }

    public void TurnOffCollider()
    {
        foreach (Collider2D co in col)
        {
            co.enabled = false;
        }
        
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
            case EnemyState.Chase:
                //currentEnemyActionState = EnemyActionState.Move;
                break;
        }
    }

    IEnumerator AlertTime()
    {
        float time = 0;
        Instantiate(alertEffect,transform);
        while (time < alertTime)
        {
            yield return null;
            time += Time.deltaTime;
        }
        if(currentEnemyState== EnemyState.Alert)
        {
            if(alertvfxStore == null)
            {
                alertvfxStore = Instantiate(alerttoChaseEffect, transform);
            }
            
            currentEnemyState = EnemyState.Chase;           
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
    public void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTargetPoint, currenSpeed * Time.deltaTime);
        if (Vector2.Distance(currentTargetPoint, transform.position) < 0.1f)
        {
            StartCoroutine(IdleWait(waitTime));
            SearchNextPatrolPoint();
        }
    }

    public IEnumerator IdleWait(float stayTime)
    {
        currentEnemyActionState = EnemyActionState.Idle;
        float time = 0;
        while(time<stayTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        currentEnemyActionState = EnemyActionState.Move;
        
        
    }

    //public IEnumerator AttackWait(EnemyState enemyState, float stayTime)
    //{
    //    currentEnemyActionState = EnemyActionState.Idle;
    //    float time = 0;
    //    while (time < stayTime)
    //    {
    //        time += Time.deltaTime;
    //        yield return null;
    //    }
    //    currentEnemyState = enemyState;

    //}
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
        transform.position = Vector2.MoveTowards(transform.position,player, currenSpeed * Time.deltaTime);
    }
    #endregion
}
