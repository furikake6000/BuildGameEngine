using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        set
        {
            facilityName = value;
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

        set
        {
            size = value;
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

        set
        {
            cost = value;
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
