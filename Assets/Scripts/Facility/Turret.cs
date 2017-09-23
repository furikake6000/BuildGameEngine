using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    //オブジェクトを視認外に移動させたい時の座標（定数）
    private static readonly Vector3 HidePosition = new Vector3(-9999f, -9999f, -9999f);

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float range = 2f;
    [SerializeField]
    private float fireRate = 0.05f;    //分速0.05発

    private static FieldBoard board;
    private Facility myFacility;

    private float coolTime; //連射クールタイム

	// Use this for initialization
	void Start () {
        if (board == null) board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();
        myFacility = GetComponent<Facility>();

        coolTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        foreach (var enemy in board.Creatures)
        {
            if (enemy.state == Creature.CreatureState.Escaping && coolTime < 0f && Vector2.Distance(enemy.Position, myFacility.Position) < range)
            {
                //弾発射
                Bullet newBullet = GameObject.Instantiate(bulletPrefab, HidePosition, Quaternion.identity).GetComponent<Bullet>();
                newBullet.Position = (Vector2)myFacility.Position;
                newBullet.Vec = (enemy.Position - (Vector2)myFacility.Position).normalized;

                coolTime = 1f / fireRate;
            }
        }
        coolTime -= FieldTimeManager.DeltaSecond / 60.0f;
	}
}
