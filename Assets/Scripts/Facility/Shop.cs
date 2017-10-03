using UnityEngine;
using FrikLib;

public class Shop : FacilityBehaviour {

    [SerializeField]
    private float buyRate = 0.01f;  //1分ごとに買ってくれる率
    
	// Update is called once per frame
	protected override void Update () {
        //必ずFacilityBehaviourのUpdate関数を最初に実行する
        base.Update();

        foreach (var visitor in board.Visitors)
        {
            //観客に対し判断
            //自分の前2マスに来ているか否か
            if(Vector2Int.Sishagonyu(visitor.Position) == MyFacility.Position + new Vector2Int(0, 1)
                || Vector2Int.Sishagonyu(visitor.Position) == MyFacility.Position + new Vector2Int(1, 1))
            {
                //確率で利益を取得
                if(Random.value < buyRate * FieldTimeManager.DeltaMinute)
                {
                    board.Money += 100;
                }
            }
        }
	}
}
