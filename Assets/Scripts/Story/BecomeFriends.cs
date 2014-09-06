using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BecomeFriends : StateAction {
	//Add virtual "Test" then generate neighbros if successful
	public override AStarNode TryAction (AStarNode curr)
	{
		//If meets requirements
		StateNode neighbor = null;
		StateNode currState = curr as StateNode;
		if (currState.emotionalState.y<=.5 && currState.emotionalState.w>=.5) {
			neighbor = Instantiate(StateStory.Instance.statePrefab,
			                                 new Vector3(),
			                                 Quaternion.identity) as StateNode;
			neighbor.emotionalState = new Vector4(currState.emotionalState.x+.5f,
			                                      currState.emotionalState.y,
			                                      currState.emotionalState.z,
			                                      currState.emotionalState.w);
			
			neighbor.actions=StateStory.Instance.actions;
		}
		//Update neighbor??????
		return neighbor;
	}
}
