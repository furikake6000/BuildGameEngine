using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ToggleButton : MonoBehaviour, IPointerDownHandler
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
    protected abstract void Action(bool isPressed);
}