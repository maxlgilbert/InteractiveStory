using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateStory : MonoBehaviour {

	public int maxDepth;
	public StateNode node;
	public StateNode sad;
	public StateNode happy;
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
		node.UpdateNeighbors();
		sad.UpdateNeighbors();
		happy.UpdateNeighbors();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			foreach (AStarNode node in _aStar.FindPath (sad, happy)) {
				StateNode happyState = node as StateNode;
				if (happyState.happy) {
					Debug.LogError("Now I am happy.");
				} else {
					Debug.LogError("I am sad.");
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			clearPath();
			UpdateNeighbors();
		}
	}
	
	
	public void clearPath () {
		sad.clear();
		happy.clear();
		_aStar.Reset();
		
	}
}
