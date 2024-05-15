using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    PlayerScr player;
    Rigidbody2D rb;
    SpriteRenderer rend;

    public float bulletSpeed;
    [HideInInspector] public Vector2 direction;
    public float lifeTime;
    public float lineRenderTime;

    public float damage;

    private float time;
    private int count;
    public float loseScale;
    public GameObject trace;
    public float drag;
    private float traceGeneTime;
    private float currentPushForce;
    public float pushForceAdd;
    public float maxPushForce;
    public GameObject collideParticle;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        player = FindObjectOfType<PlayerScr>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * bulletSpeed, ForceMode2D.Force);
        Destroy(gameObject, lifeTime);
        rb.drag = drag;
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        WaterEffect();
        GenerateTrace();
        time += Time.deltaTime;
        traceGeneTime += Time.deltaTime;
        currentPushForce += pushForceAdd * Time.deltaTime;
        if(currentPushForce>maxPushForce)
        {
            currentPushForce= maxPushForce;
        }
    }
    private void WaterEffect()
    {

        if (time > lifeTime - lineRenderTime && count == 0)
        {
            rend.enabled = true;
            player.BulletPosDelete(this.gameObject);
            count++;
        }
        if (time > lifeTime - lineRenderTime)
        {
            transform.localScale -= Vector3.one * loseScale * Time.deltaTime;
        }
        if (transform.localScale.x < -0.2)
        {
            transform.localScale = new Vector3(-0.2f, -0.2f, -0.2f);
        }
    }
    private void OnDestroy()
    {
        player.BulletPosDelete(this.gameObject);
        Instantiate(collideParticle, transform.position, Quaternion.identity);
    }
    private void GenerateTrace()
    {
        float time = Random.Range(0.2f, 0.4f);
        if(traceGeneTime> time)
        {
            GameObject traceObj= Instantiate(trace,transform.position,Quaternion.identity);
            traceObj.GetComponent<WaterBulletTrace>().direction = MoveDirection() + direction;
            traceGeneTime = 0;
        }
    }
    private Vector2 MoveDirection()
    {
        Vector2 lastPos = transform.position;
        Vector2 currentPos = transform.position;
        Vector2 direc = lastPos - currentPos;
        return direc.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponentInChildren<Enemy>();
        if (enemy != null)
        {
            Vector2 pushDirection = enemy.transform.position- transform.position;
            enemy.rb.AddForce(pushDirection.normalized * currentPushForce*(2/enemy.mass), ForceMode2D.Force);
            
            Destroy(gameObject);
        }
    }
}
