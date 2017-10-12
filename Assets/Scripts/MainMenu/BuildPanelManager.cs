using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanelManager : MonoBehaviour {
    
    private Animator animator;

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
        if (!animator.GetBool("open") && attachedButtonAnimator.GetBool("isPressed"))
        {
            //パネル開き始め処理
            OpenPanel();
        }

        if (animator.GetBool("open") && !attachedButtonAnimator.GetBool("isPressed"))
        {
            //パネル閉じ処理
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        animator.SetBool("open", true);
    }

    public void ClosePanel()
    {
        animator.SetBool("open", false);

        //Facility未選択状態に
        builder.SelectedFacility = null;
    }
}
