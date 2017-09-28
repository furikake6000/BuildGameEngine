﻿using System.Collections.Generic;
using UnityEngine;

public class Visitor : Character {

    private Vector2Int startPos;    //初期段階での位置を保存する

    private Stack<Vector2Int> visitPoints = new Stack<Vector2Int>();

    private Vector2 nextPoint;

    private List<Alien> unVisitedCreatures = new List<Alien>();

    private bool isReturningHome;   //帰宅中か否か

    public void ResetPos(Vector2Int location)
    {
        startPos = location;
        Position = startPos;
        nextPoint = location;
    }

    // Use this for initialization
    void Start () {
        if(board == null)board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();

        unVisitedCreatures = new List<Alien>(board.Aliens);
        ////シャッフル
        //unVisitedCreatures = (List<Creature>)unVisitedCreatures.OrderBy(i => Guid.NewGuid());
        
        board.Visitors.Add(this);

        ResetGoal();
    }
	
	// Update is called once per frame
	void Update () {
        //移動
        float remainSpeed = speed * FieldTimeManager.DeltaSecond / 60.0f;
        while(true)
        {
            float distToNext = Vector2.Distance(Position, nextPoint);
            if (remainSpeed < distToNext)
            {
                //次の点に着くまでにスピード使い切る
                position += (nextPoint - Position) / distToNext * remainSpeed;
                break;
            }
            else
            {
                //次の点に着くまでにスピード使い切らない
                remainSpeed -= distToNext;
                position = nextPoint;
                
                if(visitPoints.Count != 0)
                {
                    //次の点を決定(VisitPointsから取り、0.5f四方の誤差を追加)
                    nextPoint = (Vector2)visitPoints.Pop() + new Vector2(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f);
                }
                else
                {
                    //帰宅中なら自己を破棄
                    if (isReturningHome)
                    {
                        board.Visitors.Remove(this);
                        Destroy(gameObject);
                    }
                    //新しい目的地を確定
                    ResetGoal();
                    break;
                }
            }
        }

        //Positionを実座標に反映
        transform.position = board.MapPosToWorldPos(Position);
	}

    void ResetGoal()
    {
        if(FieldTimeManager.FieldTime.hour >= 17 || unVisitedCreatures.Count == 0)
        {
            //17時以降か全ての動物を訪れたなら帰宅
            visitPoints = board.SearchRoute(Vector2Int.Sishagonyu(Position), startPos);
            isReturningHome = true;
        }
        else
        {
            //任意の動物を訪れる
            visitPoints = board.SearchRoute(Vector2Int.Sishagonyu(Position), unVisitedCreatures[0].Position);
            unVisitedCreatures.RemoveAt(0);
        }
        if(visitPoints.Count != 0)nextPoint = (Vector2)visitPoints.Pop() + new Vector2(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f);
    }

}
