using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    private Vector2 position;
    public Vector2 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }
    
    [SerializeField]
    private float speed;
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected CharacterFaction faction;
    public enum CharacterFaction
    {
        player,
        enemy
    }

    protected static FieldBoard board;

    private List<Vector2Int> checkPoints;    //目的地一覧
    private List<Vector2Int> route; //これからの経路

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Positionを実座標に反映
        transform.position = board.MapPosToWorldPos(Position) + Vector3.back * 0.01001f;
    }

    #region ルート探索関係基本関数

    /// <summary>
    /// 指定座標が通行可か返す
    /// </summary>
    /// <param name="location">座標</param>
    /// <returns>通過可能か否か</returns>
    public bool CanIGoThrough(Vector2Int location)
    {
        //範囲外チェック
        if (
            location.x > board.MapWidth ||
            location.y > board.MapHeight ||
            location.x < 0 ||
            location.y < 0)
        {
            return false;
        }

        if (board.Facilities.ContainsKey(location))
        {
            //施設があった場合、その施設が通れるかをそのCharacterの属性ごとに判別
            switch (faction)
            {
                case CharacterFaction.player:
                    return board.Facilities[location].PlayerPassable;
                case CharacterFaction.enemy:
                    return board.Facilities[location].EnemyPassable;
                default:
                    //該当するFactionが無い時はデフォルトとしてPlayerPassableを返す
                    return board.Facilities[location].PlayerPassable;
            }
        }
        else
        {
            //施設がなかった場合、通れる
            return true;
        }
    }

    /// <summary>
    /// ルート検索　Goalが通れない設定だった場合その隣接マスまでの経路を返す
    /// </summary>
    /// <param name="start">スタート地点</param>
    /// <param name="goal">ゴール地点</param>
    /// <returns></returns>
    public List<Vector2Int> SearchRoute(Vector2Int start, Vector2Int goal)
    {
        //探索開始
        //ルートコネクタ...そのマスまでの最短距離とそのマスに到達する一つ前のマスのデータのセット
        Dictionary<Vector2Int, RouteConnector> searchData = new Dictionary<Vector2Int, RouteConnector>();
        searchData.Add(start, new RouteConnector(0, start));
        //未探索点データ
        Queue<Vector2Int> searchPoints = new Queue<Vector2Int>();
        searchPoints.Enqueue(start);

        Vector2Int currentPoint;    //現在探索点
        while (searchPoints.Count != 0)
        {
            currentPoint = searchPoints.Dequeue();

            //ゴールだった場合はループ抜ける
            //※横方向探索なので経路が見つかった時点でそれが最短だと分かっている
            if (currentPoint == goal)
            {
                break;
            }

            //四方向に探索を行う
            Vector2Int[] nextPoints = new Vector2Int[] { currentPoint + new Vector2Int(-1, 0)
                                                        ,currentPoint + new Vector2Int(1, 0)
                                                        ,currentPoint + new Vector2Int(0, -1)
                                                        ,currentPoint + new Vector2Int(0, 1)};

            foreach (var nextPoint in nextPoints)
            {
                //通過可能（経路）もしくはその点がゴールの一部（終点）ならば
                if (CanIGoThrough(nextPoint) || currentPoint == goal)
                {
                    if (!searchData.ContainsKey(nextPoint))
                    {
                        //既に到達している痕跡が無かった場合
                        //最短データ新規作成
                        searchData.Add(nextPoint, new RouteConnector((searchData[currentPoint].minDist + 1), currentPoint));

                        //次探索キューに追加
                        searchPoints.Enqueue(nextPoint);
                    }
                    else if (searchData[nextPoint].minDist > searchData[currentPoint].minDist + 1)
                    {
                        //既に到達している上でより良い結果に更新できる場合
                        //最短データ更新
                        searchData[nextPoint].minDist = searchData[currentPoint].minDist + 1;
                        searchData[nextPoint].pastPoint = currentPoint;

                        //次探索キューに追加
                        searchPoints.Enqueue(nextPoint);
                    }
                }
            }

        }

        //探索終了　パスデータ取得
        if (searchData.ContainsKey(goal))
        {
            //該当ゴールまでの道筋を発見
            List<Vector2Int> route = new List<Vector2Int>();
            for (Vector2Int p = goal; p != start; p = searchData[p].pastPoint)
            {
                //元に戻る点を次々に入れていく(ただし、終点であるgoalが進入不可の場合それは入れない)
                if (CanIGoThrough(p)) route.Insert(0, p);
            }
            return route;
        }
        else
        {
            //探索失敗
            return null;
        }
    }

    /// <summary>
    /// ルート検索　Goalが通れない設定だった場合その隣接マスまでの経路を返す
    /// （ゴール複数設定版）
    /// </summary>
    /// <param name="start">スタート地点</param>
    /// <param name="goals">ゴール地点（複数設定）</param>
    /// <returns></returns>
    public List<Vector2Int> SearchRoute(Vector2Int start, List<Vector2Int> goals)
    {
        //探索開始
        //ルートコネクタ...そのマスまでの最短距離とそのマスに到達する一つ前のマスのデータのセット
        Dictionary<Vector2Int, RouteConnector> searchData = new Dictionary<Vector2Int, RouteConnector>();
        searchData.Add(start, new RouteConnector(0, start));
        //未探索点データ
        Queue<Vector2Int> searchPoints = new Queue<Vector2Int>();
        searchPoints.Enqueue(start);

        Vector2Int currentPoint;    //現在探索点
        while (searchPoints.Count != 0)
        {
            currentPoint = searchPoints.Dequeue();

            //ゴールだった場合はループ抜ける
            //※横方向探索なので経路が見つかった時点でそれが最短だと分かっている
            if (goals.Contains(currentPoint))
            {
                break;
            }

            //四方向に探索を行う
            Vector2Int[] nextPoints = new Vector2Int[] { currentPoint + new Vector2Int(-1, 0)
                                                        ,currentPoint + new Vector2Int(1, 0)
                                                        ,currentPoint + new Vector2Int(0, -1)
                                                        ,currentPoint + new Vector2Int(0, 1)};

            foreach (var nextPoint in nextPoints)
            {
                //通過可能（経路）もしくはその点がゴールの一部（終点）ならば
                if (CanIGoThrough(nextPoint) || goals.Contains(currentPoint))
                {
                    if (!searchData.ContainsKey(nextPoint))
                    {
                        //既に到達している痕跡が無かった場合
                        //最短データ新規作成
                        searchData.Add(nextPoint, new RouteConnector((searchData[currentPoint].minDist + 1), currentPoint));

                        //次探索キューに追加
                        searchPoints.Enqueue(nextPoint);
                    }
                    else if (searchData[nextPoint].minDist > searchData[currentPoint].minDist + 1)
                    {
                        //既に到達している上でより良い結果に更新できる場合
                        //最短データ更新
                        searchData[nextPoint].minDist = searchData[currentPoint].minDist + 1;
                        searchData[nextPoint].pastPoint = currentPoint;

                        //次探索キューに追加
                        searchPoints.Enqueue(nextPoint);
                    }
                }
            }

        }

        //探索終了　パスデータ取得
        foreach (var goal in goals)
        {
            if (searchData.ContainsKey(goal))
            {
                //該当ゴールまでの道筋を発見
                List<Vector2Int> route = new List<Vector2Int>();
                for (Vector2Int p = goal; p != start; p = searchData[p].pastPoint)
                {
                    //元に戻る点を次々に入れていく(ただし、終点であるgoalが進入不可の場合それは入れない)
                    if (CanIGoThrough(p)) route.Insert(0, p);
                }
                return route;
            }
        }
        //ここまで来た場合、ゴールが見つからなかった　空を返す
        return null;

    }

    #endregion

    #region 経路探索関係チェックポイント制御系関数

    /// <summary>
    /// 新しく目的地を追加する
    /// </summary>
    /// <param name="point">目的地</param>
    public void AddCheckpoint(Vector2Int point)
    {
        checkPoints.Add(point);

        //routeリストに、routeの最後尾点から新しい目的地への経路配列パーツを追加
        List<Vector2Int> newRoutePart = SearchRoute(route[route.Count - 1], point);
        //新しい経路配列パーツを現在のrouteの末尾に追加
        route.AddRange(newRoutePart);
    }

    /// <summary>
    /// 全てのルートを再計算
    /// (とりあえず困ったらこれ打っとけ(重くなるけど))
    /// </summary>
    private void RecalculateRoute()
    {
        route.Clear();
        //現在地をrouteの終端に追加
        route.Add((Vector2Int)position);
        //チェックポイントに沿って次々経路探索
        foreach(var checkPoint in checkPoints)
        {
            //routeリストに、routeの最後尾点から新しい目的地への経路配列パーツを追加
            List<Vector2Int> newRoutePart = SearchRoute(route[route.Count - 1], checkPoint);
            //新しい経路配列パーツを現在のrouteの末尾に追加
            route.AddRange(newRoutePart);
        }
    }

    #endregion
}
