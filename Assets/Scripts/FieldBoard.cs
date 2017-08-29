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
    private Color backColor, edgeColor;
    #endregion

    #region Unity専用関数
    // Use this for initialization
    void Start () {
        var fieldMesh = new Mesh();

        //頂点配列を作成（ xマスの数+1 * yマスの数+1 個）
        List<Vector3> points = new List<Vector3>();
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
        List<int> triangles = new List<int>();
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

        //Filterの作成
        var fieldMeshFilter = GetComponent<MeshFilter>();
        fieldMeshFilter.sharedMesh = fieldMesh;


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
