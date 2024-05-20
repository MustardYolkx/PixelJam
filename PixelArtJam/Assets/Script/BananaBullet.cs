using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BananaBullet : MonoBehaviour
{
    public float damage;
    public float hitForce;
    /*[HideInInspector] */public bool isOnground = false;
    // Start is called before the first frame update
    void Start()
    {
        isOnground = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerScr player = collision.GetComponent<PlayerScr>();
        if (player != null)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            player.TakeDamage(damage);
            player.rb.AddForce(direction * hitForce, ForceMode2D.Force);
            Destroy(gameObject);
        }

        GroundCheck ground = collision.GetComponent<GroundCheck>();
        if (ground != null)
        {
            isOnground = true;
        }


    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        OceanDamage ocean = collision.GetComponent<OceanDamage>();
       
        if (ocean != null)
        {
            if (!isOnground)
            {
                Destroy(gameObject);
            }

        }
    }
}
