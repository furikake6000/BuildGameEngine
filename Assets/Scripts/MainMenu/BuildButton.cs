using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildButton : MonoBehaviour, IPointerDownHandler {

    Animator animator;
    [SerializeField]
    BuildPanelManager linkedBuildPanel; //このボタンを押すことによって開くパネル

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("isPressed", !animator.GetBool("isPressed"));
    }
}
