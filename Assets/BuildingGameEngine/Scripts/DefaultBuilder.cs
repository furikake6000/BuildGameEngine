using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBuilder : MonoBehaviour {

    [SerializeField]
    private List<BuildOperation> buildOperations;

    private FieldBoardBuilder boardBuilder;

	// Use this for initialization
	void Start () {
        boardBuilder = GetComponent<FieldBoardBuilder>();

		foreach(var buildOperation in buildOperations)
        {
            boardBuilder.PutFacility(buildOperation.facilityPrefab.GetComponent<Facility>(), buildOperation.location);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[Serializable]
public class BuildOperation
{
    public GameObject facilityPrefab;
    public Vector2Int location;
}