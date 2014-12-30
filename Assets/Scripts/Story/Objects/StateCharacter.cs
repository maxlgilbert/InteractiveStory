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
        state.AddState(Gun.StateName, 0);
        this.stuckAtZero = true;
	}

    public static void SetEmotionalState(string stateToChange, string field, float value, System.Collections.Generic.Dictionary<string, SmartState> globalState)
    {
        if (globalState.ContainsKey(stateToChange))
        {
            float newValue = globalState[stateToChange].GetValue(field) + value;
            /*float newY = globalState [stateChange].y + y;
            float newZ = globalState [stateChange].z + z;
            float newW = globalState [stateChange].w + w;*/
            if (newValue < 0) newValue = 0;
            if (newValue > 10) newValue = 10;
            /*if (newY < 0) newY = 0;
            if (newY > 10) newY = 10;
            if (newZ < 0) newZ = 0;
            if (newZ > 10) newZ = 10;
            if (newW < 0) newW = 0;
            if (newW > 10) newW = 10;*/
            if (globalState[stateToChange].GetValue(field) < 0) newValue = -1;
            /*if (globalState [stateChange].y < 0) newY = -1;
            if (globalState [stateChange].z < 0) newZ = -1;
            if (globalState [stateChange].w < 0) newW = -1;*/
            globalState[stateToChange].SetState(field, newValue);// = new Vector4 (newX, newY, newZ, newW);
        }
    }
}
