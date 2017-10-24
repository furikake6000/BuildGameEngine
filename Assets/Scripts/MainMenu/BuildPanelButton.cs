using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FrikLib;

public class BuildPanelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private Facility facility;
    private Image facilityImage;
    private Text facilityText;

    private static FieldBoard board;
    private static FieldBoardBuilder builder;

    public Facility Facility
    {
        get
        {
            return facility;
        }

        set
        {
            facility = value;

            //もしロードされてなければロードする
            if (facilityImage == null) facilityImage = this.GetComponentsInChildren<Image>()[1];
            if (facilityText == null) facilityText = this.GetComponentInChildren<Text>();

            if (facility == null)
            {
                facilityImage = null;
                facilityText.text = "";
            }
            else
            {
                facilityImage.sprite = facility.gameObject.GetComponent<SpriteRenderer>().sprite;
                facilityText.text = facility.FacilityName;
            }
        }
    }

    // Use this for initialization
    void Start () {
        GameObject boardObj = GameObject.FindGameObjectWithTag("FieldBoard");
        if(board == null) board = boardObj.GetComponent<FieldBoard>();
        if(builder == null) builder = boardObj.GetComponent<FieldBoardBuilder>();
	}

    //自分がタップされたら
    public void OnPointerDown(PointerEventData eventData)
    {
        //自分のボタンに該当する施設を選択する
        builder.SelectedFacility = facility;

        //建築物詳細表示
        MultiSceneManager.AddSubScene("BuildDetailWindow");
        BuildDetailWindow.ChangeSelectedFacility(facility);
    }

    //自分がタップされた後に離されたら
    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 nowPointingPosition = board.WorldPosToMapPos(Camera.main.ScreenToWorldPoint(eventData.position));
        Vector2Int nowPointingLocation = Vector2Int.Sishagonyu(nowPointingPosition);

        //施設設置
        builder.PutFacility(facility, nowPointingLocation);

        //詳細表示消す
        MultiSceneManager.RemoveSubScene("BuildDetailWindow");

        //オブジェクト選択は未選択状態に
        builder.SelectedFacility = null;
    }
}
