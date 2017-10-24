using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ToggleButton : MonoBehaviour, IPointerDownHandler
{
    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Update(){}

    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("isPressed", !animator.GetBool("isPressed"));

        Action(animator.GetBool("isPressed"));
    }

    /// <summary>
    /// ボタンがタップされた時のアクション
    /// </summary>
    /// <param name="isPressed">タップ後のボタンがPressedになっているか</param>
    protected virtual void Action(bool isPressed)
    {

    }
}