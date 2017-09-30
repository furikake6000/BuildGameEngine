using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Character {
    [SerializeField]
    private string creatureName;

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
    
    private static Facility entrance;

    public AlienState state;
    public enum AlienState
    {
        Normal,
        Escaping,
        Subsided
    }

    public void ResetPos(Vector2Int location)
    {
        //引数locationをhouse(自分の巣)として設定、そこにワープ
        housePos = location;
        Position = housePos;
        ClearCheckPoints();
    }

    // Use this for initialization
    protected override void Start()
    {
        //必ずCharacterのStart関数を最初に実行する
        base.Start();
        state = AlienState.Normal;

        //逃走口がどこにあるか取得
        if (entrance == null)
        {
            entrance = GameObject.FindGameObjectWithTag("Entrance").GetComponent<Facility>();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        //必ずCharacterのUpdate関数を最初に実行する
        base.Update();

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
            if (hp <= 0.0f)
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
                ResetHp();
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
            ResetHp();
        }
    }
}
