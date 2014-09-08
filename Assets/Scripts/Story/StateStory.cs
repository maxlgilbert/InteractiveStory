using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateStory : MonoBehaviour {

	public int maxDepth;
	public StateNode start;
	public StateNode goal;
	public StateNode statePrefab;
	private AStar _aStar;

	public List<AStarAction> actions;

	private static StateStory instance;
	
	public static StateStory Instance
	{
		get 
		{
			return instance;
		}
	}
	void Awake() {
		instance = this;
	}
	// Use this for initialization
	void Start () {
		_aStar = new AStar(maxDepth);
	}

	public void UpdateNeighbors () {
		start.UpdateNeighbors();
		goal.UpdateNeighbors();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			foreach (AStarNode node in _aStar.FindPath (start, goal)) {
				StateNode happyState = node as StateNode;
				if (happyState.parentAction != null) {
					Debug.LogError(happyState.parentAction.GetActionText());
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			clearPath();
			UpdateNeighbors();
		}
	}
	
	
	public void clearPath () {
		start.clear();
		goal.clear();
		_aStar.Reset();
		
	}
}
