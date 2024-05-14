using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnionRingBullet : MonoBehaviour
{

    public float bulletSpeed;
    [HideInInspector] public Vector2 direction;
    public float lifeTime;
    Rigidbody2D rb;
    public float drag;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * bulletSpeed, ForceMode2D.Force);
        Destroy(gameObject, lifeTime);
        rb.drag = drag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
