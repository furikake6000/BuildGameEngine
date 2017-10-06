using UnityEngine;
using FrikLib;
using UnityEngine.EventSystems;
using System;

public class FieldBoardBuilder : MonoBehaviour
{

    //オブジェクトを視認外に移動させたい時の座標（定数）
    private static readonly Vector3 HidePosition = new Vector3(-999f, -999f, -999f);

    private static readonly Color previewFacilityColor = new Color(1f, 1f, 1f, 0.5f); //プレビュオブジェクトの色
    private static readonly Color previewFacilityDisableColor = new Color(1f, 0.5f, 0.5f, 0.5f); //設置不可時色

    private FieldBoard board; //自分のFieldBoardコンポーネント
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
                //前面に
                previewFacilityRenderer.sortingOrder = 101;

                //プレビュオブジェクトのFacilityBehaviourを全て停止
                foreach (var facilityBehaviour in previewFacilityObject.GetComponents<FacilityBehaviour>())
                {
                    facilityBehaviour.enabled = false;
                }
            }
        }
    }

    private GameObject previewFacilityObject; //プレビューのファシリティ
    private SpriteRenderer previewFacilityRenderer;  //プレビューファシリティのレンダラ

    // Use this for initialization
    void Start()
    {
        //ロード系
        board = this.GetComponent<FieldBoard>();

        //無を選択
        SelectedFacility = null;
    }

    // Update is called once per frame
    void Update()
    {
        //なんらかのFacilityを選択していれば
        if (SelectedFacility != null)
        {

            //触れている座標を取得（タッチされているか否かで場合分け）
            Vector3 pointerPosition;
            if (Input.touchCount > 0)
            {
                //タッチパネル使用
                pointerPosition = Input.GetTouch(Input.touchCount - 1).position;
            }
            else
            {
                //マウス使用
                pointerPosition = Input.mousePosition;
            }

            //現在指している場所を取得
            Vector2 nowPointingPosition = board.WorldPosToMapPos(Camera.main.ScreenToWorldPoint(pointerPosition));
            Vector2Int nowPointingLocation = Vector2Int.Sishagonyu(nowPointingPosition);

            if (Input.mousePosition.y >= Screen.width / 2 && board.CanIPutFacility(SelectedFacility, nowPointingLocation))
            {
                //設置可能域にあるのならば設置されるマスに半透明のサンプルを配置
                previewFacilityObject.transform.position = board.CalcFacilityWorldPos(SelectedFacility, nowPointingLocation);
                previewFacilityRenderer.color = previewFacilityColor;
            }
            else
            {
                //配置不可域にあるのならばDisableColorでサンプルを配置
                previewFacilityObject.transform.position = board.CalcFacilityWorldPos(SelectedFacility, nowPointingPosition);
                previewFacilityRenderer.color = previewFacilityDisableColor;
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
        if (board.CanIPutFacility(facilityPrefab, location))
        {
            //施設設置成功
            Facility newFacility = GameObject.Instantiate(
                facilityPrefab.gameObject,     //Prefabを複製
                board.CalcFacilityWorldPos(facilityPrefab, location),    //位置はlocationをWorld変換したもの
                Quaternion.identity,    //回転はなし
                this.transform  //親はこのGameObject（FieldBoard）
                ).GetComponent<Facility>();

            //Facilityのサイズ範囲内の全てのマスにDictionary情報を付与
            for (var y = 0; y < newFacility.Size.y; y++)
            {
                for (var x = 0; x < newFacility.Size.x; x++)
                {
                    board.Facilities.Add(location + new Vector2Int(x, y), newFacility);
                }
            }

            //Facility側に自己の座標を通知
            newFacility.Position = location;

            //金銭処理
            board.Money -= facilityPrefab.Cost;

            return true;
        }
        else
        {
            //既に施設が存在するため施設設置失敗
            return false;
        }
    }
}
