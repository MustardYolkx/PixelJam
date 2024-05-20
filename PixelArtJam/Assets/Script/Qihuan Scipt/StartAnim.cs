using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnim : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void AnimStart()
    {
        animator.SetTrigger("StartGame");
    }
}
