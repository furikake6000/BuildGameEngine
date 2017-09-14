using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour {

    [SerializeField]
    GameObject creaturePrefab;  //クリーチャーのプレハブ
    [SerializeField]
    VirtualClock creationDate;  //生成日時

    private Creature creature;  //クリーチャー

    private FieldBoard board;
    private Facility facility;

	// Use this for initialization
	void Start () {
        board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();
        facility = GetComponent<Facility>();

        creature = null;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (FieldTimeManager.FieldTime >= creationDate && creature == null)
        {
            //時間を過ぎていたらクリーチャー生成
            creature = GameObject.Instantiate(creaturePrefab, 
                board.MapPosToWorldPos(facility.Position) + Vector3.back * 0.01001f, 
                Quaternion.identity).GetComponent<Creature>();

            //メッセージも流しとけ
            MessageManager.PutMessage(creature.CreatureName + " が新しく配置されました！", MessageManager.MessagePriority.High);
        }
	}
}
