using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBoardBuilder : MonoBehaviour {

    public Facility selectedFacility { get; set; }   //現在設置選択しているファシリティ

	// Use this for initialization
	void Start () {
        //無を選択
        selectedFacility = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
