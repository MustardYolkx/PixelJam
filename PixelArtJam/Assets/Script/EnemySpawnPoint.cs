using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public GameObject player;
    public Door triggerDoor;
    public bool isDoorOpen;

    public Door targetDoor;

    public enum StartTrigger
    {
        None,
        DoorOpen,
    }
    public StartTrigger startTrigger;
    public enum SpwanObjType
    {
        Enemy,
    }
    public SpwanObjType spawnObjType;
    public GameObject spawnObj;
    public int spawnObjIndex;
    public bool canEnemyMove;
    public bool canEnemyDestroyed;
    public bool isThisEnemyLevelCondition;

    private GameObject targetEnemy;

    public enum TargetTrigger
    {
        SpawnOnce,
        SpawnLoopByEnemyDie,
        SpawnLoopByTime,
        Player,
        Enmey,
    }
    public TargetTrigger targetTrigger;
    // Start is called before the first frame update

    public enum PlayerTriggerCondition
    {
        HP,
        WaterStorage,
    }
    public PlayerTriggerCondition playerTriggerCondition;

    public enum Condition
    {
        Greater,
        Less,
        Equal,
        Alive,
        Die,
    }
    public Condition condition;

    public float value;

    private int count;
    public int spawnNumber;

    public float loopTime;
    private float loopTimeCount;

    public bool triggerAnyDoor;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(startTrigger == StartTrigger.None)
        {
            if (targetTrigger == TargetTrigger.SpawnOnce)
            {
                TargetCondition();
            }
            else if (targetTrigger == TargetTrigger.SpawnLoopByEnemyDie)
            {
                TargetCondition(spawnObj,spawnNumber);
            }
            else if (targetTrigger == TargetTrigger.Player)
            {
                TargetCondition(player, value, condition, playerTriggerCondition);
            }
        }
        else if(startTrigger == StartTrigger.DoorOpen)
        {
            if (triggerDoor==null)
            {
                if (targetTrigger == TargetTrigger.SpawnLoopByEnemyDie)
                {
                    TargetCondition(spawnObj,spawnNumber);
                }
            }
        }

        loopTimeCount += Time.deltaTime;
    }
    public void DoorOpenTrigger()
    {
        isDoorOpen= true;
    }

    public void TargetCondition()
    {
        if (count == 0)
        {
            SpawnEnemy(spawnObj, spawnObjIndex);
            count++;
        }
        
    }
    public void TargetCondition(GameObject spawnObj,int number)
    {
        if(count == 0)
        {
            SpawnEnemy(spawnObj, spawnObjIndex);
            count++;
        }
        else if(count<number)
        {
            if (targetEnemy == null)
            {
                SpawnEnemy(spawnObj, spawnObjIndex);
                count++;
            }
        }
    }
    public void TargetCondition(float loopTime)
    {
        if (loopTimeCount>loopTime)
        {
            SpawnEnemy(spawnObj, spawnObjIndex);
            loopTimeCount = 0;
            
        }
        
    }
    public void TargetCondition(GameObject targetObj,float value,Condition condition,PlayerTriggerCondition playerTriggerCondition)
    {
        PlayerScr player = targetObj.GetComponent<PlayerScr>();
        if(player != null)
        {
            if (playerTriggerCondition == PlayerTriggerCondition.WaterStorage)
            {
                if (condition == Condition.Greater)
                {
                    if (player.waterStorage > value)
                    {
                        if(count == 0)
                        {
                            SpawnEnemy(spawnObj, spawnObjIndex);
                            count++;
                        }
                    }
                }
            }
            
        }
        
    }

    public void SpawnEnemy(GameObject spawnEnemy,int index)
    {
        if(spawnObjType == SpwanObjType.Enemy)
        {
            targetEnemy = Instantiate(spawnEnemy, transform.position, Quaternion.identity);
            Enemy enemy = targetEnemy.GetComponent<Enemy>();
                
            if (enemy != null)
             {
                enemy.patrolIndex = index;

                if (canEnemyMove)
                {
                    enemy.isMovable = true;                  
                }
                if (!canEnemyDestroyed)
                {
                    enemy.hp = 9999;
                }
                if (triggerAnyDoor)
                {
                    enemy.targetDoor = targetDoor;
                    
                    enemy.isTriggerDoor = true;                    
                }
                if(isThisEnemyLevelCondition)
                {
                    enemy.level1Condition = FindObjectOfType<GameRoot>();
                    enemy.AddThisEnemyToGameRoot();
                }
            }
        }
        
    }
    public void TriggerDoor()
    {
        targetDoor.SetTriggerTarget(targetEnemy);
    }
}
