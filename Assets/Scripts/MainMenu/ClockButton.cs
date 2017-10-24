using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockButton : ToggleButton {
    
    protected override void Action(bool isPressed)
    {
        //FieldTimeManager.ToggleClockEnabledStatic();
    }

    protected override void Update()
    {
        base.Update();

        animator.SetBool("isPressed", FieldTimeManager.TimeEnabled);
    }

}
