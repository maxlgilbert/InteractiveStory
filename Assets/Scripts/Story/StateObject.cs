using UnityEngine;
using System.Collections;
public enum Role {
	Protagonist,
	Character
}

public class StateObject : MonoBehaviour {
	public Vector4 emotionalState;

	public Role role;
	// Use this for initialization
	void Start () {
		StateStory.Instance.AddStateObject (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
