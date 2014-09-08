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

	public List<string> goalObjects;
	public List<Vector4> goalStates;
	private Dictionary<string, Vector4> _goalMap;

	public List<AStarAction> actions;

	private static StateStory instance;

	private Dictionary<string,Vector4> _globalState;

	public Dictionary<Role,List<string>> roles;

	private Dictionary<int,string> stateMap;

	private List<AStarNode> _plan;

	public string storyBoardText{
		get {
			string returnString = "";
			returnString += "Goal: Get to a state of ";
			returnString += StateToString(_goalMap);
			string printState = "";
			foreach (AStarNode node in _plan) {
				StateNode happyState = node as StateNode;
				if (happyState.parentAction != null) {
					printState += happyState.parentAction.GetActionText() + "\n";
					printState += StateToString( happyState.globalState);
				}
			}
			returnString += printState;
			return returnString;
		}
	}

	public string StateToString (Dictionary<string,Vector4> state){
		string printState = "";
		int keyNumber = 0;
		foreach (string key in state.Keys) {
			printState += key + ": ";
			bool first = true;
			for (int i = 0; i < 4; i++) {
				if (state[key][i] >= 0.0f) {
					if (!first){ 
						printState += ", ";
					} else {
						first = false;
					}
					printState += stateMap[i] + " " + state[key][i];
				}
			}
			keyNumber++;
			if (keyNumber != state.Keys.Count){
				printState += "; ";
			}
		}
		printState += "\n";
		return printState;

	}
	
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
		stateMap = new Dictionary<int, string>();
		stateMap[0] = "joy";
		stateMap[1] = "anger";
		stateMap[2] = "fear";
		stateMap[3] = "trust";
		roles = new Dictionary<Role, List<string>> ();
		_plan = new List<AStarNode>();
		_goalMap = new Dictionary<string, Vector4>();
		if (goalStates.Count == goalObjects.Count) {
			for (int i = 0; i < goalStates.Count; i++) {
				_goalMap.Add(goalObjects[i],goalStates[i]);
			}
		}

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
		if (Input.GetKeyDown(KeyCode.C)) {
			clearPath();
		}
	}

	public void StartStory () {
		_start = new StateNode(_globalState);
		Dictionary<string,Vector4> goalState = new Dictionary<string, Vector4>();
		foreach (string key in _globalState.Keys) {
			goalState[key] = new Vector4(-1.0f,-1.0f,-1.0f,-1.0f);
		}
		goalState[protagonist.gameObject.name] = new Vector4(.5f,-1.0f,-1.0f,-1.0f);
		_goal = new StateNode(goalState);
		UpdateNeighbors();
		_plan = _aStar.FindPath (_start, _goal);
	}
	
	
	public void clearPath () {
		_start.clear();
		_goal.clear();
		_aStar.Reset();
		_plan = new List<AStarNode>();
		
	}
}
