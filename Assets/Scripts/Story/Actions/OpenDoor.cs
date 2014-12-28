using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenDoor : StateAction {
	public override string GetActionText ()
	{
		return "You opened ";
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
            List<Door> doors = StateStory.Instance.fixedRooms[roomNumber].doors;
            for (int i = 0; i < doors.Count; i++)
            {
                if (!Door.CanGetThrough(doors[i].gameObject.name, currState.globalState))
                {
                    neighbor = new StateNode(currState.globalState);


                    Door.SetGlobalState(doors[i].gameObject.name, "Open", 1f, neighbor.globalState);
                    int nextRoomRumber = doors[i].roomOne;
                    if (nextRoomRumber == roomNumber)
                    {
                        nextRoomRumber = doors[i].roomTwo;
                    }
                    Door.SetGlobalState(currState.stateName, "Room", nextRoomRumber, neighbor.globalState);

                    neighbor.actions = StateStory.Instance.actions;

                    int numActions = 0;
                    for (int j = 0; j < curr.parentActions.Count; j++)
                    {
                        neighbor.parentActions.Add(curr.parentActions[j]);
                        if (curr.parentActions[j] == (this.GetActionText() + doors[i].gameObject.name))
                        {
                            numActions++;
                        }
                    }
                    if (numActions < this.numberAvailable)
                    {
                        neighbor.actionID = this.actionIndex;
                        neighbor.actionID |= StateStory.Instance.protagonist.objectIndex;
                        neighbor.parentActions.Add(this.GetActionText() + doors[i].gameObject.name);
                        _possibleNeighbors.Add(neighbor);
                    }
                }
            }
        }
        return _possibleNeighbors;
	}

	public override string ToString ()
	{
		string returnString = gameObject.name + "\n";
		returnString += "Requirements: door closed.\n";
		returnString += "Results: door opened.";
		return returnString;
	}

    public override void EnactAction(List<StateObject> Actors, List<StateObject> Objects)
    {
        //ActionCompletedHandler actionCompleted = () => OnActionCompleted();
        //Actors[0].WaitFor(2.0f, actionCompleted);
        base.EnactAction(Actors, Objects);
    }
}
