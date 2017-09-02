using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class FieldBoard : MonoBehaviour {

    #region Inspector表示変数
    [SerializeField]
    private int xTileSize, yTileSize;
    [SerializeField]
    private float widthOfTile, aspectRatioOfTile;
    [SerializeField]
    private float edgeWeight;
    [SerializeField]
    private Material fieldMaterial, gridMaterial;

    [SerializeField]
    private float fieldTimeUpdateFreq;  //フィールド時間を更新する頻度(秒)

    [SerializeField]
    private GameObject debugFacility;
    public Text debugText;
    #endregion

    #region インスタンス変数
    //ゲーム進行データ群
    private Dictionary<Vector2Int, Facility> facilities;    //施設データ
    private DateTime fieldTime; //フィールド上の時間データ
    private TimeSpan fieldTimeUpdateAddSpan;  //フィールド時間を更新するたびに加算される時間(Serialize不可)
    private bool fieldTimeEnabled;  //フィールド時間が動いているか止まっているか

    //エンジン基盤部
    private MeshFilter fieldMeshFilter; //メッシュフィルタ保存用
    private MeshRenderer fieldMeshRenderer; //メッシュレンダラ保存用
    private GameObject fieldGrid;   //グリッド描画用オブジェクト、このオブジェクトの子
    private MeshFilter gridMeshFilter; //グリッド用メッシュフィルタ保存用
    private MeshRenderer gridMeshRenderer; //グリッド用メッシュレンダラ保存用

    private float heightOfTile; //WidthとAspectから算出したマス高さ
    #endregion

    #region Unity専用関数
    // 初期化関数
    private void Awake()
    {
        //施設データリセット
        facilities = new Dictionary<Vector2Int, Facility>();
        //時間は2000年1月1日にリセット
        fieldTime = new DateTime(2000, 1, 1);

        //TimeSpanを初期化
        fieldTimeUpdateAddSpan = new TimeSpan(1, 0, 0);
        //時間を有効化
        fieldTimeEnabled = true;

        //マス高さ算出
        heightOfTile = widthOfTile * aspectRatioOfTile;
    }
    // 初期化関数（グラフィック系、GetComponent系）
    void Start () {
        //GetComponent系
        fieldMeshFilter = GetComponent<MeshFilter>();
        fieldMeshRenderer = GetComponent<MeshRenderer>();
        //子オブジェクトとしてフィールドグリッドオブジェクト作成
        fieldGrid = new GameObject("FieldGrid");
        fieldGrid.transform.parent = this.transform;
        //ちょっとだけ前に表示
        fieldGrid.transform.position = this.transform.position + Vector3.back * 0.01f;
        gridMeshRenderer = fieldGrid.AddComponent<MeshRenderer>();
        gridMeshFilter = fieldGrid.AddComponent<MeshFilter>();

        //メッシュのリフレッシュ
        RefreshMeshes();

        //※デバッグ用
        PutFacility(debugFacility.GetComponent<Facility>(), new Vector2Int(0, 0));
        PutFacility(debugFacility.GetComponent<Facility>(), new Vector2Int(3, 3));
        PutFacility(debugFacility.GetComponent<Facility>(), new Vector2Int(5, 8));

        //Menuシーンの読み込み
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update () {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosInField = WorldPosToMapPos(mousePos);
        debugText.text =
            "Mouse Pos: " + Input.mousePosition.ToString() + "\n" +
            "Mouse Pos(World): " + mousePos.ToString() + "\n" +
            "Mouse Pos(Field): " + mousePosInField.ToString() + "\n" + 
            "Mouse Pos(Field_Int): " + ((Vector2Int)mousePosInField).ToString() ;
	}
    #endregion

    #region 独自時間コルーチン
    IEnumerator FieldTimeUpdate()
    {
        //ループ
        while (true)
        {
            //時間の加算
            fieldTime.Add(fieldTimeUpdateAddSpan);

            //毎年行われる処理

            //毎月行われる処理

            //毎日行われる処理

            //毎時行われる処理

            //毎分行われる処理

            //毎秒行われる処理

            //Enabledの判定
            while (!fieldTimeEnabled)
            {
                //fieldTimeEnabledがfalseなら永遠にここを回り続ける
                yield return null;
            }

            yield return new WaitForSeconds(fieldTimeUpdateFreq);
        }
    }
    #endregion

    #region 自作メソッド
    /// <summary>
    /// マップにおける座標からワールド座標への変換
    /// </summary>
    /// <param name="mapPos">マップ上の座標(マスの中心座標)</param>
    /// <returns>ワールド座標</returns>
    public Vector3 MapPosToWorldPos(Vector2 mapPos)
    {
        //マップの中心座標との相対位置を求める
        Vector2 mapPosRelativeFromCenter = new Vector2(mapPos.x - (float)(xTileSize - 1) / 2, mapPos.y - (float)(yTileSize - 1) / 2);

        //WorldPos.x = ( mapPosRFC.x - mapPosRFC.y ) / -2f
        //WorldPos.y = ( mapPosRFC.x + mapPosRFC.y ) / -2f
        //WorldPos.z = ( mapPosRFC.x + mapPosRFC.y ) / -200f(yに比例)
        Vector3 worldPos = new Vector3(
            (mapPosRelativeFromCenter.x - mapPosRelativeFromCenter.y) / -2f * widthOfTile,
            (mapPosRelativeFromCenter.x + mapPosRelativeFromCenter.y) / -2f * heightOfTile,
            (mapPosRelativeFromCenter.x + mapPosRelativeFromCenter.y) / -200f);

        return worldPos;
    }

    public Vector2 WorldPosToMapPos(Vector3 worldPos)
    {
        //マップのゼロ点からの位置ベクトルを求める
        Vector3 worldPosVecFromZero = worldPos - MapPosToWorldPos(Vector2.zero);

        //X、Yそれぞれの軸においてworldPosVecFromZeroの要素が何マス分にあたるか計算
        float xFromZero = worldPosVecFromZero.x / widthOfTile;
        float yFromZero = worldPosVecFromZero.y / heightOfTile;

        //二次元ベクトルに変換
        Vector2 mapPos = new Vector2(-xFromZero - yFromZero, xFromZero - yFromZero);

        return mapPos;
    }

    /// <summary>
    /// フィールド・グリッドのメッシュ作成
    /// </summary>
    private void RefreshMeshes()
    {
        //フィールドのメッシュ作成
        var fieldMesh = new Mesh();
        //頂点配列を作成（ xマスの数+1 * yマスの数+1 個）
        var points = new List<Vector3>();
        for (var y = 0; y < yTileSize + 1; y++)
        {
            for (var x = 0; x < xTileSize + 1; x++)
            {
                //各タイルの頂点をインプット（中心点ではなく頂点なので0.5fずらしている）
                points.Add(MapPosToWorldPos(new Vector2((float)x - 0.5f, (float)y - 0.5f)));
            }
        }
        fieldMesh.vertices = points.ToArray();
        //四角形メッシュ配列を作成（三角形が xマスの数 * yマスの数 個）
        var triangles = new List<int>();
        for (var x = 0; x < xTileSize; x++)
        {
            for (var y = 0; y < yTileSize; y++)
            {
                //三角形を張る（表から見て時計回りの順番に！）
                //上半分の三角形
                triangles.Add(x + y * (xTileSize + 1));
                triangles.Add((x + 1) + y * (xTileSize + 1));
                triangles.Add(x + (y + 1) * (xTileSize + 1));
                //下半分の三角形
                triangles.Add((x + 1) + y * (xTileSize + 1));
                triangles.Add((x + 1) + (y + 1) * (xTileSize + 1));
                triangles.Add(x + (y + 1) * (xTileSize + 1));
            }
        }
        fieldMesh.triangles = triangles.ToArray();
        //法線の再計算
        fieldMesh.RecalculateNormals();
        //MeshやMaterialを実際の値に設定
        fieldMeshFilter.sharedMesh = fieldMesh;
        fieldMeshRenderer.material = fieldMaterial;
        //フィールドのメッシュ作成　ここまで

        //フィールドグリッドのメッシュ作成
        var gridMesh = new Mesh();
        //頂点配列の作成（グリッドなので隅の点だけでよい）
        var gridPoints = new List<Vector3>();
        for (var x = 0; x < xTileSize + 1; x++)
        {
            //上端2点
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)x - 0.5f - edgeWeight / 2, -0.5f - edgeWeight / 2)));
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)x - 0.5f + edgeWeight / 2, -0.5f - edgeWeight / 2)));

            //下端2点
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)x - 0.5f - edgeWeight / 2, (float)yTileSize - 0.5f + edgeWeight / 2)));
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)x - 0.5f + edgeWeight / 2, (float)yTileSize - 0.5f + edgeWeight / 2)));
        }
        for (var y = 0; y < yTileSize + 1; y++)
        {
            //左端2点
            gridPoints.Add(MapPosToWorldPos(new Vector2(-0.5f - edgeWeight / 2, (float)y - 0.5f - edgeWeight / 2)));
            gridPoints.Add(MapPosToWorldPos(new Vector2(-0.5f - edgeWeight / 2, (float)y - 0.5f + edgeWeight / 2)));

            //右端2点
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)xTileSize - 0.5f - edgeWeight / 2, (float)y - 0.5f - edgeWeight / 2)));
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)xTileSize - 0.5f - edgeWeight / 2, (float)y - 0.5f + edgeWeight / 2)));
        }
        gridMesh.vertices = gridPoints.ToArray();
        //四角形メッシュ配列を作成
        var gridTriangles = new List<int>();
        for (var x = 0; x < xTileSize + 1; x++)
        {
            //三角形ひとつめ
            gridTriangles.Add(x * 4);
            gridTriangles.Add(x * 4 + 2);
            gridTriangles.Add(x * 4 + 1);
            //三角形ふたつめ
            gridTriangles.Add(x * 4 + 1);
            gridTriangles.Add(x * 4 + 2);
            gridTriangles.Add(x * 4 + 3);
        }
        for (var y = 0; y < yTileSize + 1; y++)
        {
            //三角形ひとつめ
            gridTriangles.Add((xTileSize + 1) * 4 + y * 4);
            gridTriangles.Add((xTileSize + 1) * 4 + y * 4 + 2);
            gridTriangles.Add((xTileSize + 1) * 4 + y * 4 + 1);
            //三角形ふたつめ
            gridTriangles.Add((xTileSize + 1) * 4 + y * 4 + 1);
            gridTriangles.Add((xTileSize + 1) * 4 + y * 4 + 2);
            gridTriangles.Add((xTileSize + 1) * 4 + y * 4 + 3);
        }
        gridMesh.triangles = gridTriangles.ToArray();
        //法線の再計算
        gridMesh.RecalculateNormals();
        //MeshやMaterialを実際の値に設定
        gridMeshFilter.sharedMesh = gridMesh;
        gridMeshRenderer.material = gridMaterial;
        //フィールドグリッドのメッシュ作成　ここまで
    }

    /// <summary>
    /// 施設設置が可能かどうか
    /// </summary>
    /// <param name="facilityPrefab">施設のPrefab</param>
    /// <param name="location">設置する位置（Vector2Int）</param>
    /// <returns></returns>
    public bool CanIPutFacility(Facility facilityPrefab, Vector2Int location)
    {
        //範囲外チェック
        if (
            location.x + facilityPrefab.Size.x > xTileSize || 
            location.y + facilityPrefab.Size.y > yTileSize ||
            location.x < 0 || 
            location.y < 0)
        {
            return false;
        }

        //Facilityのサイズ範囲内の全てのマスを見る
        for (var y = 0; y < facilityPrefab.Size.y; y++)
        {
            for(var x = 0; x < facilityPrefab.Size.x; x++)
            {
                //既存施設チェック
                if (facilities.ContainsKey(location + new Vector2Int(x, y)))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 新しく設置する施設の座標計算
    /// </summary>
    /// <param name="facilityPrefab">施設のPrefab</param>
    /// <param name="location">設置する位置（Vector2Int）</param>
    /// <returns>施設の置くべきWorld座標</returns>
    public Vector3 CalcFacilityWorldPos(Facility facilityPrefab, Vector2Int location)
    {
        //設置座標の計算(facility自体のサイズも考慮)
        Vector3 newFacilityPos = MapPosToWorldPos((Vector2)location + (Vector2)(facilityPrefab.Size - new Vector2Int(1, 1)) / 2);
        //縦向きのズレを算出
        SpriteRenderer facilityPrefabRenderer = facilityPrefab.gameObject.GetComponent<SpriteRenderer>();
        float yGap = (facilityPrefabRenderer.bounds.size.y - facilityPrefabRenderer.bounds.size.x * aspectRatioOfTile) / 2 * widthOfTile;
        //ズレ適用
        newFacilityPos.y += yGap;

        return newFacilityPos;
    }
    /// <summary>
    /// 施設設置関数
    /// </summary>
    /// <param name="facilityPrefab">施設のPrefab</param>
    /// <param name="location">設置する位置（Vector2Int）</param>
    /// <returns>設置成功判定（trueで成功）</returns>
    public bool PutFacility(Facility facilityPrefab, Vector2Int location)
    {
        if (CanIPutFacility(facilityPrefab, location))
        {
            //施設設置成功
            Facility newFacility = GameObject.Instantiate(
                facilityPrefab.gameObject,     //Prefabを複製
                CalcFacilityWorldPos(facilityPrefab, location),    //位置はlocationをWorld変換したもの
                Quaternion.identity,    //回転はなし
                this.transform  //親はこのGameObject（FieldBoard）
                ).GetComponent<Facility>();

            //Facilityのサイズ範囲内の全てのマスにDictionary情報を付与
            for (var y = 0; y < newFacility.Size.y; y++)
            {
                for (var x = 0; x < newFacility.Size.x; x++)
                {
                    facilities.Add(location + new Vector2Int(x, y), newFacility);
                }
            }
            return true;
        }
        else
        {
            //既に施設が存在するため施設設置失敗
            return false;
        }
    }
    #endregion
}
