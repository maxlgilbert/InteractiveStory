using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateStory : MonoBehaviour {

	public int maxDepth;
	public StateObject protagonist;
	private StateObject _selectedObject;
	private Dictionary<ulong,StateObject> _stateObjects;
    private ulong _objectIndex = 1;
	[HideInInspector] public int selectedState;
	//public StateObject goal;
	private StateNode _start;
	private StateNode _goal;
	public StateNode statePrefab;
	private AStar _aStar;

	public List<string> goalObjects;
	public List<Vector4> goalStates;
	private Dictionary<string, Vector4> _goalMap;

	public Dictionary<ulong,AStarAction> actions;
    private StateAction _currentAction;
    private ulong _actionIndex = 0;

	private static StateStory instance;

	private Dictionary<string,Vector4> _globalState;

	public Dictionary<Role,List<StateObject>> roles;

	private Dictionary<int,string> stateMap;

	private List<AStarNode> _plan;
    private int _currentPlanStep = 0 ;
	public Material green;
	public Material red;

	public int adjustorIncrement;

	private bool _failedPath;

	public Dictionary<string,Dictionary<int,int>> numberOfMoves;

	public int availableMoves;

	private bool _triedPlan = false;
	
	void Awake() {
		instance = this;
		_globalState = new Dictionary<string, Vector4> ();
		stateMap = new Dictionary<int, string>();
		stateMap[0] = "joy";
		stateMap[1] = "anger";
		stateMap[2] = "fear";
		stateMap[3] = "trust";
		roles = new Dictionary<Role, List<StateObject>> ();
		_plan = new List<AStarNode>();
		_goalMap = new Dictionary<string, Vector4>();
		if (goalStates.Count == goalObjects.Count) {
			for (int i = 0; i < goalStates.Count; i++) {
				_goalMap.Add(goalObjects[i],goalStates[i]);
			}
		}
		_selectedObject = protagonist;
		_stateObjects = new Dictionary<ulong,StateObject> ();
		selectedState = 0;
		_failedPath = false;
		numberOfMoves = new Dictionary<string, Dictionary<int, int>>();
        actions = new Dictionary<ulong, AStarAction>();

		
	}

	
	// Use this for initialization
	void Start () {
		_aStar = new AStar(maxDepth);
		SetSelectedObject (protagonist);
        foreach (AStarAction action in actions.Values)
        {
            StateAction stateAction = action as StateAction;
            stateAction.ActionCompleted += PlayNextAction;
        }
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.C)) {
			clearPath();
        }
       /* if (Input.GetKeyDown(KeyCode.T))
        {
            foreach (AStarAction action in actions)
            {
                StateAction stateAction = action as StateAction;
                List<StateObject> Actors = new List<StateObject>();
                List<StateObject> Objects = new List<StateObject>();
                Actors.Add(protagonist);
                Actors.Add(_selectedObject);
                stateAction.EnactAction(Actors, Objects);
                Debug.LogError("Pressed T");
            }
        }*/
	}
	
	
	public int movesLeft {
		get {
			int moves = 0;
			foreach (string key in numberOfMoves.Keys) {
				for (int i = 0; i < 4; i ++) {
					moves += (int) Mathf.Abs(numberOfMoves[key][i]);
				}
			}
			return availableMoves - moves;
		}
	}
    public void PlayStory()
    {
        if (!_failedPath)
        {
            foreach (AStarNode node in _plan)
            {
                StateNode happyState = node as StateNode;
                if (happyState.parentActions != null && happyState.parentActions.Count != 0)
                {
                    StateAction stateAction = GetAction(happyState.actionID);
                    List<StateObject> Actors = GetObjects(happyState.actionID);
                    List<StateObject> Objects = new List<StateObject>();
                    //Actors.Add(protagonist);
                    //Actors.Add(_selectedObject);
                    stateAction.EnactAction(Actors,Objects);
                }
            }
        } 
    }
    public StateAction GetAction(ulong state)
    {
        return  actions[3 & state] as StateAction;
    }
    public void AddAction(StateAction stateAction)
    {
        actions[_actionIndex] = stateAction;
        stateAction.actionIndex = _actionIndex;
        stateAction.ActionCompleted += PlayNextAction;
        _actionIndex++;
    }
    public void PlayNextAction()
    {
        // IF next action can play, play
        // Else replan
        Debug.LogError("Play next step");
        if (!_failedPath && _currentPlanStep < _plan.Count)
        {
            Debug.LogError(_currentPlanStep);
            AStarNode node = _plan[_currentPlanStep];
            StateNode happyState = node as StateNode;
            StateAction stateAction = GetAction(happyState.actionID);
            List<StateObject> Actors = GetObjects(happyState.actionID);
            List<StateObject> Objects = new List<StateObject>();
            _currentPlanStep++;
            stateAction.EnactAction(Actors, Objects);
            Debug.LogError(_currentPlanStep);
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
					if (happyState.parentActions != null && happyState.parentActions.Count != 0) {
						printState += happyState.parentActions[happyState.parentActions.Count-1] + "\n";
						printState += StateToString( happyState.globalState);
					}
				}
				returnString += printState;
			} else {
				returnString += "No possible path from this state!\n" +
					"Try adjusting the initial state.";
			}
			if (_triedPlan && (_plan.Count == 0 || !_plan[_plan.Count - 1].Equals(_goal))){
				returnString += "Goal is not possible, try adjusting the states!";
			} else if (_triedPlan){
				returnString += "You reached the goal!";
			}
			return returnString;
		}
	}

	public string initialStateText {
		get {
			string returnString = "Adjust the state:\n";
			returnString += "Moves available: " + this.movesLeft + "\n";
			returnString += "Current State:\n";
			returnString += StateToString(_globalState);
			returnString += "Selected object: " + _selectedObject.gameObject.name;
			return returnString;
		}
	}
	
	public string actionListText {
		get {
			string returnString = "Available actions:\n";
			foreach (StateAction action in actions.Values) {
				returnString += action.ToString() + "\n";
			}
			return returnString;
		}
	}

    public List<StateObject> GetObjects(ulong state)
    {
        List<StateObject> objects = new List<StateObject>();
        ulong objectIndex = 4;
        for (int i = 0; i < _stateObjects.Values.Count; i++ )
        {
            if ((state & objectIndex) == objectIndex)
            {
                objects.Add(_stateObjects[objectIndex]);
            }
            objectIndex *= 2;
        }
        return objects;
    }
	public StateObject GetSelectedObject () {
		return _selectedObject;
	}

	public void SetSelectedObject (StateObject stateObject) {
		foreach (StateObject stateObjectVal in _stateObjects.Values) {
			stateObjectVal.Deselect();
		}
		stateObject.Select ();
		_selectedObject = stateObject;
	}

	public bool ChangeStateOfObject (StateObject stateObject, float x, float y, float z, float w) {
		bool successfulChange = true;
		float newX = _globalState [stateObject.gameObject.name].x + x;
		float newY = _globalState [stateObject.gameObject.name].y + y;
		float newZ = _globalState [stateObject.gameObject.name].z + z;
		float newW = _globalState [stateObject.gameObject.name].w + w;
		if (newX < 0 || newX < 0 || newX < 0 || newX < 0 ||
		    newX > 10 || newX > 10 || newX > 10 || newX > 10) {
			successfulChange = false;
		}
		if (newX < 0) newX = 0;
		if (newX > 10) newX = 10;
		if (newY < 0) newY = 0;
		if (newY > 10) newY = 10;
		if (newZ < 0) newZ = 0;
		if (newZ > 10) newZ = 10;
		if (newW < 0) newW = 0;
		if (newW > 10) newW = 10;
		if (_globalState [stateObject.gameObject.name].x < 0.0f) newX = -1;
		if (_globalState [stateObject.gameObject.name].y < 0.0f) newY = -1;
		if (_globalState [stateObject.gameObject.name].z < 0.0f) newZ = -1;
		if (_globalState [stateObject.gameObject.name].w < 0.0f) newW = -1;
		_globalState [stateObject.gameObject.name] = new Vector4 (newX, newY, newZ, newW);
		stateObject.emotionalState = new Vector4 (newX, newY, newZ, newW);
		return successfulChange;
	}

	public string StateToString (Dictionary<string,Vector4> state){
		string printState = "";
		int keyNumber = 0;
		foreach (string key in state.Keys) {
			printState += key + ": ";
			bool first = true;
			for (int i = 0; i < 4; i++) {
				if (state[key][i] >= 0) {
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
		_stateObjects[_objectIndex*4] = stateObject;
        stateObject.objectIndex = _objectIndex*4;
        _objectIndex*=2;
		_globalState.Add (stateObject.name, stateObject.emotionalState);
		if (roles.ContainsKey(stateObject.role)) {
			roles[stateObject.role].Add(stateObject);
		} else {
			List<StateObject> names = new List<StateObject>();
			names.Add (stateObject);
			roles[stateObject.role] = names;
		}
		numberOfMoves[stateObject.name] = new Dictionary<int, int>();
		for (int i = 0; i < 4; i++) {
			numberOfMoves[stateObject.name][i] = 0;
		}
	}
	public void StartStory () {
		_start = new StateNode(_globalState);
		Dictionary<string,Vector4> goalState = new Dictionary<string, Vector4>();
		foreach (string key in _globalState.Keys) {
			goalState[key] = new Vector4(-1,-1,-1,-1);
		}
		goalState[protagonist.gameObject.name] = new Vector4(5,-1,-1,-1);
		_goal = new StateNode(goalState);
		UpdateNeighbors();
		_plan = _aStar.FindPath (_start, _goal);
		_triedPlan = true;
		_failedPath = false;
		if (_plan == null) {
			_failedPath = true;
			clearPath();
		}
        PlayNextAction();
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
