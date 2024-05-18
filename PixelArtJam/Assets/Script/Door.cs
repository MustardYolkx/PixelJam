using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject triggerTarget;

    public bool isDoorOpen;
    private int count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTriggerTarget(GameObject targetObj)
    {
        count++;
    }
    public void TargetDie(GameObject triggerTarget)
    {
        if(triggerTarget == null)
        {
            Open();
        }
    }
    public void Open()
    {
        StartCoroutine(OpenProcess());
    }
    IEnumerator OpenProcess()
    {
        isDoorOpen= true;
        yield return null;
        Destroy(gameObject);
    }
}
