using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBoardBuilder : MonoBehaviour {

    //オブジェクトを視認外に移動させたい時の座標（定数）
    private static readonly Vector3 HidePosition = new Vector3(-999f, -999f, -999f);

    [SerializeField]
    private GameObject firstSelectedFacility; //初期選択施設（デバッグ用）

    private FieldBoard myBoard; //自分のFieldBoardコンポーネント
    private Facility selectedFacility;   //現在設置選択しているファシリティ
    public Facility SelectedFacility
    {
        get
        {
            return selectedFacility;
        }

        set
        {
            selectedFacility = value;
            //プレビュ処理
            //既にプレビュオブジェクトが存在していた場合は抹消する
            if (previewFacilityObject != null)
            {
                Destroy(previewFacilityObject);
            }
            //プレビュオブジェクトの生成(※無を選択した場合は生成しなくていい)
            if (selectedFacility != null)
            {
                //インスタンティエイト（視認外座標に生成）
                previewFacilityObject = GameObject.Instantiate(selectedFacility.gameObject, HidePosition, Quaternion.identity);
                //半透明に
                previewFacilityObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }

    private GameObject previewFacilityObject; //プレビューのファシリティ

    // Use this for initialization
    void Start () {
        //ロード系
        myBoard = this.GetComponent<FieldBoard>();

        //無を選択
        SelectedFacility = null;

        //初期選択施設を選択（デバッグ用）
        SelectedFacility = firstSelectedFacility.GetComponent<Facility>();
	}
	
	// Update is called once per frame
	void Update () {
        //なんらかのFacilityを選択していれば
        if(SelectedFacility != null)
        {
            //マウスが指している場所が設置可能領域だったらプレビュ表示
            Vector2Int nowPointingLocation = Vector2Int.Sishagonyu(myBoard.WorldPosToMapPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            if (myBoard.CanIPutFacility(SelectedFacility, nowPointingLocation))
            {
                previewFacilityObject.transform.position = myBoard.CalcFacilityWorldPos(SelectedFacility, nowPointingLocation);
                
                //マウスが押されていたら施設配置
                if (Input.GetMouseButton(0))
                {
                    myBoard.PutFacility(SelectedFacility, nowPointingLocation);
                }
            }
            else
            {
                previewFacilityObject.transform.position = HidePosition;
            }
        }
    }
}
