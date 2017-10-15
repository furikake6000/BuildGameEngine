using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ToggleButton : MonoBehaviour, IPointerDownHandler
{
    Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("isPressed", !animator.GetBool("isPressed"));

        Action(animator.GetBool("isPressed"));
    }

    /// <summary>
    /// ボタンがタップされた時のアクション
    /// </summary>
    /// <param name="isPressed">タップ後のボタンがPressedになっているか</param>
    public abstract void Action(bool isPressed);
}
