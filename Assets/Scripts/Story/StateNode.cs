using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateNode : AStarNode {
	
	void Start () {
		actions = StateStory.Instance.actions;
	}

	public bool happy;

	public Vector4 emotionalState;
	//Get from sad to happy node
	//Just use one action for now -> define concept of action
	//Print out action's name...
	//Add state needed for action...

	public override bool Equals (AStarNode other)
	{
		bool equals = true;
		StateNode otherState = other as StateNode;
		for (int i = 0; i < 4; i++) {
			if (emotionalState[i] > 0 && otherState.emotionalState[i] > 0) {
				equals = (emotionalState[i] == otherState.emotionalState[i]);
			}
		}
		return equals;
	}

	public override float distance (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		return (emotionalState-otherState.emotionalState).sqrMagnitude;
	}

	public override float estimate (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		return (emotionalState-otherState.emotionalState).sqrMagnitude;
	}
}
