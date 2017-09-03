using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanelManager : MonoBehaviour {

    //オブジェクトを視認外に移動させたい時の座標（定数）
    private static readonly Vector3 HidePosition = new Vector3(-9999f, -9999f, -9999f);

    private Animator panelAnimator;
    private bool isOpen, isCompletelyOpen;  //それぞれ「開き始め～閉じ始め」「開き終わり～閉じ始め」にONとなる
    private Facility[] facilityPrefabs;
    private BuildButton[] facilityButtons;
    private RectTransform[] facilityButtonTransform;

    [SerializeField]
    private Animator attachedButtonAnimator;
    [SerializeField]
    private GameObject facilityButtonPrefab;    //ボタンのプレハブ

    private void Start()
    {
        panelAnimator = this.GetComponent<Animator>();

        //登録されているFaciitiesを一括取得
        //同時にボタンを作成
        facilityPrefabs = Resources.LoadAll<Facility>("Facilities");
        facilityButtons = new BuildButton[facilityPrefabs.Length];
        facilityButtonTransform = new RectTransform[facilityPrefabs.Length];
        for(var i = 0; i < facilityPrefabs.Length; i++)
        {
            facilityButtons[i] = GameObject.Instantiate(facilityButtonPrefab, this.transform).GetComponent<BuildButton>();
            //施設セット
            facilityButtons[i].Facility = facilityPrefabs[i];
            //座標あわせ
            facilityButtonTransform[i] = facilityButtons[i].gameObject.GetComponent<RectTransform>();
            //facilityButtonTransform[i].anchoredPosition = new Vector3(-350f + 200f * i, -114f, 0f);
            facilityButtonTransform[i].anchoredPosition = HidePosition;
        }
    }

    private void Update()
    {
        if (!isOpen && attachedButtonAnimator.GetBool("isPressed"))
        {
            //パネル開き始め処理
            OpenPanel();
        }

        if(!isCompletelyOpen && attachedButtonAnimator.GetBool("isPressed") && attachedButtonAnimator.GetBool("animationFinished"))
        {
            //パネル完全に開いた処理
            isCompletelyOpen = true;
            for (var i = 0; i < facilityButtonTransform.Length; i++)
            {
                //座標あわせ
                facilityButtonTransform[i].anchoredPosition = new Vector3(-350f + 200f * i, -114f, 0f);
            }
        }

        if (isOpen && !attachedButtonAnimator.GetBool("isPressed"))
        {
            ClosePanel();
            isCompletelyOpen = false;
            for (var i = 0; i < facilityButtonTransform.Length; i++)
            {
                //座標あわせ(見えない所に)
                facilityButtonTransform[i].anchoredPosition = HidePosition;
            }
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
