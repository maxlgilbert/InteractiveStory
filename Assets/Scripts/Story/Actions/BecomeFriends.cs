using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BecomeFriends : StateAction {
	public override string GetActionText ()
	{
		return "You became friends with ";
	}
	//Add virtual "Test" then generate neighbros if successful
	public override List<AStarNode> TryAction (AStarNode curr)
	{
		//If meets requirements
		_possibleNeighbors = new List<AStarNode>();
		StateNode neighbor = null;
		StateNode currState = curr as StateNode;


		/*foreach (AStarNode possibleNeighbor in possibleNeighbors) {
			if (possibleNeighbor != null) {
				int numActions = 0;
				for (int i  = 0; i < this.parentActions.Count; i++) {
					possibleNeighbor.parentActions.Add(this.parentActions[i]);
					if (this.parentActions[i] == action.GetActionText()) {
						numActions++;
					}
				}
				if (numActions < action.numberAvailable) {
					possibleNeighbor.parentActions.Add(action.GetActionText());
					_neighbors.Add(possibleNeighbor);
				}
			}
		}*/



        if (currState.globalState[currState.stateName].GetValue("Anger") <= 5 && currState.globalState[currState.stateName].GetValue("Trust")>= 5)
        {
			for (int i = 0; i < StateStory.Instance.roles[Role.Character].Count; i++) {
				string friendName = StateStory.Instance.roles[Role.Character][i].gameObject.name;
				if (FixedRoom.RoomsConnected((int) currState.globalState[currState.stateName].GetValue("Room"),(int)currState.globalState[friendName].GetValue("Room"),currState.globalState, false)
				    && currState.globalState[friendName].GetValue("Anger") <= 5 && currState.globalState[friendName].GetValue("Trust") >= 5)
                {
					neighbor = new StateNode(currState.globalState); /*Instantiate(StateStory.Instance.statePrefab,
					                                 new Vector3(),
					                                 Quaternion.identity) as StateNode;*/
					/*neighbor.globalState[currState.stateName] = new Vector4(currState.globalState[currState.stateName].x+.5f,
					                                      currState.globalState[currState.stateName].y,
					                                      currState.globalState[currState.stateName].z,
					                                      currState.globalState[currState.stateName].w);*/


                    StateCharacter.SetEmotionalState(currState.stateName, "Joy", 5f,neighbor.globalState);
                    //neighbor.SetGlobalState(currState.stateName, "Anger", 0);
                    //neighbor.SetGlobalState(currState.stateName, "Fear", 0);
                    //neighbor.SetGlobalState(currState.stateName, "Trust", 0);
                        //,0,0,0);
					
					neighbor.actions=StateStory.Instance.actions;
					/*neighbor.globalState[friendName] = new Vector4(currState.globalState[friendName].x+.3f,
					                                                                                 currState.globalState[friendName].y,
					                                                                                 currState.globalState[friendName].z,
					                                                                                 currState.globalState[friendName].w);*/



                    StateCharacter.SetEmotionalState(friendName, "Joy", 5f, neighbor.globalState);
                    //neighbor.SetGlobalState(friendName, "Anger", 0);
                    //neighbor.SetGlobalState(friendName, "Fear", 0);
                    //neighbor.SetGlobalState(friendName, "Trust", 0);

					int numActions = 0;
					for (int j  = 0; j < curr.parentActions.Count; j++) {
						neighbor.parentActions.Add(curr.parentActions[j]);
						if (curr.parentActions[j] == (this.GetActionText() + friendName)) {
							numActions++;
						}
					}
                    if (numActions < this.numberAvailable)
                    {
                        neighbor.actionID = this.actionIndex;
                        neighbor.actionID |= StateStory.Instance.roles[Role.Character][i].objectIndex;
                        neighbor.actionID |= StateStory.Instance.protagonist.objectIndex;
						neighbor.parentActions.Add(this.GetActionText() + friendName);
						_possibleNeighbors.Add(neighbor);
					}
				}
			}
		}
		//Update neighbor??????
		return _possibleNeighbors;
	}

	
	public override string ToString ()
	{
		string returnString = gameObject.name + "\n";
		returnString += "Requirements: anger less then 6 and trust greater than 4 for both characters.\n";
		returnString += "Results: joy increases by 5 for both characters.";
		return returnString;
	}

    public override void EnactAction(List<StateObject> Actors, List<StateObject> Objects)
    {
        ActionCompletedHandler actionCompleted = () => OnActionCompleted();
        Actors[0].MoveToWithin(Actors[1].gameObject.transform.position,1.0f,actionCompleted);
        //OnActionCompleted();
    }
}
