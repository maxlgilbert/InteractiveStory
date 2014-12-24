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
		if (currState.globalState[currState.stateName].z<=7) {
			neighbor = new StateNode(currState.globalState);

			
			neighbor.SetState(currState.stateName,1,0,0,0);
			
			neighbor.actions=StateStory.Instance.actions;
			string friendName = StateStory.Instance.roles[Role.Character][0].gameObject.name;
			
			neighbor.SetState(friendName,1,0,0,0);
		}
		if (neighbor != null) {
			int numActions = 0;
			for (int j  = 0; j < curr.parentActions.Count; j++) {
				neighbor.parentActions.Add(curr.parentActions[j]);
				if (curr.parentActions[j] == (this.GetActionText())) {
					numActions++;
				}
			}
            if (numActions < this.numberAvailable)
            {
                neighbor.actionID = this.actionIndex;
				neighbor.parentActions.Add(this.GetActionText());
				_possibleNeighbors.Add(neighbor);
			}
		}
		return _possibleNeighbors;
	}

	public override string ToString ()
	{
		string returnString = gameObject.name + "\n";
		returnString += "Requirements: fear less then 8 for both characters.\n";
		returnString += "Raises joy by 1 for both characters.";
		return returnString;
	}
}
