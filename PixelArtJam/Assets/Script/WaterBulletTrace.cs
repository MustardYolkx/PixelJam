using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBulletTrace : MonoBehaviour
{

    Rigidbody2D rb;
    public float bulletSpeed;
    public Vector2 direction;
    public float lifeTime;
    private float time;
    private int count;
    public float loseScale;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * bulletSpeed, ForceMode2D.Force);
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale -= Vector3.one * loseScale * Time.deltaTime;
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
