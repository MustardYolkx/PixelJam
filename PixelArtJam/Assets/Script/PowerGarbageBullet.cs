using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGarbageBullet : MonoBehaviour
{
    public float bulletSpeed;
    [HideInInspector] public Vector2 direction;
    public float lifeTime;
    public float damage;
    public float hitForce;
    Rigidbody2D rb;
    public float drag;
    public GameObject banana;
    private float liveTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * bulletSpeed, ForceMode2D.Force);
        //Destroy(gameObject, lifeTime);
        rb.drag = drag;
    }

    // Update is called once per frame
    void Update()
    {
        liveTime += Time.deltaTime;
        DestroyByTime(lifeTime);
    }

    public void OnDestroy()
    {
       
    }
    public void DestroyByTime(float time)
    {
        if(liveTime>time)
        {
            Instantiate(banana, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerScr player = collision.GetComponent<PlayerScr>();
        Vector2 direction = (collision.transform.position - transform.position).normalized;
        if (player != null)
        {
            player.rb.AddForce(direction * hitForce, ForceMode2D.Force);
            //CameraShake.Instance.ShakeCamera(2);
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
