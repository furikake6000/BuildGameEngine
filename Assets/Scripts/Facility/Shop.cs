using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    [SerializeField]
    private float buyRate = 0.01f;  //1分ごとに買ってくれる率

    private Facility myFacility;
    private static FieldBoard board;

    // Use this for initialization
    void Start () {
        myFacility = GetComponent<Facility>();
        if (board == null) board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();
    }
	
	// Update is called once per frame
	void Update () {
        //時間経過取得
        int deltaMinute;  //Update内で何分が経過したか
        deltaMinute = FieldTimeManager.FieldTimeNow.minute - FieldTimeManager.FieldTimePast.minute;
        if (deltaMinute < 0) deltaMinute += 60;

        foreach (var visitor in board.Visitors)
        {
            //観客に対し判断
            //自分の前2マスに来ているか否か
            if(Vector2Int.Sishagonyu(visitor.Position) == myFacility.Position + new Vector2Int(0, 1)
                || Vector2Int.Sishagonyu(visitor.Position) == myFacility.Position + new Vector2Int(1, 1))
            {
                //確率で利益を取得
                if(Random.value < buyRate * deltaMinute)
                {
                    board.Money += 100;
                }
            }
        }
	}
}
