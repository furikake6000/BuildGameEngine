using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepressBehaviour : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("animationFinished", false);
        animator.SetBool("isPressed", false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("animationFinished", true);
    }
}
