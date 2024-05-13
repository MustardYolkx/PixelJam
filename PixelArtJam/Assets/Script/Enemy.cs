using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public float mass;
    private float drag = 5;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        rb.drag= drag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
