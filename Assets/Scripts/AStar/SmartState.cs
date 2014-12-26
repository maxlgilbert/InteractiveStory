using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartState {

    private Dictionary<string,float> _state;
    public SmartState()
    {
        _state = new Dictionary<string,float>();
	}

	public SmartState(SmartState other)
	{
		_state = new Dictionary<string,float>();
		foreach(string field in other.GetFields()) {
			_state[field] = other.GetValue(field);
		}
	}


    public void AddState (string field, float value) {
        _state[field] = value;
    }

    public void SetState(string field, float value)
    {
        _state[field] = value;
    }

    public float GetValue(string field)
    {
		if (_state.ContainsKey(field)){
        return _state[field];
		}
		else return -1.0f;
    }

    public bool Contains(string field)
    {
        return _state.ContainsKey(field);
    }

	public List<string> GetFields() {
		List<string> fields = new List<string>();
		foreach(string key in _state.Keys) {
			fields.Add(key);
		}
		return fields;
	}

    public float Distance(SmartState otherState)
    {
        float distance = 0.0f;
        foreach (string key in _state.Keys)
        {
            if (_state[key] >= 0 && otherState.Contains(key) && otherState.GetValue(key) >= 0)
            {
                distance += Mathf.Abs(_state[key] - otherState.GetValue(key));
            }
        }
        return distance;
    }

    public bool Equals(SmartState other)
    {
        //for (int i = 0; i < 4; i++) {
        //    if (globalState[key][i] >= 0 && otherState.globalState[key][i] >= 0) {
        //        if (globalState[key][i] != otherState.globalState[key][i]) {
        //            return false;
        //        }
        //    }
        //}
        //SmartState otherState = other as SmartState;
        foreach (string key in _state.Keys)
        {
            float value = _state[key];
			if (other.Contains(key)) {
	            float otherValue = other.GetValue(key);
	            if (value >= 0 && otherValue >= 0)
	            {
	               // Debug.LogError(key +": " + value + ", other value: " + otherValue);
	                if (value != otherValue)
	                {
	                    return false;
	                }
	            }
			}
        }
        return true;
    }
}
