using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildButton : MonoBehaviour, IPointerDownHandler {

    Animator animator;
    [SerializeField]
    BuildPanelManager attachedPanel;    //関連するBuildPanelManager

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("isPressed", !animator.GetBool("isPressed"));
        if (animator.GetBool("isPressed"))
        {
            attachedPanel.OpenPanel();
        }
        else
        {
            attachedPanel.ClosePanel();
        }
    }
}
