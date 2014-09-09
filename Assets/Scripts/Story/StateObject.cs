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

	void OnMouseDown () {
		StateStory.Instance.SetSelectedObject(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Select () {
		gameObject.renderer.material = StateStory.Instance.green;
	}

	public void Deselect () {
		gameObject.renderer.material = StateStory.Instance.red;
	}
}
