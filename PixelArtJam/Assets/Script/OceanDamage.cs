using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanDamage : MonoBehaviour
{

    public float timeDuration;
    public float timeCount;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerScr player = collision.GetComponent<PlayerScr>();
        Enemy enemy = collision.GetComponent<Enemy>();

        if (timeCount > timeDuration)
        {
            if (enemy != null)
            {
                if (!enemy.isOnground)
                {
                    //enemy.TakeDamage(damage);
                    timeCount = 0;
                }
            }
            if (player != null)
            {
                if (!player.isOnGround)
                {
                    //player.TakeDamage(damage);
                    timeCount = 0;
                }
                
            }
        }
        
        
    }
}
