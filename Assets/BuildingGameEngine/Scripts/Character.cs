using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

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
    
    [SerializeField]
    private float speed;
    [SerializeField]
    private float hp;
    [SerializeField]
    private CharacterFaction faction;
    public enum CharacterFaction
    {
        player,
        enemy
    }

    private static FieldBoard board;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
