using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivateArtifact : StateAction {
	public override string GetActionText ()
	{
		return "activated the";
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
            List<string> willingFriends = new List<string>();
            List<string> availableFriends = new List<string>();
            List<StateObject> characters = new List<StateObject>();
            if (StateStory.Instance.roles.TryGetValue(Role.Character, out characters))
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    string friendName = StateStory.Instance.roles[Role.Character][i].gameObject.name;
                    if (currState.globalState[friendName].GetValue("Trust") >= 5) //TODO figure out interesting numbers
                    {

                        //Debug.LogError("here 3");
                        willingFriends.Add(friendName);
                    }
                    //Debug.LogError("here 4");
                    availableFriends.Add(friendName);
                }
                List<StateObject> artifacts = new List<StateObject>();
                if (StateStory.Instance.roles.TryGetValue(Role.Artifact, out artifacts))
                {
                    for (int i = 0; i < artifacts.Count; i++)
                    {
                        SmartState artifactState = currState.globalState[artifacts[i].gameObject.name];
                        int artifactRoomNumber = (int)artifactState.GetValue("Room");
                        //Debug.LogError("you " + roomNumber + " artifact " + artifactRoomNumber);
                        //string guardName = StateStory.Instance.roles[Role.Guard][0].gameObject.name;
                        //Debug.LogError("guard room " + currState.globalState[guardName].GetValue("Room"));
                        if (FixedRoom.RoomsConnected(roomNumber, artifactRoomNumber, currState.globalState, false))
                        {
                            //Debug.LogError("here 1");

                            // Can satisfy conditions to unlock artifact with 3 friends
                            if (willingFriends.Count >= 2 &&
                                FixedRoom.RoomsConnected((int)currState.globalState[willingFriends[0]].GetValue("Room"), artifactRoomNumber, currState.globalState, false) &&
                                FixedRoom.RoomsConnected((int)currState.globalState[willingFriends[1]].GetValue("Room"), artifactRoomNumber, currState.globalState, false))
                            {
                                //Debug.LogError("here 2");
                                neighbor = new StateNode(currState.globalState);

                                StateCharacter.SetGlobalState(artifacts[i].gameObject.name, AncientArtifact.StateName, 1.0f, neighbor.globalState);

                                neighbor.actions = StateStory.Instance.actions;

                                StateCharacter.SetEmotionalState(currState.stateName, "Joy", 5f, neighbor.globalState);

                                int numActions = 0;
                                for (int j = 0; j < curr.parentActions.Count; j++)
                                {
                                    neighbor.parentActions.Add(curr.parentActions[j]);
                                    if (curr.parentActions[j] == ("You, " + willingFriends[0] + " and " + willingFriends[1] + " " + this.GetActionText() + " " + artifacts[i].gameObject.name))
                                    {
                                        numActions++;
                                    }
                                }
                                if (numActions < this.numberAvailable)
                                {
                                    neighbor.actionID = this.actionIndex;
                                    neighbor.actionID |= StateStory.Instance.protagonist.objectIndex;
                                    neighbor.parentActions.Add("You, " + willingFriends[0] + " and " + willingFriends[1] + " " + this.GetActionText() + " " + artifacts[i].gameObject.name);
                                    _possibleNeighbors.Add(neighbor);
                                }
                            }
                            else if (availableFriends.Count >= 1)
                            {

                                if (protag.GetValue(Gun.StateName) == 1.0f)
                                {
                                    for (int k = 0; k < availableFriends.Count; k++)
                                    {
                                        string friendName = availableFriends[k];
                                        if (FixedRoom.RoomsConnected((int)currState.globalState[friendName].GetValue("Room"), artifactRoomNumber, currState.globalState, false))
                                        {
                                            neighbor = new StateNode(currState.globalState);

                                            StateCharacter.SetGlobalState(artifacts[i].gameObject.name, AncientArtifact.StateName, 1.0f, neighbor.globalState);

                                            neighbor.actions = StateStory.Instance.actions;

                                            StateCharacter.SetEmotionalState(currState.stateName, "Joy", 5f, neighbor.globalState);

                                            StateCharacter.SetGlobalState(friendName, "Joy", 0f, neighbor.globalState);
                                            StateCharacter.SetGlobalState(friendName, "Anger", 0f, neighbor.globalState);
                                            StateCharacter.SetGlobalState(friendName, "Fear", 0f, neighbor.globalState);
                                            StateCharacter.SetGlobalState(friendName, "Trust", 0f, neighbor.globalState);

                                            int numActions = 0;
                                            for (int j = 0; j < curr.parentActions.Count; j++)
                                            {
                                                neighbor.parentActions.Add(curr.parentActions[j]);
                                                if (curr.parentActions[j] == ("You sacrificed " + friendName + " and then " + this.GetActionText() + " " + artifacts[i].gameObject.name))
                                                {
                                                    numActions++;
                                                }
                                            }
                                            if (numActions < this.numberAvailable)
                                            {
                                                neighbor.actionID = this.actionIndex;
                                                neighbor.actionID |= StateStory.Instance.protagonist.objectIndex;
                                                neighbor.parentActions.Add("You sacrificed " + friendName + " and then " + this.GetActionText() + " " + artifacts[i].gameObject.name);
                                                _possibleNeighbors.Add(neighbor);
                                            }
                                        }

                                    }
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
		returnString += "Requirements: three friends present (trust greater than 5), or a gun.\n";
		returnString += "Results: artifact activated.";
		return returnString;
	}

    public override void EnactAction(string actionText)
    {
        ActionCompletedHandler actionCompleted = () => OnActionCompleted();
        ActionCompletedHandler doNothing = () =>  StateStory.Instance.protagonist.OnNothingChanged();
        string[] actionWords = actionText.Split(new char[] { ' ', '.'});
        StateObject artifact = StateStory.Instance.GetStateObject(actionWords[actionWords.Length - 1]);
        if (actionWords[1].Equals("sacrificed"))
        {
            StateObject sacrificialVictim = StateStory.Instance.GetStateObject(actionWords[2]);
            sacrificialVictim.MoveToWithin(artifact.gameObject.transform.position, 2.0f, doNothing);

            Vector3 graveYard = StateStory.Instance.fixedRooms[-1].gameObject.transform.position;
            graveYard.x += -.5f;
            graveYard.z += -.8f;
            ActionCompletedHandler shootFriend = () =>
            {
                sacrificialVictim.MoveToWithin(graveYard, 1.0f, OnActionCompleted, null, true);
            };
            StateStory.Instance.protagonist.MoveToWithin(artifact.gameObject.transform.position, 2.0f, shootFriend);

        }
        else
        {
            StateObject friendOne = StateStory.Instance.GetStateObject(actionWords[1]);
            StateObject friendTwo = StateStory.Instance.GetStateObject(actionWords[3]);
            StateStory.Instance.protagonist.MoveToWithin(artifact.gameObject.transform.position, 1.0f, actionCompleted);
            friendOne.MoveToWithin(artifact.gameObject.transform.position, 1.0f, doNothing);
            friendTwo.MoveToWithin(artifact.gameObject.transform.position, 1.0f, doNothing);
        }
    }
}
