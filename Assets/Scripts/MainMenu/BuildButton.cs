using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildButton : ToggleButton {

    [SerializeField]
    BuildPanelManager attachedPanel;    //関連するBuildPanelManager

    public override void Action(bool isPressed)
    {
        if (isPressed)
        {
            attachedPanel.OpenPanel();
        }
        else
        {
            attachedPanel.ClosePanel();
        }
    }
}
