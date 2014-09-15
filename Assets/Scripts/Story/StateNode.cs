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
	public void SetState (string stateChange, float x, float y, float z, float w) {
		float newX = globalState [stateChange].x + x;
		float newY = globalState [stateChange].y + y;
		float newZ = globalState [stateChange].z + z;
		float newW = globalState [stateChange].w + w;
		if (newX < 0) newX = 0;
		if (newX > 10) newX = 10;
		if (newY < 0) newY = 0;
		if (newY > 10) newY = 10;
		if (newZ < 0) newZ = 0;
		if (newZ > 10) newZ = 10;
		if (newW < 0) newW = 0;
		if (newW > 10) newW = 10;
		if (globalState [stateChange].x < 0) newX = -1;
		if (globalState [stateChange].y < 0) newY = -1;
		if (globalState [stateChange].z < 0) newZ = -1;
		if (globalState [stateChange].w < 0) newW = -1;
		globalState [stateChange] = new Vector4 (newX, newY, newZ, newW);
	}
	public override bool Equals (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		
		//TODO Go both ways? 
		foreach (string key in globalState.Keys) {
			for (int i = 0; i < 4; i++) {
				if (globalState[key][i] >= 0 && otherState.globalState[key][i] >= 0) {
					if (globalState[key][i] != otherState.globalState[key][i]) {
						return false;
					}
				}
			}
		}
		return true;
	}

	public override float distance (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		float distance = 0.0f;
		//TODO Go both ways? 
		foreach (string key in globalState.Keys) {
			for (int i = 0; i < 4; i++) {
				if (globalState[key][i] >= 0 && otherState.globalState[key][i] >= 0) {
					distance += Mathf.Abs (globalState[key][i] - otherState.globalState[key][i]);
				}
			}
		}
		return distance;
	}

	public override float estimate (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		float distance = 0.0f;
		//TODO Go both ways? 
		foreach (string key in globalState.Keys) {
			for (int i = 0; i < 4; i++) {
				if (globalState[key][i] >= 0 && otherState.globalState[key][i] >= 0) {
					distance += Mathf.Abs (globalState[key][i] - otherState.globalState[key][i]);
				}
			}
		}
		return distance;
	}
}
