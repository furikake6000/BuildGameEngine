using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : FacilityBehaviour {

    [SerializeField]
    GameObject creaturePrefab;  //クリーチャーのプレハブ
    [SerializeField]
    VirtualClock creationDate;  //生成日時

    private Creature creature;  //クリーチャー

	// Use this for initialization
	protected override void Start () {
        //必ずFacilityBehaviourのStart関数を最初に実行する
        base.Start();
        creature = null;
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        //必ずFacilityBehaviourのUpdate関数を最初に実行する
        base.Update();

        if (FieldTimeManager.FieldTime >= creationDate && creature == null)
        {
            //時間を過ぎていたらクリーチャー生成
            creature = GameObject.Instantiate(creaturePrefab, 
                board.MapPosToWorldPos(myFacility.Position) + Vector3.back * 0.01001f, 
                Quaternion.identity).GetComponent<Creature>();
            creature.ResetPos(myFacility.Position);
            creature.MyFence = this;

            //ボードに登録
            board.Creatures.Add(creature);
            
            //メッセージも流しとけ
            MessageManager.PutMessage(creature.CreatureName + " が新しく配置されました！", MessageManager.MessagePriority.Middle);
        }
	}
}
