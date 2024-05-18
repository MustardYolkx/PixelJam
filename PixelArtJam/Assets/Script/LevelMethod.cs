using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMethod : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoorTrigger(Door door)
    {
        door.Open();
    }
}
