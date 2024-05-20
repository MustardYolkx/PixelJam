using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerScr player = collision.gameObject.GetComponent<PlayerScr>();
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (player != null)
        {
            player.isOnGround = true;
        }
        if (enemy != null)
        {
            enemy.isOnground = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerScr player = collision.gameObject.GetComponent<PlayerScr>();
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (player != null)
        {
            player.isOnGround = false;
            player.FallingDown();
        }
        if(enemy!=null)
        {
            enemy.isOnground= false;
        }
    }
}
