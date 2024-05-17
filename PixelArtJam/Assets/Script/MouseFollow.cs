using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private Vector3 mousePos;
    public float followSmooth;
    public float followSmoothQuick;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButton(0))
        {
            if (Vector2.Distance(transform.position, mousePos) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, mousePos, Time.deltaTime * followSmooth);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, mousePos) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, mousePos, Time.deltaTime * followSmoothQuick);
            }
            
        }
        
    }
}
