using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public float lifeTime;
    public bool autoDestroy = true;
    // Start is called before the first frame update
    void Start()
    {
        if(autoDestroy)
        {
            DestroyMyself(lifeTime);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyMyself(float time)
    {
        Destroy(gameObject,time);
    }
}
