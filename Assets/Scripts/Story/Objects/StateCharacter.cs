using UnityEngine;
using System.Collections;

public class StateCharacter : StateObject {

	public Vector4 emotionalState;

	//public Role role;


	void Awake () {
		state = new SmartState();
		state.AddState("Joy", emotionalState.x);
		state.AddState("Anger", emotionalState.y);
		state.AddState("Fear", emotionalState.z);
		state.AddState("Trust", emotionalState.w);
	}
}
