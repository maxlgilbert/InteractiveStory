using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootGuard : StateAction {
	public override string GetActionText ()
	{
		return "You shot ";
	}
	public override List<AStarNode> TryAction (AStarNode curr)
	{
        //If meets requirements
        _possibleNeighbors = new List<AStarNode>();
        StateNode neighbor = null;
        StateNode currState = curr as StateNode;
        SmartState protag = currState.globalState[currState.stateName];
        int roomNumber = (int) protag.GetValue("Room");
        //Debug.LogError(roomNumber);
        if (StateStory.Instance.fixedRooms.ContainsKey(roomNumber))
        {
            if (protag.GetValue(Gun.StateName) == 1.0f)
            {
                List<StateObject> guards = new List<StateObject>();
                if (StateStory.Instance.roles.TryGetValue(Role.Guard, out guards))
                {
                    for (int i = 0; i < guards.Count; i++)
                    {
                        SmartState guardState = currState.globalState[guards[i].gameObject.name];
                        if (FixedRoom.RoomsConnected(roomNumber, (int)guardState.GetValue("Room"), currState.globalState, true))
                        {
                            neighbor = new StateNode(currState.globalState);

                            List<StateObject> characters = new List<StateObject>();
                            if (StateStory.Instance.roles.TryGetValue(Role.Character, out characters))
                            {
                                for (int k = 0; k < characters.Count; k++)
                                {
                                    string friendName = characters[k].gameObject.name;
                                    StateCharacter.SetEmotionalState(friendName, "Fear", 5f, neighbor.globalState);
                                }
                                StateCharacter.SetGlobalState(guards[i].gameObject.name, "Room", StateStory.Instance.fixedRooms.Count, neighbor.globalState);

                                neighbor.actions = StateStory.Instance.actions;

                                int numActions = 0;
                                for (int j = 0; j < curr.parentActions.Count; j++)
                                {
                                    neighbor.parentActions.Add(curr.parentActions[j]);
                                    if (curr.parentActions[j] == (this.GetActionText() + guards[i].gameObject.name))
                                    {
                                        numActions++;
                                    }
                                }
                                if (numActions < this.numberAvailable)
                                {
                                    neighbor.actionID = this.actionIndex;
                                    neighbor.actionID |= StateStory.Instance.protagonist.objectIndex;
                                    neighbor.parentActions.Add(this.GetActionText() + guards[i].gameObject.name);
                                    _possibleNeighbors.Add(neighbor);
                                }
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
		returnString += "Requirements: have gun, guard reachable.\n";
		returnString += "Results: guard back in \"Story Tool Box\", increase in fear of all characters.";
		return returnString;
	}

    public override void EnactAction(string actionText)
    {
        string[] actionWords = actionText.Split(new char[] { ' ', '.' });
        StateObject guard = StateStory.Instance.GetStateObject(actionWords[actionWords.Length - 1]);
        Vector3 guardLocation = guard.gameObject.transform.position;
        Vector3 graveYard = StateStory.Instance.fixedRooms[-1].gameObject.transform.position;
        graveYard.x += .5f;
        ActionCompletedHandler shootGuard = () =>
        {
            guard.MoveToWithin(graveYard, 1.0f, OnActionCompleted,null,true);
        };
        //doorToOpen.MoveToWithin(newDoorLocation, 1.0f, actionCompleted);
        StateStory.Instance.protagonist.MoveToWithin(guardLocation, 3.0f, shootGuard);
    }
}
