using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class FieldBoard : MonoBehaviour {

    #region Inspector表示変数
    [SerializeField]
    private int xTileSize, yTileSize;
    [SerializeField]
    private float widthOfTile, heightOfTile;
    [SerializeField]
    private float edgeWeight;
    [SerializeField]
    private Material fieldMaterial, gridMaterial;
    #endregion

    #region インスタンス変数
    private GameObject fieldGrid;   //グリッド描画用オブジェクト、このオブジェクトの子
    #endregion

    #region Unity専用関数
    // Use this for initialization
    void Start () {

        //フィールドのメッシュ作成
        var fieldMesh = new Mesh();
        var fieldMeshFilter = GetComponent<MeshFilter>();
        var fieldMeshRenderer = GetComponent<MeshRenderer>();
        //頂点配列を作成（ xマスの数+1 * yマスの数+1 個）
        var points = new List<Vector3>();
        for(var x = 0; x < xTileSize + 1; x++)
        {
            for(var y = 0; y < yTileSize + 1; y++)
            {
                //各タイルの頂点をインプット（中心点ではなく頂点なので0.5fずらしている）
                points.Add(mapPosToWorldPos(new Vector2((float)x - 0.5f, (float)y - 0.5f)));
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
        //子オブジェクトとしてフィールドグリッドオブジェクト作成
        fieldGrid = new GameObject("FieldGrid");
        fieldGrid.transform.parent = this.transform;
        var gridMeshRenderer = fieldGrid.AddComponent<MeshRenderer>();
        var gridMeshFilter = fieldGrid.AddComponent<MeshFilter>();
        //頂点配列の作成（グリッドなので隅の点だけでよい）
        var gridPoints = new List<Vector3>();
        for (var x = 0; x < xTileSize + 1; x++)
        {
            //上端2点
            gridPoints.Add(mapPosToWorldPos(new Vector2((float)x - 0.5f - edgeWeight / 2, -0.5f - edgeWeight / 2)));
            gridPoints.Add(mapPosToWorldPos(new Vector2((float)x - 0.5f + edgeWeight / 2, -0.5f - edgeWeight / 2)));

            //下端2点
            gridPoints.Add(mapPosToWorldPos(new Vector2((float)x - 0.5f - edgeWeight / 2, (float)yTileSize - 0.5f + edgeWeight / 2)));
            gridPoints.Add(mapPosToWorldPos(new Vector2((float)x - 0.5f + edgeWeight / 2, (float)yTileSize - 0.5f + edgeWeight / 2)));
        }
        for (var y = 0; y < yTileSize + 1; y++)
        {
            //左端2点
            gridPoints.Add(mapPosToWorldPos(new Vector2(-0.5f - edgeWeight / 2, (float)y - 0.5f - edgeWeight / 2)));
            gridPoints.Add(mapPosToWorldPos(new Vector2(-0.5f - edgeWeight / 2, (float)y - 0.5f + edgeWeight / 2)));

            //右端2点
            gridPoints.Add(mapPosToWorldPos(new Vector2((float)xTileSize - 0.5f - edgeWeight / 2, (float)y - 0.5f - edgeWeight / 2)));
            gridPoints.Add(mapPosToWorldPos(new Vector2((float)xTileSize - 0.5f - edgeWeight / 2, (float)y - 0.5f + edgeWeight / 2)));
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

    // Update is called once per frame
    void Update () {
		
	}
    #endregion

    #region 自作メソッド
    /// <summary>
    /// マップにおける座標からワールド座標への変換
    /// </summary>
    /// <param name="mapPos">マップ上の座標(マスの中心座標)</param>
    /// <returns>ワールド座標</returns>
    private Vector3 mapPosToWorldPos(Vector2 mapPos)
    {
        //マップの中心座標との相対位置を求める
        Vector2 mapPosRelativeFromCenter = new Vector2(mapPos.x - (float)(xTileSize - 1) / 2, mapPos.y - (float)(yTileSize - 1) / 2);
        
        //WorldPos.x = ( mapPosRFC.x - mapPosRFC.y ) / -2f
        //WorldPos.y = ( mapPosRFC.x + mapPosRFC.y ) / -2f
        //WorldPos.z = 0f
        Vector3 worldPos = new Vector3(
            (mapPosRelativeFromCenter.x - mapPosRelativeFromCenter.y) / -2f * widthOfTile,
            (mapPosRelativeFromCenter.x + mapPosRelativeFromCenter.y) / -2f * heightOfTile,
            0f);

        return worldPos;
    }
    #endregion
}
