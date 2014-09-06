using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConverseDeep : StateAction {
	public override AStarNode TryAction (AStarNode curr)
	{
		//If meets requirements
		StateNode neighbor = null;
		StateNode currState = curr as StateNode;
		if (currState.emotionalState.z<=.5) {
			neighbor = Instantiate(StateStory.Instance.statePrefab,
			                       new Vector3(),
			                       Quaternion.identity) as StateNode;
			neighbor.emotionalState = new Vector4(currState.emotionalState.x,
			                                      currState.emotionalState.y,
			                                      currState.emotionalState.z,
			                                      currState.emotionalState.w+.3f);
			
			neighbor.actions=StateStory.Instance.actions;
		}
		return neighbor;
	}
}
