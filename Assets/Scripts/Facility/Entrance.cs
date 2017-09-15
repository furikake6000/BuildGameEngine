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
        int deltaMinute;  //Update内で何分が経過したか
        deltaMinute = FieldTimeManager.FieldTimeNow.minute - FieldTimeManager.FieldTimePast.minute;
        if (deltaMinute < 0) deltaMinute += 60;

        for(var i=0; i < (int)(deltaMinute * visitorCreateNumPerHour / 60.0f); i++)
        {
            //6-17時の間ならばVisitor生成
            if(FieldTimeManager.FieldTime.hour >= 6 && FieldTimeManager.FieldTime.hour <= 17)
            {
                Visitor newVisitor = GameObject.Instantiate(visitorPrefab, HidePosition, Quaternion.identity).GetComponent<Visitor>();
                newVisitor.ResetPos(myFacility.Position + new Vector2Int(1, 3));
            }
        }
	}
}
