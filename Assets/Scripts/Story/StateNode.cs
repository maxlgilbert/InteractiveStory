using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateNode : AStarNode {
	/*void Awake () {
		globalState = new Dictionary<string, Vector4> ();
		stateName = "global";
		globalState[stateName] = emotionalState;
	}

	void Start () {
		actions = StateStory.Instance.actions;
	}*/

	public StateNode (Dictionary<string,Vector4> state) {
		actions = StateStory.Instance.actions;
		globalState = new Dictionary<string, Vector4> ();
		
		foreach (string key in state.Keys) {
			globalState[key] = state[key];
		}
		stateName = "Protagonist";

	}

	public bool happy;

	public string stateName;

	public Vector4 emotionalState;

	public Dictionary<string, Vector4> globalState;
	//Get from sad to happy node
	//Just use one action for now -> define concept of action
	//Print out action's name...
	//Add state needed for action...

	public override bool Equals (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		for (int i = 0; i < 4; i++) {
			if (globalState[stateName][i] >= 0 && otherState.globalState[stateName][i] >= 0) {
				if (globalState[stateName][i] != otherState.globalState[stateName][i]) {
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
			if (globalState[stateName][i] >= 0 && otherState.globalState[stateName][i] >= 0) {
				distance += Mathf.Abs (globalState[stateName][i] - otherState.globalState[stateName][i]);
			}
		}
		return distance;
	}

	public override float estimate (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		float distance = 0.0f;
		for (int i = 0; i < 4; i++) {
			if (globalState[stateName][i] >= 0 && otherState.globalState[stateName][i] >= 0) {
				distance += Mathf.Abs (globalState[stateName][i] - otherState.globalState[stateName][i]);
			}
		}
		return distance;
	}
}
