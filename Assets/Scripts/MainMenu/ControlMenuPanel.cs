using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenuPanel : MonoBehaviour {

    //パネルの開閉処理について、public関数を用意しAnimatorから操れるようにする

    private Animator panelAnimator;
    private bool isOpen;
    [SerializeField]
    private Animator attachedButtonAnimator;

    private void Start()
    {
        panelAnimator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isOpen && attachedButtonAnimator.GetBool("isPressed"))
        {
            OpenPanel();
        }

        if (isOpen && !attachedButtonAnimator.GetBool("isPressed"))
        {
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        isOpen = true;
        panelAnimator.SetBool("open", true);
    }

    public void ClosePanel()
    {
        isOpen = false;
        panelAnimator.SetBool("open", false);
    }
}
