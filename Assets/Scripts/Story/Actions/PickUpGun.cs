using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUpGun : StateAction {
	public override string GetActionText ()
	{
		return "You picked up ";
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
            if (protag.GetValue(Gun.StateName) != 1.0f)
            {
                List<StateObject> guns = new List<StateObject>();
                if (StateStory.Instance.roles.TryGetValue(Role.Gun, out guns))
                {
                    for (int i = 0; i < guns.Count; i++)
                    {
                        SmartState gunState = currState.globalState[guns[i].gameObject.name];
                        if (FixedRoom.RoomsConnected(roomNumber, (int)gunState.GetValue("Room"), currState.globalState, false))
                        {
                            neighbor = new StateNode(currState.globalState);


                            StateCharacter.SetGlobalState(currState.stateName, Gun.StateName, 1.0f, neighbor.globalState);

                            neighbor.actions = StateStory.Instance.actions;

                            int numActions = 0;
                            for (int j = 0; j < curr.parentActions.Count; j++)
                            {
                                neighbor.parentActions.Add(curr.parentActions[j]);
                                if (curr.parentActions[j] == (this.GetActionText() + guns[i].gameObject.name))
                                {
                                    numActions++;
                                }
                            }
                            if (numActions < this.numberAvailable)
                            {
                                neighbor.actionID = this.actionIndex;
                                neighbor.actionID |= StateStory.Instance.protagonist.objectIndex;
                                neighbor.parentActions.Add(this.GetActionText() + guns[i].gameObject.name);
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
		returnString += "Requirements: gun reachable.\n";
		returnString += "Results: pick up gun.";
		return returnString;
	}

    public override void EnactAction(string actionText)
    {
        string[] actionWords = actionText.Split(new char[] { ' ', '.' });
        StateObject gun = StateStory.Instance.GetStateObject(actionWords[actionWords.Length-1]);
        Vector3 gunLocation = gun.gameObject.transform.position;
        ActionCompletedHandler grabGun = () =>
        {
            gun.gameObject.transform.parent = StateStory.Instance.protagonist.transform;
            OnActionCompleted();
        };
        //doorToOpen.MoveToWithin(newDoorLocation, 1.0f, actionCompleted);
        StateStory.Instance.protagonist.MoveToWithin(gunLocation, 1.0f, grabGun);
    }
}
