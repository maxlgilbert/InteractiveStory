using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BecomeFriends : StateAction {
	public override string GetActionText ()
	{
		return "You became friends!";
	}
	//Add virtual "Test" then generate neighbros if successful
	public override AStarNode TryAction (AStarNode curr)
	{
		//If meets requirements
		StateNode neighbor = null;
		StateNode currState = curr as StateNode;
		if (currState.globalState[currState.stateName].y<=.5 && currState.globalState[currState.stateName].w>=.5) {
			neighbor = new StateNode(currState.globalState); /*Instantiate(StateStory.Instance.statePrefab,
			                                 new Vector3(),
			                                 Quaternion.identity) as StateNode;*/
			neighbor.globalState[currState.stateName] = new Vector4(currState.globalState[currState.stateName].x+.5f,
			                                      currState.globalState[currState.stateName].y,
			                                      currState.globalState[currState.stateName].z,
			                                      currState.globalState[currState.stateName].w);
			
			neighbor.actions=StateStory.Instance.actions;
		}
		//Update neighbor??????
		return neighbor;
	}
}
