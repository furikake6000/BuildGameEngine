using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanelManager : MonoBehaviour {
    
    private Animator panelAnimator;
    private bool isOpen;
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
            facilityButtonTransform[i].anchoredPosition = new Vector3(-350f + 200f * i, -114f, 0f);
        }
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
