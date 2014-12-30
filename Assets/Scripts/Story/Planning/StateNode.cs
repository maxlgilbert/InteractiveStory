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

	public StateNode (Dictionary<string,SmartState> state) {
		actions = StateStory.Instance.actions;
		globalState = new Dictionary<string, SmartState> ();
		
		foreach (string key in state.Keys) {
			globalState[key] = new SmartState(state[key]);
		}
		stateName = "You";

	}

	public bool happy;

	public string stateName;

	public Vector4 emotionalState;

	public Dictionary<string, SmartState> globalState;
	//Get from sad to happy node
	//Just use one action for now -> define concept of action
	//Print out action's name...
	//Add state needed for action...
	/*public void SetGlobalState (string stateToChange, string field, float value){//float x, float y, float z, float w) {
		if (globalState.ContainsKey(stateToChange)) {
			float newValue = globalState [stateToChange].GetValue(field) + value;
			if (newValue < 0) newValue = 0;
			if (newValue > 10) newValue = 10;
			if (globalState [stateToChange].GetValue(field) < 0) newValue = -1;
			globalState [stateToChange].SetState(field,newValue);// = new Vector4 (newX, newY, newZ, newW);
		}
	}*/
	public override bool Equals (AStarNode other)
	{
		StateNode otherState = other as StateNode;
		
		//TODO Go both ways? 
		foreach (string key in globalState.Keys) {
			if (globalState.ContainsKey(key) && otherState.globalState.ContainsKey(key)){
	            if (!globalState[key].Equals(otherState.globalState[key]))
	            {

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
		//TODO Go both ways? 
		foreach (string key in globalState.Keys) {
            //for (int i = 0; i < 4; i++) {
            //    if (globalState[key][i] >= 0 && otherState.globalState[key][i] >= 0) {
            //        distance += Mathf.Abs (globalState[key][i] - otherState.globalState[key][i]);
            //    }
			//}
			if (globalState.ContainsKey(key) && otherState.globalState.ContainsKey(key)) {
				distance += globalState[key].Distance(otherState.globalState[key]);
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
            //for (int i = 0; i < 4; i++) {
            //    if (globalState[key][i] >= 0 && otherState.globalState[key][i] >= 0) {
            //        distance += Mathf.Abs (globalState[key][i] - otherState.globalState[key][i]);
            //    }
			//}
			if (globalState.ContainsKey(key) && otherState.globalState.ContainsKey(key)) {
				distance += globalState[key].Distance(otherState.globalState[key]);
			}
		}
		return distance;
	}
}
