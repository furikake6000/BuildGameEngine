﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressBehaviour : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("animationFinished", false);
        animator.SetBool("isPressed", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("animationFinished", true);
    }
}
