using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateNode : AStarNode {
	
	void Start () {
		actions = StateStory.Instance.actions;
	}

	public bool happy;
	//Get from sad to happy node
	//Just use one action for now -> define concept of action
	//Print out action's name...
	//Add state needed for action...

	public override bool Equals (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		return (happy == otherState.happy);
	}

	public override float distance (AStarNode other)
	{
		return 1.0f;
	}

	public override float estimate (AStarNode other)
	{
		return 1.0f;
	}
}
