using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float colRange;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;

    private static FieldBoard board;
    private Vector2 position;

    private Vector2 vec;    //発射ベクトル（正規化必須）

    public Vector2 Vec
    {
        get
        {
            return vec;
        }

        set
        {
            vec = value;
        }
    }

    public Vector2 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (board == null) board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();
    }

    // Update is called once per frame
    void Update () {
        //時間経過取得
        int deltaMinute;  //Update内で何分が経過したか
        deltaMinute = FieldTimeManager.FieldTimeNow.minute - FieldTimeManager.FieldTimePast.minute;
        if (deltaMinute < 0) deltaMinute += 60;

        Position += Vec * speed * deltaMinute;

		foreach(var enemy in board.Creatures)
        {
            if((enemy.Position - Position).magnitude < colRange)
            {
                enemy.Hp -= damage;
                Destroy(gameObject);
            }
        }

        //Positionを実座標に反映
        transform.position = board.MapPosToWorldPos(position);
    }
}
