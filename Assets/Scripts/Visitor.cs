using System.Collections.Generic;
using UnityEngine;

public class Visitor : Character {

    private Vector2Int startPos;    //初期段階での位置を保存する

    private Stack<Vector2Int> visitPoints = new Stack<Vector2Int>();

    private Vector2 nextPoint;

    private List<Alien> unVisitedAliens = new List<Alien>();

    public void ResetPos(Vector2Int location)
    {
        startPos = location;
        Position = startPos;
        nextPoint = location;
    }

    // Use this for initialization
    void Start () {
        if(board == null)board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();

        //ルート策定（全ての動物をランダムな順で回る）
        unVisitedAliens = new List<Alien>(board.Aliens);
        unVisitedAliens.Shuffle();
        foreach (var alien in unVisitedAliens) AddCheckpoint(alien.MyFence.MyFacility.Position);

        //初期位置に戻る
        AddCheckpoint(startPos);
    }
	
	// Update is called once per frame
	void Update () {
        if (HasReachedGoal())
        {
            //最終ゴール（入り口）に戻ったなら自己を破棄
            board.Visitors.Remove(this);
            Destroy(gameObject);
        }
	}
}
