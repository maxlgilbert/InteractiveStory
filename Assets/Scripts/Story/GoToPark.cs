using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoToPark : StateAction {
	public override string GetActionText ()
	{
		return "You went to the park.";
	}
	public override List<AStarNode> TryAction (AStarNode curr)
	{
		//If meets requirements
		_possibleNeighbors = new List<AStarNode>();
		StateNode neighbor = null;
		StateNode currState = curr as StateNode;
		if (currState.globalState[currState.stateName].y<=9 && currState.globalState[currState.stateName].z<=9) {
			neighbor = new StateNode(currState.globalState); /*Instantiate(StateStory.Instance.statePrefab,
			                       new Vector3(),
			                       Quaternion.identity) as StateNode;*/
			/*neighbor.globalState[currState.stateName] = new Vector4(currState.globalState[currState.stateName].x,
			                                      currState.globalState[currState.stateName].y-.2f,
			                                      currState.globalState[currState.stateName].z-.2f,
			                                      currState.globalState[currState.stateName].w);*/
			neighbor.SetState(currState.stateName,0,-2,-2,0);
			
			neighbor.actions=StateStory.Instance.actions;
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
                //neighbor.actionID |= StateStory.Instance.roles[Role.Character][i].objectIndex;
                neighbor.actionID |= StateStory.Instance.protagonist.objectIndex;
				neighbor.parentActions.Add(this.GetActionText());
				_possibleNeighbors.Add(neighbor);
			}
		}
		//_possibleNeighbors.Add(neighbor);
		return _possibleNeighbors;
	}

	public override string ToString ()
	{
		string returnString = gameObject.name + "\n";
		returnString += "Requirements: anger and fear less then 10 for both characters.\n";
		returnString += "Results: anger and fear decrease by 2 for both characters.";
		return returnString;
	}

    public override void EnactAction(List<StateObject> Actors, List<StateObject> Objects)
    {
        ActionCompletedHandler actionCompleted = () => OnActionCompleted();
        Actors[0].WaitFor(2.0f, actionCompleted);
    }
}
