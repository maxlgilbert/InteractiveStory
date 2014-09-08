using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoToPark : StateAction {
	public override string GetActionText ()
	{
		return "You went to the park.";
	}
	public override AStarNode TryAction (AStarNode curr)
	{
		//If meets requirements
		StateNode neighbor = null;
		StateNode currState = curr as StateNode;
		if (currState.globalState[currState.stateName].y<=.7 && currState.globalState[currState.stateName].z<=.7) {
			neighbor = new StateNode(currState.globalState); /*Instantiate(StateStory.Instance.statePrefab,
			                       new Vector3(),
			                       Quaternion.identity) as StateNode;*/
			/*neighbor.globalState[currState.stateName] = new Vector4(currState.globalState[currState.stateName].x,
			                                      currState.globalState[currState.stateName].y-.2f,
			                                      currState.globalState[currState.stateName].z-.2f,
			                                      currState.globalState[currState.stateName].w);*/
			neighbor.SetState(currState.stateName,0,-.2f,-.2f,0);
			
			neighbor.actions=StateStory.Instance.actions;
		}
		return neighbor;
	}
}
