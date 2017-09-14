using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
