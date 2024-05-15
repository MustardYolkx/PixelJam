using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByPlayerBullet : MonoBehaviour
{

    Collider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WaterBullet bullet1 = collision.gameObject.GetComponent<WaterBullet>();
        WaterBulletSingle bulletSingle = collision.gameObject.GetComponent<WaterBulletSingle>();
        if(bullet1!= null||bulletSingle!=null) 
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
