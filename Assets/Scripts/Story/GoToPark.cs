using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoToPark : StateAction {
	public override AStarNode TryAction (AStarNode curr)
	{
		//If meets requirements
		StateNode neighbor = null;
		StateNode currState = curr as StateNode;
		if (currState.emotionalState.y<=.7 && currState.emotionalState.z<=.7) {
			neighbor = Instantiate(StateStory.Instance.statePrefab,
			                       new Vector3(),
			                       Quaternion.identity) as StateNode;
			neighbor.emotionalState = new Vector4(currState.emotionalState.x,
			                                      currState.emotionalState.y-.2f,
			                                      currState.emotionalState.z-.2f,
			                                      currState.emotionalState.w);
			
			neighbor.actions=StateStory.Instance.actions;
		}
		return neighbor;
	}
}
