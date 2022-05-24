using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    public Animator animator;
    public bool walking;
    public bool running;
    public bool attacking; // future implementation

    // Update is called once per frame
    void Update()
    {
        
        animator.SetBool("walking", walking);
        animator.SetBool("running", running);
        //animator.SetBool("attacking",attacking);

    }


    public void Fire(){

        animator.SetTrigger("fire");

    }

    public void ResetFire(){

        animator.ResetTrigger("fire");

    }

    public void Die(){

        animator.SetTrigger("die");

    }

    public void ResetDie(){

        animator.ResetTrigger("die");

    }


}