using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldNavigator : MonoBehaviour {

    [SerializeField]
    private float speed;    //毎分

    private static FieldBoard board;

    private List<Vector2Int> checkpoints; //目的地の一覧（前から順に訪れる）
    public List<Vector2Int> Checkpoints
    {
        get
        {
            return checkpoints;
        }

        set
        {
            checkpoints = value;
        }
    }

    // Use this for initialization
    void Start () {
        if (board == null) board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
