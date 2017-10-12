using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanelManager : MonoBehaviour {
    
    private Animator animator;
    private bool isOpen, isCompletelyOpen;  //それぞれ「開き始め～閉じ始め」「開き終わり～閉じ始め」にONとなる

    [SerializeField]
    private Animator attachedButtonAnimator;
    [SerializeField]
    private GameObject facilityButtonPrefab;    //ボタンのプレハブ

    private FieldBoardBuilder builder;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        builder = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoardBuilder>();

        RefreshFacilityButtons();
    }

    private void RefreshFacilityButtons()
    {
        //登録されているFaciitiesを一括取得
        //同時にボタンを作成
        List<Facility> facilityPrefabs = new List<Facility>(Resources.LoadAll<Facility>("Facilities"));
        for (var i = facilityPrefabs.Count - 1; i >= 0; i--)
        {
            //Buildできないものは取り除く
            if (!facilityPrefabs[i].Buildable) facilityPrefabs.RemoveAt(i);
        }
        for (var i = 0; i < facilityPrefabs.Count; i++)
        {
            BuildPanelButton newFacilityButton;
            newFacilityButton = GameObject.Instantiate(facilityButtonPrefab, this.transform).GetComponent<BuildPanelButton>();
            //施設セット
            newFacilityButton.Facility = facilityPrefabs[i];
            //座標あわせ
            newFacilityButton.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-350f + 200f * i, 215f, 0f);
        }
    }

    private void Update()
    {
        if (!isOpen && attachedButtonAnimator.GetBool("isPressed"))
        {
            //パネル開き始め処理
            OpenPanel();
        }

        if (isOpen && !attachedButtonAnimator.GetBool("isPressed"))
        {
            //パネル閉じ処理
            ClosePanel();
            isCompletelyOpen = false;
        }
    }

    public void OpenPanel()
    {
        isOpen = true;
        animator.SetBool("open", true);
    }

    public void ClosePanel()
    {
        isOpen = false;
        animator.SetBool("open", false);

        //Facility未選択状態に
        builder.SelectedFacility = null;
    }
}
