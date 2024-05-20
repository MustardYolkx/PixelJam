using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimTrigger : MonoBehaviour
{

    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void destroyThis()
    {
        enemy.DestroyedThis();
    }
}
