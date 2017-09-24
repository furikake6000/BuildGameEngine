using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBoardBuilder : MonoBehaviour {

    //オブジェクトを視認外に移動させたい時の座標（定数）
    private static readonly Vector3 HidePosition = new Vector3(-999f, -999f, -999f);

    private static readonly Color previewFacilityColor = new Color(1f, 1f, 1f, 0.5f); //プレビュオブジェクトの色
    private static readonly Color previewFacilityDisableColor = new Color(1f, 0.5f, 0.5f, 0.5f); //設置不可時色

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
                //レンダラ読み込み
                previewFacilityRenderer = previewFacilityObject.GetComponent<SpriteRenderer>();
                //半透明に
                previewFacilityRenderer.color = new Color(1f, 1f, 1f, 0.5f);

                //プレビュオブジェクトのFacilityBehaviourを全て停止
                foreach(var facilityBehaviour in previewFacilityObject.GetComponents<FacilityBehaviour>())
                {
                    facilityBehaviour.enabled = false;
                }
            }
        }
    }

    private GameObject previewFacilityObject; //プレビューのファシリティ
    private SpriteRenderer previewFacilityRenderer;  //プレビューファシリティのレンダラ

    // Use this for initialization
    void Start () {
        //ロード系
        myBoard = this.GetComponent<FieldBoard>();

        //無を選択
        SelectedFacility = null;
    }
	
	// Update is called once per frame
	void Update () {
        //なんらかのFacilityを選択していれば
        if(SelectedFacility != null)
        {
            ////マウスが指している場所が設置可能領域だったらプレビュ表示
            //Vector2Int nowPointingLocation = Vector2Int.Sishagonyu(myBoard.WorldPosToMapPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            //if (myBoard.CanIPutFacility(SelectedFacility, nowPointingLocation))
            //{
            //    previewFacilityObject.transform.position = myBoard.CalcFacilityWorldPos(SelectedFacility, nowPointingLocation);

            //    //マウスが押されていたら施設配置
            //    if (Input.GetMouseButton(0))
            //    {
            //        PutFacility(SelectedFacility, nowPointingLocation);
            //    }
            //}
            //else
            //{
            //    previewFacilityObject.transform.position = HidePosition;
            //}

            //タッチ数をカウント
            if(Input.touchCount >= 1)
            {
                Touch t1 = Input.GetTouch(0);
                Vector2 nowPointingPosition = myBoard.WorldPosToMapPos(Camera.main.ScreenToWorldPoint(t1.position));
                Vector2Int nowPointingLocation = Vector2Int.Sishagonyu(nowPointingPosition);

                if (t1.position.y >= Screen.width / 2 && myBoard.CanIPutFacility(SelectedFacility, nowPointingLocation))
                {
                    previewFacilityObject.transform.position = myBoard.CalcFacilityWorldPos(SelectedFacility, nowPointingLocation);
                    previewFacilityRenderer.color = previewFacilityColor;

                    //タッチが離されたら施設配置
                    if (t1.phase == TouchPhase.Ended)
                    {
                        PutFacility(SelectedFacility, nowPointingLocation);
                    }
                }
                else
                {
                    previewFacilityObject.transform.position = myBoard.CalcFacilityWorldPos(SelectedFacility, nowPointingPosition);
                    previewFacilityRenderer.color = previewFacilityDisableColor;
                }
            }
            else
            {
                //非タッチ時には非表示
                previewFacilityObject.transform.position = HidePosition;
            }
        }
    }

    /// <summary>
    /// 施設設置関数
    /// </summary>
    /// <param name="facilityPrefab">施設のPrefab</param>
    /// <param name="location">設置する位置（Vector2Int）</param>
    /// <returns>設置成功判定（trueで成功）</returns>
    public bool PutFacility(Facility facilityPrefab, Vector2Int location)
    {
        if (myBoard.CanIPutFacility(facilityPrefab, location))
        {
            //施設設置成功
            Facility newFacility = GameObject.Instantiate(
                facilityPrefab.gameObject,     //Prefabを複製
                myBoard.CalcFacilityWorldPos(facilityPrefab, location),    //位置はlocationをWorld変換したもの
                Quaternion.identity,    //回転はなし
                this.transform  //親はこのGameObject（FieldBoard）
                ).GetComponent<Facility>();

            //Facilityのサイズ範囲内の全てのマスにDictionary情報を付与
            for (var y = 0; y < newFacility.Size.y; y++)
            {
                for (var x = 0; x < newFacility.Size.x; x++)
                {
                    myBoard.Facilities.Add(location + new Vector2Int(x, y), newFacility);
                }
            }

            //Facility側に自己の座標を通知
            newFacility.Position = location;

            //金銭処理
            myBoard.Money -= facilityPrefab.Cost;

            return true;
        }
        else
        {
            //既に施設が存在するため施設設置失敗
            return false;
        }
    }
}
