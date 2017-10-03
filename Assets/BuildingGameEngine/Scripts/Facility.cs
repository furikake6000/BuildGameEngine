using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrikLib;

public class Facility : MonoBehaviour {

    #region インスペクタ表示変数
    [SerializeField]
    private string facilityName;    //名前
    public string FacilityName
    {
        get
        {
            return facilityName;
        }
    }
    [SerializeField]
    private Vector2Int size = new Vector2Int(1,1); //施設のサイズ
    public Vector2Int Size
    {
        get
        {
            return size;
        }
    }
    [SerializeField]
    private int cost;
    public int Cost
    {
        get
        {
            return cost;
        }
    }
    [SerializeField]
    private bool playerPassable, enemyPassable;  //通過可能か（敵・味方で違ってくる）
    public bool PlayerPassable
    {
        get
        {
            return playerPassable;
        }
    }
    public bool EnemyPassable
    {
        get
        {
            return enemyPassable;
        }
    }
    [SerializeField]
    private bool buildable; //建設可能か
    public bool Buildable
    {
        get
        {
            return buildable;
        }
    }

    private List<Vector2Int> accessablePoints;  //外部との隣接点（経路探索に必要）
    public List<Vector2Int> AccessablePoints
    {
        get
        {
            return accessablePoints;
        }
    }
    #endregion

    private Vector2Int position;
    public Vector2Int Position
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
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region オリジナル関数群

    #endregion
}
