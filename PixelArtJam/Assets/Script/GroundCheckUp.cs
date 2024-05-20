using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckUp : MonoBehaviour
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
        PlayerScr player = collision.gameObject.GetComponent<PlayerScr>();
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (player != null)
        {
            player.groundUpCheck = true;
        }
        if (enemy != null)
        {
            enemy.groundUpCheck = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerScr player = collision.gameObject.GetComponent<PlayerScr>();
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (player != null)
        {
            player.groundUpCheck = true;
        }
        if (enemy != null)
        {
            enemy.groundUpCheck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerScr player = collision.gameObject.GetComponent<PlayerScr>();
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (player != null)
        {
            player.groundUpCheck = false;
        }
        if (enemy != null)
        {
            enemy.groundUpCheck = true;
        }
    }
}
