using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float collisionRange;   //衝突域、どれくらい近くに敵がいればダメージを与えるか
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float range;    //どれくらい遠くまで飛ぶか（どれくらい初期位置から離れたら消えるか）

    private static FieldBoard board;
    private Vector2 position, startPosition;    //数値上の位置 startPositionは初期位置（範囲外消去判定に使用）

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
            startPosition = position;   //初期位置を記憶
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

        position += Vec * speed * deltaMinute;

        //敵当たり判定
		foreach(var enemy in board.Creatures)
        {
            if((enemy.Position - position).magnitude < collisionRange)
            {
                enemy.Hp -= damage;
                Destroy(gameObject);
            }
        }

        //範囲外判定
        if ((startPosition - position).magnitude > range)
        {
            Destroy(gameObject);
        }

        //Positionを実座標に反映
        transform.position = board.MapPosToWorldPos(position);
    }
}
