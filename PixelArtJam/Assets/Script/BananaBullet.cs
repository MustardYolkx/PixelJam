using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BananaBullet : MonoBehaviour
{
    public float damage;
    public float hitForce;
    /*[HideInInspector] */public bool isOnground = false;


    //animation
    Animator animator;
    void Start()
    {
        isOnground = false;
        animator = GetComponent<Animator>();
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
            //trigger explode anim and unable the boxcollider to make sure it won't trigger again
            animator.SetTrigger("Explode");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;


            Vector2 direction = (collision.transform.position - transform.position).normalized;
            player.TakeDamage(damage);
            player.rb.AddForce(direction * hitForce, ForceMode2D.Force);
            //Destroy(gameObject); move destroy to animation event trigger 
            
        }

        GroundCheck ground = collision.GetComponent<GroundCheck>();
        if (ground != null)
        {
            isOnground = true;
        }

    }
    public void Destroy()
    {
        Destroy(gameObject);
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
