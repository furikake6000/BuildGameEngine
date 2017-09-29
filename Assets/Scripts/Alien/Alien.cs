using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Character {
    [SerializeField]
    private string creatureName;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxHp;

    public string CreatureName
    {
        get
        {
            return creatureName;
        }

        set
        {
            creatureName = value;
        }
    }

    private Vector2Int housePos;

    private Fence myFence;  //自分が格納されているフェンス
    public Fence MyFence
    {
        get
        {
            return myFence;
        }

        set
        {
            myFence = value;
        }
    }

    private static FieldBoard board;
    private static Facility entrance;

    public AlienState state;
    public enum AlienState
    {
        Normal,
        Escaping,
        Subsided
    }

    private float hp;
    public float Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
        }
    }

    public void ResetPos(Vector2Int location)
    {
        //引数locationをhouse(自分の巣)として設定、そこにワープ
        housePos = location;
        Position = housePos;
        ClearCheckPoints();
    }

    // Use this for initialization
    void Start()
    {
        //HP設定
        Hp = maxHp;

        if (board == null) board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();
        state = AlienState.Normal;

        //逃走口がどこにあるか取得
        if (entrance == null)
        {
            entrance = GameObject.FindGameObjectWithTag("Entrance").GetComponent<Facility>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        //時間外なら逃走
        if ((FieldTimeManager.FieldTime.hour <= 5 || FieldTimeManager.FieldTime.hour >= 20) && state == AlienState.Normal)
        {
            state = AlienState.Escaping;
            //逃亡ルート策定
            AddCheckpoint(entrance);

            MessageManager.PutMessage(creatureName + "が逃げ出しました!!", MessageManager.MessagePriority.High);
        }

        if (state == AlienState.Escaping)
        {
            if (!Moving)
            {
                //移動完了？

            }
            if (Hp <= 0.0f)
            {
                //HP0になれば沈静化
                state = AlienState.Subsided;
                Position = housePos;
            }

        }

        if (state == AlienState.Subsided)
        {
            if (Random.value < 0.001f)
            {
                Hp = maxHp;
                state = AlienState.Escaping;
                //逃亡ルート策定
                AddCheckpoint(entrance);

                MessageManager.PutMessage(creatureName + "が逃げ出しました!!", MessageManager.MessagePriority.High);
            }
        }

        //朝になればリセット
        if ((state == AlienState.Escaping || state == AlienState.Subsided) && (FieldTimeManager.FieldTime.hour >= 6 && FieldTimeManager.FieldTime.hour <= 17))
        {
            state = AlienState.Normal;
            Position = housePos;
            Hp = maxHp;
        }
    }
}
