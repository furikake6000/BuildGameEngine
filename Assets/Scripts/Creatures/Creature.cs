using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

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
    private Vector2 position;
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
    
    private List<Vector2Int> route = new List<Vector2Int>();
    private Vector2 nextPoint;

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

    public CreatureState state;
    public enum CreatureState
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
        housePos = location;
        position = housePos;
        nextPoint = location;   //袋小路に陥った時に0,0に行かないように
    }

    // Use this for initialization
    void Start ()
    {
        //HP設定
        Hp = maxHp;

        if (board == null) board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();
        state = CreatureState.Normal;

        //逃走口がどこにあるか取得
        if(entrance == null)
        {
            entrance = GameObject.FindGameObjectWithTag("Entrance").GetComponent<Facility>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Positionを実座標に反映
        transform.position = board.MapPosToWorldPos(position) + Vector3.back * 0.01001f;

        //時間外なら逃走
        if((FieldTimeManager.FieldTime.hour <= 5 || FieldTimeManager.FieldTime.hour >= 20) && state == CreatureState.Normal)
        {
            state = CreatureState.Escaping;
            //逃亡ルート策定
            route = board.SearchRoute(myFence.MyFacility, entrance);
            nextPoint = route[0];
            route.RemoveAt(0);

            MessageManager.PutMessage(creatureName + "が逃げ出しました!!", MessageManager.MessagePriority.High);
        }
        
        if(state == CreatureState.Escaping)
        {
            //移動
            float remainSpeed = speed * FieldTimeManager.DeltaSecond / 60.0f;
            while (true)
            {
                float distToNext = Vector2.Distance(position, nextPoint);
                if (remainSpeed < distToNext)
                {
                    //次の点に着くまでにスピード使い切る
                    position += (nextPoint - position) / distToNext * remainSpeed;
                    break;
                }
                else
                {
                    //次の点に着くまでにスピード使い切らない
                    remainSpeed -= distToNext;
                    position = nextPoint;

                    if(Vector2Int.Sishagonyu(position) == escapeGoal)
                    {
                        MessageManager.PutMessage(creatureName + "が園外に脱走してしまいました...5000円の損失です。", MessageManager.MessagePriority.High);
                        board.Money -= 5000;
                        Destroy(this.gameObject);
                    }

                    if (route.Count != 0)
                    {
                        //次の点を決定(VisitPointsから取り、0.5f四方の誤差を追加)
                        nextPoint = (Vector2)route[0] + new Vector2(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f);
                        route.RemoveAt(0);
                    }
                    else
                    {
                        //逃走確定
                        //ゲームオーバー
                        break;
                    }
                }
            }
            if (Hp <= 0.0f)
            {
                //HP0になれば沈静化
                state = CreatureState.Subsided;
                position = housePos;
            }
            
        }

        if(state == CreatureState.Subsided)
        {
            if(Random.value < 0.001f)
            {
                Hp = maxHp;
                state = CreatureState.Escaping;
                route = board.SearchRoute((Vector2Int)position, escapeGoal);
                nextPoint = route.Pop();

                MessageManager.PutMessage(creatureName + "が逃げ出しました!!", MessageManager.MessagePriority.High);
            }
        }

        //朝になればリセット
        if((state == CreatureState.Escaping || state == CreatureState.Subsided) && (FieldTimeManager.FieldTime.hour >= 6 && FieldTimeManager.FieldTime.hour <= 17))
        {
            state = CreatureState.Normal;
            position = housePos;
            Hp = maxHp;
        }
    }
}
