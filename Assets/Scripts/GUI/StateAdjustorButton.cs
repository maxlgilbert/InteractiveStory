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
		bool reverse = false;
		int index = StateStory.Instance.selectedState;
		int moves = StateStory.Instance.numberOfMoves[StateStory.Instance.GetSelectedObject().name][index];
		if (direction) {
			if (moves < 0) reverse = true;
		} else {
			if (moves > 0) reverse = true;
		}
		if (reverse || StateStory.Instance.movesLeft > 0) {
			float increment = (direction) ? StateStory.Instance.adjustorIncrement : -StateStory.Instance.adjustorIncrement;
			float x = (index == 0) ? increment : 0;
			float y = (index == 1) ? increment : 0;
			float z = (index == 2) ? increment : 0;
			float w = (index == 3) ? increment : 0;
			if (StateStory.Instance.ChangeStateOfObject (StateStory.Instance.GetSelectedObject(),x,y,z,w)){
				if (direction){
					StateStory.Instance.numberOfMoves[StateStory.Instance.GetSelectedObject().name][index] += 1;
				} else {
					StateStory.Instance.numberOfMoves[StateStory.Instance.GetSelectedObject().name][index] -= 1;
				}
			}
		}
	}
}
