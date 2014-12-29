using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListenToRadio : StateAction {
	public override string GetActionText ()
	{
		return " went to investigate ";
	}
    public override List<AStarNode> TryAction(AStarNode curr)
    {
        //If meets requirements
        _possibleNeighbors = new List<AStarNode>();
        StateNode neighbor = null;
        StateNode currState = curr as StateNode;

        List<StateObject> radios = new List<StateObject>();
        if (StateStory.Instance.roles.TryGetValue(Role.Radio, out radios))
        {
            List<StateObject> guards = new List<StateObject>();
            if (StateStory.Instance.roles.TryGetValue(Role.Guard, out guards))
            {
                for (int i = 0; i < guards.Count; i++)
                {
                    for (int k = 0; k < radios.Count; k++)
                    {
                        SmartState radioState = currState.globalState[radios[k].gameObject.name];
                        SmartState guardState = currState.globalState[guards[i].gameObject.name];
                        if (FixedRoom.RoomsConnected((int)radioState.GetValue("Room"), (int)guardState.GetValue("Room"), currState.globalState, true))
                        {
                            neighbor = new StateNode(currState.globalState);
                            StateCharacter.SetGlobalState(guards[i].gameObject.name, "Room", (int)radioState.GetValue("Room"), neighbor.globalState);

                            neighbor.actions = StateStory.Instance.actions;

                            int numActions = 0;
                            for (int j = 0; j < curr.parentActions.Count; j++)
                            {
                                neighbor.parentActions.Add(curr.parentActions[j]);
                                if (curr.parentActions[j] == (guards[i].gameObject.name + this.GetActionText() + radios[k].gameObject.name))
                                {
                                    numActions++;
                                }
                            }
                            if (numActions < this.numberAvailable)
                            {
                                neighbor.actionID = this.actionIndex;
                                neighbor.actionID |= StateStory.Instance.protagonist.objectIndex;
                                neighbor.parentActions.Add(guards[i].gameObject.name + this.GetActionText() + radios[k].gameObject.name);
                                _possibleNeighbors.Add(neighbor);
                            }
                        }
                    }
                }
            }
        }
        return _possibleNeighbors;
    }

	public override string ToString ()
	{
		string returnString = gameObject.name + "\n";
		returnString += "Requirements: radio reachable.\n";
		returnString += "Results: character goes to investiagte noise.";
		return returnString;
	}

    public override void EnactAction(List<StateObject> Actors, List<StateObject> Objects)
    {
        //ActionCompletedHandler actionCompleted = () => OnActionCompleted();
        //Actors[0].WaitFor(2.0f, actionCompleted);
        base.EnactAction(Actors, Objects);
    }
}
