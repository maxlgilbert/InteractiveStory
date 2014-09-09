using UnityEngine;
using System.Collections;

public class StateAdjustorButton : MonoBehaviour {
	public bool direction;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {
		int index = StateStory.Instance.selectedState;
		float increment = (direction) ? StateStory.Instance.adjustorIncrement : -StateStory.Instance.adjustorIncrement;
		float x = (index == 0) ? increment : 0.0f;
		float y = (index == 1) ? increment : 0.0f;
		float z = (index == 2) ? increment : 0.0f;
		float w = (index == 3) ? increment : 0.0f;
		StateStory.Instance.ChangeStateOfObject (StateStory.Instance.GetSelectedObject(),x,y,z,w);
	}
}
