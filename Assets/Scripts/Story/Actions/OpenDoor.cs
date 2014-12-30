using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenDoor : StateAction {
	public override string GetActionText ()
	{
		return "You opened ";
	}
    public override List<AStarNode> TryAction(AStarNode curr)
    {
        //If meets requirements
        _possibleNeighbors = new List<AStarNode>();
        StateNode neighbor = null;
        StateNode currState = curr as StateNode;
        SmartState protag = currState.globalState[currState.stateName];
        int roomNumber = (int)protag.GetValue("Room");
        //Debug.LogError(roomNumber);
        if (StateStory.Instance.fixedRooms.ContainsKey(roomNumber))
        {
            List<StateObject> doorObjects = StateStory.Instance.roles[Role.Door];
            List<Door> doors = new List<Door>();
            foreach (StateObject doorObject in doorObjects)
            {
                Door door = doorObject as Door;
                doors.Add(door);
            }
            for (int i = 0; i < doors.Count; i++)
            {
                int doorRoom1 = doors[i].roomOne;
                int doorRoom2 = doors[i].roomTwo;
                if (!Door.IsGuarded(doors[i].gameObject.name, currState.globalState) &&
                    !Door.IsOpen(doors[i].gameObject.name, currState.globalState) &&
                    (FixedRoom.RoomsConnected(roomNumber, doorRoom1, currState.globalState, false) ||
                    FixedRoom.RoomsConnected(roomNumber, doorRoom2, currState.globalState, false)))
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

    public override void EnactAction(string actionText)
    {
        ActionCompletedHandler actionCompleted = () => OnActionCompleted();
        string[] actionWords = actionText.Split(new char[] { ' ', '.' });
        StateObject doorToOpen = StateStory.Instance.GetStateObject(actionWords[2]);
        Vector3 newDoorLocation = doorToOpen.gameObject.transform.position;
        newDoorLocation.y -= 10.0f;
        ActionCompletedHandler openDoor = () => doorToOpen.MoveToWithin(newDoorLocation, 1.0f, actionCompleted);
        //doorToOpen.MoveToWithin(newDoorLocation, 1.0f, actionCompleted);
        Vector3 goalPosition = doorToOpen.gameObject.transform.position;
        goalPosition.y = 0;
        StateStory.Instance.protagonist.MoveToWithin(goalPosition, 1.0f, openDoor);
    }
}
