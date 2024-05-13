using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float storage;
    public float capcity;

    private Vector3 originScale;
    // Start is called before the first frame update
    void Start()
    {
        originScale = transform.localScale;
        storage = capcity;
    }

    // Update is called once per frame
    void Update()
    {
        float proportion = storage / capcity;
        transform.localScale = proportion* originScale;
    }
}
