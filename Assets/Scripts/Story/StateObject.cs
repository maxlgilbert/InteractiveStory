using UnityEngine;
using System.Collections;

public class StateObject : MonoBehaviour {
	public Vector4 emotionalState;

	// Use this for initialization
	void Start () {
		StateStory.Instance.AddStateObject (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
