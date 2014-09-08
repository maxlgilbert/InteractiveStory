using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConverseDeep : StateAction {

	public override string GetActionText ()
	{
		return "You had a deep conversation.";
	}
	public override AStarNode TryAction (AStarNode curr)
	{
		//If meets requirements
		StateNode neighbor = null;
		StateNode currState = curr as StateNode;
		if (currState.globalState[currState.stateName].z<=.5) {
			neighbor = new StateNode(currState.globalState);/*Instantiate(StateStory.Instance.statePrefab,
			                       new Vector3(),
			                       Quaternion.identity) as StateNode;*/
			/*neighbor.globalState[currState.stateName] = new Vector4(currState.globalState[currState.stateName].x,
			                                      currState.globalState[currState.stateName].y,
			                                      currState.globalState[currState.stateName].z,
			                                      currState.globalState[currState.stateName].w+.3f);*/
			neighbor.SetState(currState.stateName,0,0,0,.3f);
			
			neighbor.actions=StateStory.Instance.actions;
		}
		return neighbor;
	}
}
