﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConverseDeep : StateAction
{

    public override string GetActionText()
    {
        return "You had a deep conversation.";
    }
    public override List<AStarNode> TryAction(AStarNode curr)
    {
        //If meets requirements
        _possibleNeighbors = new List<AStarNode>();
        StateNode neighbor = null;
        StateNode currState = curr as StateNode;
        if (currState.globalState[currState.stateName].GetValue("Fear") <= 5)
        {
            neighbor = new StateNode(currState.globalState);/*Instantiate(StateStory.Instance.statePrefab,
			                       new Vector3(),
			                       Quaternion.identity) as StateNode;*/
            /*neighbor.globalState[currState.stateName] = new Vector4(currState.globalState[currState.stateName].x,
                                                  currState.globalState[currState.stateName].y,
                                                  currState.globalState[currState.stateName].z,
                                                  currState.globalState[currState.stateName].w+.3f);*/

            //neighbor.SetGlobalState(currState.stateName, "Joy", 0);
            //neighbor.SetGlobalState(currState.stateName, "Anger", 0);
            //neighbor.SetGlobalState(currState.stateName, "Fear", 0);
            StateCharacter.SetEmotionalState(currState.stateName, "Trust", 3f, neighbor.globalState);

            neighbor.actions = StateStory.Instance.actions;
        }
        if (neighbor != null)
        {
            int numActions = 0;
            for (int j = 0; j < curr.parentActions.Count; j++)
            {
                neighbor.parentActions.Add(curr.parentActions[j]);
                if (curr.parentActions[j] == (this.GetActionText()))
                {
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

    public override string ToString()
    {
        string returnString = gameObject.name + "\n";
        returnString += "Requirements: fear less then 6 for both characters.\n";
        returnString += "Results: trust increases by 3 for both characters.";
        return returnString;
    }

    public override void EnactAction(string actionText)
    {
        base.EnactAction(actionText);
    }
}
