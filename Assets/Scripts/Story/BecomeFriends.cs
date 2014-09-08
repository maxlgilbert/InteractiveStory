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
			/*neighbor.globalState[currState.stateName] = new Vector4(currState.globalState[currState.stateName].x+.5f,
			                                      currState.globalState[currState.stateName].y,
			                                      currState.globalState[currState.stateName].z,
			                                      currState.globalState[currState.stateName].w);*/

			
			neighbor.SetState(currState.stateName,.5f,0,0,0);
			
			neighbor.actions=StateStory.Instance.actions;
			string friendName = StateStory.Instance.roles[Role.Character][0];
			/*neighbor.globalState[friendName] = new Vector4(currState.globalState[friendName].x+.3f,
			                                                                                 currState.globalState[friendName].y,
			                                                                                 currState.globalState[friendName].z,
			                                                                                 currState.globalState[friendName].w);*/
			
			neighbor.SetState(friendName,.5f,0,0,0);
		}
		//Update neighbor??????
		return neighbor;
	}
}
