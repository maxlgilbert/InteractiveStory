using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateStory : MonoBehaviour {

	public int maxDepth;
	[HideInInspector] public StateCharacter protagonist;
	private StateObject _selectedObject;
    private Dictionary<ulong, StateObject> _stateObjects;
    private List<StateObject> _possibleStateObjects;
	public Dictionary<int,FixedRoom> fixedRooms;
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

    public Dictionary<ulong, AStarAction> actions;
    public List<StateAction> possibleActions;
    public StateAction selectedAction;
    private StateAction _currentAction;
    private ulong _actionIndex = 0;
    public int maxActions = 4;
    public ulong maxPossibleActions = 16;

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

    public GameObject intersectionPlane;

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
        possibleActions = new List<StateAction>();
        _possibleStateObjects = new List<StateObject>();
		fixedRooms = new Dictionary<int, FixedRoom>();

		
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
        return  actions[(maxPossibleActions-1) & state] as StateAction;
    }

    public void AddPossibleAction(StateAction stateAction)
    {
        stateAction.actionIndex = _actionIndex;
        stateAction.ActionCompleted += PlayNextAction;
        _actionIndex++;
    }
    public bool AddAction(StateAction stateAction)
    {
        if (actions.Keys.Count < maxActions && !actions.ContainsKey(stateAction.actionIndex))
        {
            actions[stateAction.actionIndex] = stateAction;
            return true;
        }
        return false;
    }

    public bool RemoveAction(StateAction stateAction)
    {
        if (actions.ContainsKey(stateAction.actionIndex))
        {
            actions.Remove(stateAction.actionIndex);
            return true;
        }
        return false;
    }
    public void PlayNextAction()
    {
        // IF next action can play, play
        // Else replan
        //Debug.LogError("Play next step");
        if (!_failedPath && _currentPlanStep < _plan.Count)
        {
           // Debug.LogError(_currentPlanStep);
            AStarNode node = _plan[_currentPlanStep];
            StateNode happyState = node as StateNode;
            _currentPlanStep++;
            if (happyState.actionID != ulong.MaxValue)
            {
                StateAction stateAction = GetAction(happyState.actionID);
                List<StateObject> Actors = GetObjects(happyState.actionID);
                List<StateObject> Objects = new List<StateObject>();
                stateAction.EnactAction(Actors, Objects);
            }
            else
            {
                PlayNextAction();
            }
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
						//printState += StateToString( happyState.globalState);
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
            string returnString = "";
            /*returnString += "Adjust the state:\n";
			returnString += "Moves available: " + this.movesLeft + "\n";
			returnString += "Current State:\n";
			returnString += StateToString(_globalState);*/
			//returnString += "Selected object: " + _selectedObject.gameObject.name;
            if (_selectedObject != null)
            {
                returnString += _selectedObject.gameObject.name + ": " + _selectedObject.state.ToString();
            }
			return returnString;
		}
	}
	
	public string actionListText {
		get {
			string returnString = "";
            returnString += actions.Keys.Count + " out of " + maxActions + " actions allowed\n";
            if (selectedAction != null)
            {
                returnString += selectedAction.ToString();
            }
            else if (possibleActions.Count != 0)
            {
                returnString += possibleActions[0].ToString();
            }
			return returnString;
		}
	}

    public List<StateObject> GetObjects(ulong state)
    {
        List<StateObject> objects = new List<StateObject>();
        ulong objectIndex = maxPossibleActions;
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
		//stateObject.emotionalState = new Vector4 (newX, newY, newZ, newW);
		stateObject.ChangeState("Joy",newX);
		stateObject.ChangeState("Anger",newY);
		stateObject.ChangeState("Fear",newZ);
		stateObject.ChangeState("Trust",newW);
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
	public void AddFixedRoom (FixedRoom fixedRoom) { // TODO what if not only rooms lol
		fixedRooms[fixedRoom.roomNumber] =  fixedRoom;
	}

    public void AddPossibleObject(StateObject stateObject)
    {
        _possibleStateObjects.Add(stateObject);
        stateObject.objectIndex = _objectIndex * maxPossibleActions;
        _objectIndex *= 2;
        if (stateObject.role == Role.Protagonist)
        {
            protagonist = stateObject as StateCharacter;
        }
    }

    public void AddStateObject(StateObject stateObject)
    {
        _stateObjects[stateObject.objectIndex] = stateObject;
        StateCharacter character = stateObject as StateCharacter;
        if (character != null)
        {
            _globalState.Add(character.name, character.emotionalState);
        }
        if (roles.ContainsKey(stateObject.role))
        {
            roles[stateObject.role].Add(stateObject);
        }
        else
        {
            List<StateObject> names = new List<StateObject>();
            names.Add(stateObject);
            roles[stateObject.role] = names;
        }
        numberOfMoves[stateObject.name] = new Dictionary<int, int>();
        for (int i = 0; i < 4; i++)
        {
            numberOfMoves[stateObject.name][i] = 0;
        }
		
	}
    public void StartStory()
    {
        clearPath();
        Dictionary<string, SmartState> startState = new Dictionary<string, SmartState>();
		foreach (StateObject stateObject in _stateObjects.Values) {
			startState[stateObject.gameObject.name] = new SmartState(stateObject.state);
            //Debug.LogError(stateObject.gameObject.name);
            //Debug.LogError(stateObject.state.ToString());
		}
        _start = new StateNode(startState);

        Dictionary<string,SmartState> goalState = new Dictionary<string,SmartState>();
		goalState[protagonist.gameObject.name] = new SmartState();
		goalState[protagonist.gameObject.name].SetState("Joy",5);
		_goal = new StateNode(goalState);

		UpdateNeighbors();
		_plan = _aStar.FindPath (_start, _goal);
		_triedPlan = true;
		_failedPath = false;
		if (_plan == null) {
			_failedPath = true;
			clearPath();
        }
        _currentPlanStep = 0;
        PlayNextAction();
	}
	
	public void UpdateNeighbors () {
		_start.UpdateNeighbors();
		_goal.UpdateNeighbors();
		
	}

	
	
	public void clearPath () {
        if (_start != null)
        {
            _start.clear();
            _goal.clear();
            _aStar.Reset();
            _plan = new List<AStarNode>();
        }
		
	}
}
