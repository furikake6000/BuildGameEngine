using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FacilityBehaviour : MonoBehaviour {

    protected static FieldBoard board;
    protected Facility myFacility;

    // Use this for initialization
    protected virtual void Start () {
        if (board == null) board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();
        myFacility = GetComponent<Facility>();
    }
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}
}
