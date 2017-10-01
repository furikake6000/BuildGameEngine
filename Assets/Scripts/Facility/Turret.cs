using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : FacilityBehaviour {

    //オブジェクトを視認外に移動させたい時の座標（定数）
    private static readonly Vector3 HidePosition = new Vector3(-9999f, -9999f, -9999f);

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float range = 2f;
    [SerializeField]
    private float fireRate = 0.05f;    //分速0.05発

    private float coolTime; //連射クールタイム

	// Use this for initialization
	protected override void Start () {
        //必ずFacilityBehaviourのStart関数を最初に実行する
        base.Start();

        coolTime = 0f;
	}
	
	// Update is called once per frame
	protected override void Update () {
        //必ずFacilityBehaviourのUpdate関数を最初に実行する
        base.Update();

        foreach (var enemy in board.Aliens)
        {
            if (enemy.state == Alien.AlienState.Escaping && coolTime < 0f && Vector2.Distance(enemy.Position, MyFacility.Position) < range)
            {
                //弾発射
                Bullet newBullet = GameObject.Instantiate(bulletPrefab, HidePosition, Quaternion.identity).GetComponent<Bullet>();
                newBullet.Position = (Vector2)MyFacility.Position;
                newBullet.Vec = (enemy.Position - (Vector2)MyFacility.Position).normalized;

                coolTime = 1f / fireRate;
            }
        }
        coolTime -= FieldTimeManager.DeltaSecond / 60.0f;
	}
}
