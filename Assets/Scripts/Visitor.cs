using System.Collections.Generic;
using UnityEngine;
using FrikLib;

public class Visitor : Character {

    private Vector2Int startPos;    //初期段階での位置を保存する

    private Stack<Vector2Int> visitPoints = new Stack<Vector2Int>();

    private List<Alien> unVisitedAliens = new List<Alien>();

    // Use this for initialization
    protected override void Start () {
        //必ずCharacterのStart関数を最初に実行する
        base.Start();
        
        //始点記憶
        startPos = Vector2Int.Sishagonyu(Position);

        //ルート策定（全ての動物をランダムな順で回る）
        unVisitedAliens = new List<Alien>(board.Aliens);
        unVisitedAliens.Shuffle();
        foreach (var alien in unVisitedAliens) AddCheckpoint(alien.MyFence.MyFacility.Position);

        //初期位置に戻る
        AddCheckpoint(startPos);

        //ルート再計算
        RecalcRoute();
    }
	
	// Update is called once per frame
	protected override void Update () {
        //必ずCharacterのUpdate関数を最初に実行する
        base.Update();

        if (HasReachedGoal())
        {
            //最終ゴール（入り口）に戻ったなら自己を破棄
            board.Visitors.Remove(this);
            Destroy(gameObject);
        }
	}
}
