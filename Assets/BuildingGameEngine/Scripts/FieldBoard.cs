using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FrikLib;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class FieldBoard : MonoBehaviour {

    #region Inspector表示変数
    [SerializeField]
    private int mapWidth, mapHeight;
    public int MapWidth
    {
        get
        {
            return mapWidth;
        }

        set
        {
            mapWidth = value;
            RefreshMeshes();
        }
    }
    public int MapHeight
    {
        get
        {
            return mapHeight;
        }

        set
        {
            mapHeight = value;
            RefreshMeshes();
        }
    }
    [SerializeField]
    private float sizeOfTile, aspectRatioOfTile;
    [SerializeField]
    private float edgeWeight;
    [SerializeField]
    private Material fieldMaterial, gridMaterial;

    [Header("Money data")]
    [SerializeField]
    private int defaultMoney;   //初期資金

    [Header("UI data")]
    [SerializeField]
    private Text moneyLabel;
    [SerializeField]
    private Text statisticLabel;    //各情報表示窓

    [Header("Debug data")]
    [SerializeField]
    private Text debugText;
    #endregion

    #region インスタンス変数
    //ゲーム進行データ群
    private Dictionary<Vector2Int, Facility> facilities;    //施設データ
    public Dictionary<Vector2Int, Facility> Facilities
    {
        get
        {
            return facilities;
        }
    }   //get only

    private int money;  //資金
    public int Money
    {
        get
        {
            return money;
        }

        set
        {
            money = value;
        }
    }

    private List<Alien> aliens;   //クリーチャー
    public List<Alien> Aliens
    {
        get
        {
            return aliens;
        }

        set
        {
            aliens = value;
        }
    }

    public List<Visitor> Visitors
    {
        get
        {
            return visitors;
        }

        set
        {
            visitors = value;
        }
    }

    private List<Visitor> visitors; //観客


    //エンジン基盤部
    private MeshFilter fieldMeshFilter; //メッシュフィルタ保存用
    private MeshRenderer fieldMeshRenderer; //メッシュレンダラ保存用
    private GameObject fieldGrid;   //グリッド描画用オブジェクト、このオブジェクトの子
    private MeshFilter gridMeshFilter; //グリッド用メッシュフィルタ保存用
    private MeshRenderer gridMeshRenderer; //グリッド用メッシュレンダラ保存用

    private FieldTimeManager timeManager;  //タイムマネジャ保存用

    private float heightOfTile; //WidthとAspectから算出したマス高さ
    
    #endregion

    #region Unity専用関数
    // 初期化関数
    private void Awake()
    {
        //各種一覧データリセット
        facilities = new Dictionary<Vector2Int, Facility>();
        aliens = new List<Alien>();
        visitors = new List<Visitor>();

        //マス高さ算出
        heightOfTile = sizeOfTile * aspectRatioOfTile;

        //資金設定
        Money = defaultMoney;
    }
    // 初期化関数（グラフィック系、GetComponent系）
    void Start () {
        //GetComponent系
        fieldMeshFilter = GetComponent<MeshFilter>();
        fieldMeshRenderer = GetComponent<MeshRenderer>();
        timeManager = GetComponent<FieldTimeManager>();
        //子オブジェクトとしてフィールドグリッドオブジェクト作成
        fieldGrid = new GameObject("FieldGrid");
        fieldGrid.transform.parent = this.transform;
        //ちょっとだけ前に表示
        fieldGrid.transform.position = this.transform.position + Vector3.back * 0.01f;
        gridMeshRenderer = fieldGrid.AddComponent<MeshRenderer>();
        gridMeshFilter = fieldGrid.AddComponent<MeshFilter>();

        //メッシュのリフレッシュ
        RefreshMeshes();
        
    }

    // Update is called once per frame
    void Update () {

        //資金表示
        if(moneyLabel != null)moneyLabel.text = Money + "円";
        //来客者数表示
        if (statisticLabel != null) statisticLabel.text = Visitors.Count + "人";
        timeManager.RefreshClockView();
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
        Vector2 mapPosRelativeFromCenter = new Vector2(mapPos.x - (float)(MapWidth - 1) / 2, mapPos.y - (float)(MapHeight - 1) / 2);

        //WorldPos.x = ( mapPosRFC.x - mapPosRFC.y ) / -2f
        //WorldPos.y = ( mapPosRFC.x + mapPosRFC.y ) / -2f
        //WorldPos.z = ( mapPosRFC.x + mapPosRFC.y ) / -100f(1段下に降りるごとに0.01fだけ変化)
        Vector3 worldPos = new Vector3(
            (mapPosRelativeFromCenter.x - mapPosRelativeFromCenter.y) / -2f * sizeOfTile,
            (mapPosRelativeFromCenter.x + mapPosRelativeFromCenter.y) / -2f * heightOfTile,
            (mapPosRelativeFromCenter.x + mapPosRelativeFromCenter.y) / -100f);

        return worldPos;
    }

    public Vector2 WorldPosToMapPos(Vector3 worldPos)
    {
        //マップのゼロ点からの位置ベクトルを求める
        Vector3 worldPosVecFromZero = worldPos - MapPosToWorldPos(Vector2.zero);

        //X、Yそれぞれの軸においてworldPosVecFromZeroの要素が何マス分にあたるか計算
        float xFromZero = worldPosVecFromZero.x / sizeOfTile;
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
        for (var y = 0; y < MapHeight + 1; y++)
        {
            for (var x = 0; x < MapWidth + 1; x++)
            {
                //各タイルの頂点をインプット（中心点ではなく頂点なので0.5fずらしている）
                points.Add(MapPosToWorldPos(new Vector2((float)x - 0.5f, (float)y - 0.5f)));
            }
        }
        fieldMesh.vertices = points.ToArray();
        //四角形メッシュ配列を作成（三角形が xマスの数 * yマスの数 個）
        var triangles = new List<int>();
        for (var x = 0; x < MapWidth; x++)
        {
            for (var y = 0; y < MapHeight; y++)
            {
                //三角形を張る（表から見て時計回りの順番に！）
                //上半分の三角形
                triangles.Add(x + y * (MapWidth + 1));
                triangles.Add((x + 1) + y * (MapWidth + 1));
                triangles.Add(x + (y + 1) * (MapWidth + 1));
                //下半分の三角形
                triangles.Add((x + 1) + y * (MapWidth + 1));
                triangles.Add((x + 1) + (y + 1) * (MapWidth + 1));
                triangles.Add(x + (y + 1) * (MapWidth + 1));
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
        for (var x = 0; x < MapWidth + 1; x++)
        {
            //上端2点
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)x - 0.5f - edgeWeight / 2, -0.5f - edgeWeight / 2)));
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)x - 0.5f + edgeWeight / 2, -0.5f - edgeWeight / 2)));

            //下端2点
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)x - 0.5f - edgeWeight / 2, (float)MapHeight - 0.5f + edgeWeight / 2)));
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)x - 0.5f + edgeWeight / 2, (float)MapHeight - 0.5f + edgeWeight / 2)));
        }
        for (var y = 0; y < MapHeight + 1; y++)
        {
            //左端2点
            gridPoints.Add(MapPosToWorldPos(new Vector2(-0.5f - edgeWeight / 2, (float)y - 0.5f - edgeWeight / 2)));
            gridPoints.Add(MapPosToWorldPos(new Vector2(-0.5f - edgeWeight / 2, (float)y - 0.5f + edgeWeight / 2)));

            //右端2点
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)MapWidth - 0.5f - edgeWeight / 2, (float)y - 0.5f - edgeWeight / 2)));
            gridPoints.Add(MapPosToWorldPos(new Vector2((float)MapWidth - 0.5f - edgeWeight / 2, (float)y - 0.5f + edgeWeight / 2)));
        }
        gridMesh.vertices = gridPoints.ToArray();
        //四角形メッシュ配列を作成
        var gridTriangles = new List<int>();
        for (var x = 0; x < MapWidth + 1; x++)
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
        for (var y = 0; y < MapHeight + 1; y++)
        {
            //三角形ひとつめ
            gridTriangles.Add((MapWidth + 1) * 4 + y * 4);
            gridTriangles.Add((MapWidth + 1) * 4 + y * 4 + 2);
            gridTriangles.Add((MapWidth + 1) * 4 + y * 4 + 1);
            //三角形ふたつめ
            gridTriangles.Add((MapWidth + 1) * 4 + y * 4 + 1);
            gridTriangles.Add((MapWidth + 1) * 4 + y * 4 + 2);
            gridTriangles.Add((MapWidth + 1) * 4 + y * 4 + 3);
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
            location.x + facilityPrefab.Size.x > MapWidth || 
            location.y + facilityPrefab.Size.y > MapHeight ||
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
                if (Facilities.ContainsKey(location + new Vector2Int(x, y)))
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
    public Vector3 CalcFacilityWorldPos(Facility facilityPrefab, Vector2 location)
    {
        //設置座標の計算(facility自体のサイズも考慮)
        Vector3 newFacilityPos = MapPosToWorldPos(location + (Vector2)(facilityPrefab.Size - new Vector2Int(1, 1)) / 2);
        //縦向きのズレを算出
        SpriteRenderer facilityPrefabRenderer = facilityPrefab.gameObject.GetComponent<SpriteRenderer>();
        float yGap = (facilityPrefabRenderer.bounds.size.y - facilityPrefabRenderer.bounds.size.x * aspectRatioOfTile) / 2 * sizeOfTile;
        //ズレ適用
        newFacilityPos.y += yGap;

        //Facilityサイズによって微妙にZ座標を調整する
        if(facilityPrefab.Size.x > facilityPrefab.Size.y)
        {
            newFacilityPos.z -= location.y * 0.0001f;
        }
        else if(facilityPrefab.Size.y > facilityPrefab.Size.x)
        {
            newFacilityPos.z -= location.x * 0.0001f;
        }

        return newFacilityPos;
    }
    #endregion
}

class RouteConnector
{
    public int minDist;
    public Vector2Int pastPoint;

    public RouteConnector(int minDist, Vector2 pastPoint)
    {
        this.minDist = minDist;
        this.pastPoint = pastPoint;
    }
}