using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour {

    //オブジェクトを視認外に移動させたい時の座標（定数）
    private static readonly Vector3 HidePosition = new Vector3(-9999f, -9999f, -9999f);

    [SerializeField]
    GameObject visitorPrefab;
    [SerializeField]
    float visitorCreateNumPerHour;  //1hに何人の観客を生成するか
    Facility myFacility;

	// Use this for initialization
	void Start () {
        myFacility = GetComponent<Facility>();
    }

    // Update is called once per frame
    void Update () {

        //6-17時の間ならばVisitor生成
        if (FieldTimeManager.FieldTime.hour >= 6 && FieldTimeManager.FieldTime.hour <= 17)
        {
            if (FieldTimeManager.DeltaMinute * visitorCreateNumPerHour / 60.0f < 1f)
            {
                //確率で生成
                if (Random.value < FieldTimeManager.DeltaMinute * visitorCreateNumPerHour / 60.0f)
                {
                    Visitor newVisitor = GameObject.Instantiate(visitorPrefab, HidePosition, Quaternion.identity).GetComponent<Visitor>();
                    newVisitor.ResetPos(myFacility.Position + new Vector2Int(1, 3));
                }
            }
            else
            {
                //複数個まとめて生成
                for (var i = 0; i < (int)(FieldTimeManager.DeltaMinute * visitorCreateNumPerHour / 60.0f); i++)
                {
                    Visitor newVisitor = GameObject.Instantiate(visitorPrefab, HidePosition, Quaternion.identity).GetComponent<Visitor>();
                    newVisitor.ResetPos(myFacility.Position + new Vector2Int(1, 3));
                }
            }
        }
	}
}
