using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateStory : MonoBehaviour {

	public int maxDepth;
	public StateObject protagonist;
	private StateObject _selectedObject;
	private List<StateObject> _stateObjects;
	[HideInInspector] public int selectedState;
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
	public Material green;
	public Material red;

	public float adjustorIncrement;

	private bool _failedPath;
	
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
		_selectedObject = protagonist;
		_stateObjects = new List<StateObject> ();
		selectedState = 0;
		_failedPath = false;
		
	}

	
	// Use this for initialization
	void Start () {
		_aStar = new AStar(maxDepth);
		SetSelectedObject (protagonist);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.C)) {
			clearPath();
		}
	}

	public string storyBoardText{
		get {
			string returnString = "";
			returnString += "Goal: Get to a state of ";
			returnString += StateToString(_goalMap);
			if (!_failedPath) {
				string printState = "";
				foreach (AStarNode node in _plan) {
					StateNode happyState = node as StateNode;
					if (happyState.parentAction != null) {
						printState += happyState.parentAction.GetActionText() + "\n";
						printState += StateToString( happyState.globalState);
					}
				}
				returnString += printState;
			} else {
				returnString += "No possible path from this state!\n" +
					"Try adjusting the initial state.";
			}
			return returnString;
		}
	}

	public string initialStateText {
		get {
			string returnString = "";
			returnString += "Initial State:\n";
			returnString += StateToString(_globalState);
			returnString += "Selected object: " + _selectedObject.gameObject.name;
			return returnString;
		}
	}

	public StateObject GetSelectedObject () {
		return _selectedObject;
	}

	public void SetSelectedObject (StateObject stateObject) {
		for (int i = 0; i < _stateObjects.Count; i++) {
			_stateObjects[i].Deselect();
		}
		stateObject.Select ();
		_selectedObject = stateObject;
	}

	public void ChangeStateOfObject (StateObject stateObject, float x, float y, float z, float w) {
		float newX = _globalState [stateObject.gameObject.name].x + x;
		float newY = _globalState [stateObject.gameObject.name].y + y;
		float newZ = _globalState [stateObject.gameObject.name].z + z;
		float newW = _globalState [stateObject.gameObject.name].w + w;
		if (newX < 0.0f) newX = 0.0f;
		if (newX > 1.0f) newX = 1.0f;
		if (newY < 0.0f) newY = 0.0f;
		if (newY > 1.0f) newY = 1.0f;
		if (newZ < 0.0f) newZ = 0.0f;
		if (newZ > 1.0f) newZ = 1.0f;
		if (newW < 0.0f) newW = 0.0f;
		if (newW > 1.0f) newW = 1.0f;
		if (_globalState [stateObject.gameObject.name].x < 0.0f) newX = -1.0f;
		if (_globalState [stateObject.gameObject.name].y < 0.0f) newY = -1.0f;
		if (_globalState [stateObject.gameObject.name].z < 0.0f) newZ = -1.0f;
		if (_globalState [stateObject.gameObject.name].w < 0.0f) newW = -1.0f;
		_globalState [stateObject.gameObject.name] = new Vector4 (newX, newY, newZ, newW);
		stateObject.emotionalState = new Vector4 (newX, newY, newZ, newW);
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
				printState += "\n";
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

	public void AddStateObject (StateObject stateObject) {
		_stateObjects.Add (stateObject);
		_globalState.Add (stateObject.name, stateObject.emotionalState);
		if (roles.ContainsKey(stateObject.role)) {
			roles[stateObject.role].Add(stateObject.gameObject.name);
		} else {
			List<string> names = new List<string>();
			names.Add (stateObject.gameObject.name);
			roles[stateObject.role] = names;
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
		_failedPath = false;
		if (_plan == null) {
			_failedPath = true;
			clearPath();
		}
	}
	
	public void UpdateNeighbors () {
		_start.UpdateNeighbors();
		_goal.UpdateNeighbors();
		
	}

	
	
	public void clearPath () {
		_start.clear();
		_goal.clear();
		_aStar.Reset();
		_plan = new List<AStarNode>();
		
	}
}
