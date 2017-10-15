using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ToggleButton : MonoBehaviour, IPointerDownHandler
{
    protected Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        MyStart();
    }
    /// <summary>
    /// ToggleButtonクラスのStartの後に呼ばれる関数 継承用
    /// </summary>
    protected virtual void MyStart(){}

    private void Update()
    {
        MyUpdate();
    }
    /// <summary>
    /// ToggleButtonクラスのUpdateの後に呼ばれる関数 継承用
    /// </summary>
    protected virtual void MyUpdate(){}

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