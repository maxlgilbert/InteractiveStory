using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateStory : MonoBehaviour {

	public int maxDepth;
	public StateObject protagonist;
	//public StateObject goal;
	private StateNode _start;
	private StateNode _goal;
	public StateNode statePrefab;
	private AStar _aStar;

	public List<AStarAction> actions;

	private static StateStory instance;

	private Dictionary<string,Vector4> _globalState;

	public Dictionary<Role,List<string>> roles;
	
	public static StateStory Instance
	{
		get 
		{
			return instance;
		}
	}
	void Awake() {
		instance = this;
		_globalState = new Dictionary<string, Vector4> ();
		roles = new Dictionary<Role, List<string>> ();
	}

	public void AddStateObject (StateObject stateObject) {
		_globalState.Add (stateObject.name, stateObject.emotionalState);
		if (roles.ContainsKey(stateObject.role)) {
			roles[stateObject.role].Add(stateObject.gameObject.name);
		} else {
			List<string> names = new List<string>();
			names.Add (stateObject.gameObject.name);
			roles[stateObject.role] = names;
		}
	}
	// Use this for initialization
	void Start () {
		_aStar = new AStar(maxDepth);
	}

	public void UpdateNeighbors () {
		_start.UpdateNeighbors();
		_goal.UpdateNeighbors();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			_start = new StateNode(_globalState);
			Dictionary<string,Vector4> goalState = new Dictionary<string, Vector4>();
			foreach (string key in _globalState.Keys) {
				goalState[key] = new Vector4(-1.0f,-1.0f,-1.0f,-1.0f);
			}
			goalState[protagonist.gameObject.name] = new Vector4(.5f,-1.0f,-1.0f,-1.0f);
			_goal = new StateNode(goalState);
			UpdateNeighbors();
			foreach (AStarNode node in _aStar.FindPath (_start, _goal)) {
				StateNode happyState = node as StateNode;
				string printState = "";
				if (happyState.parentAction != null) {
					Debug.LogError(happyState.parentAction.GetActionText());
					foreach (string key in _start.globalState.Keys) {
						printState += (key + ": " + happyState.globalState[key] + " ");
					}
					Debug.LogError(printState);
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			clearPath();
		}
	}
	
	
	public void clearPath () {
		_start.clear();
		_goal.clear();
		_aStar.Reset();
		
	}
}
