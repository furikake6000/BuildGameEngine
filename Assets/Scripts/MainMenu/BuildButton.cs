using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour {

    private Facility facility;
    private Image facilityImage;
    private Text facilityText;

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

        //自分のボタンイベントの設定
        Button myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OnPressed);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPressed()
    {
        //自分のボタンに該当する施設を選択する
        FieldBoardBuilder.SelectedFacility = facility;
    }
}
