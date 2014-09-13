using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Call : StateAction {
	public override string GetActionText ()
	{
		return "You had a phone conversation";
	}
	//Add virtual "Test" then generate neighbros if successful
	public override List<AStarNode> TryAction (AStarNode curr)
	{
		//If meets requirements
		_possibleNeighbors = new List<AStarNode>();
		StateNode neighbor = null;
		StateNode currState = curr as StateNode;
		if (currState.globalState[currState.stateName].x<0.1f) {
			neighbor = new StateNode(currState.globalState); /*Instantiate(StateStory.Instance.statePrefab,
			                                 new Vector3(),
			                                 Quaternion.identity) as StateNode;*/
			/*neighbor.globalState[currState.stateName] = new Vector4(currState.globalState[currState.stateName].x+.5f,
			                                      currState.globalState[currState.stateName].y,
			                                      currState.globalState[currState.stateName].z,
			                                      currState.globalState[currState.stateName].w);*/

			
			neighbor.SetState(currState.stateName,.1f,0,0,0);
			
			neighbor.actions=StateStory.Instance.actions;
			string friendName = StateStory.Instance.roles[Role.Character][0];
			/*neighbor.globalState[friendName] = new Vector4(currState.globalState[friendName].x+.3f,
			                                                                                 currState.globalState[friendName].y,
			                                                                                 currState.globalState[friendName].z,
			                                                                                 currState.globalState[friendName].w);*/
			
			neighbor.SetState(friendName,.1f,0,0,0);
		}
		if (neighbor != null) {
			int numActions = 0;
			for (int j  = 0; j < curr.parentActions.Count; j++) {
				neighbor.parentActions.Add(curr.parentActions[j]);
				if (curr.parentActions[j] == (this.GetActionText())) {
					numActions++;
				}
			}
			if (numActions < this.numberAvailable) {
				neighbor.parentActions.Add(this.GetActionText());
				_possibleNeighbors.Add(neighbor);
			}
		}
		//Update neighbor??????
		//_possibleNeighbors.Add(neighbor);
		return _possibleNeighbors;	}
}
