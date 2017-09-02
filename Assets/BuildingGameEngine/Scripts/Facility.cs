using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour {

    #region インスペクタ表示変数
    [SerializeField]
    private string facilityName;    //名前
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
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region オリジナル関数群

    #endregion
}
