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
		StateNode otherState = other as StateNode;
		for (int i = 0; i < 4; i++) {
			if (emotionalState[i] >= 0 && otherState.emotionalState[i] >= 0) {
				if (emotionalState[i] != otherState.emotionalState[i]) {
					return false;
				}
			}
		}
		return true;
	}

	public override float distance (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		float distance = 0.0f;
		for (int i = 0; i < 4; i++) {
			if (emotionalState[i] >= 0 && otherState.emotionalState[i] >= 0) {
				distance += Mathf.Abs (emotionalState[i] - otherState.emotionalState[i]);
			}
		}
		return distance;
	}

	public override float estimate (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		float distance = 0.0f;
		for (int i = 0; i < 4; i++) {
			if (emotionalState[i] >= 0 && otherState.emotionalState[i] >= 0) {
				distance += Mathf.Abs (emotionalState[i] - otherState.emotionalState[i]);
			}
		}
		return distance;
	}
}
