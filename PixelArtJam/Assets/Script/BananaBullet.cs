using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaBullet : MonoBehaviour
{
    public float damage;
    public float hitForce;
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
        PlayerScr player = collision.GetComponent<PlayerScr>();
        if (player != null)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            player.TakeDamage(damage);
            player.rb.AddForce(direction * hitForce, ForceMode2D.Force);
            Destroy(gameObject);
        }
    }
}
