using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointGrid : MonoBehaviour {
	public AStarNode start;
	public AStarNode goal;
	public int width;
	public int height;
	private LineRenderer _lineRenderer;
	private List<Vector3> _lineToDraw;
	public WaypointNode[,] allNodes;
	public WaypointNode node;
	public int maxDepth;
	public List<AStarAction> actions;

	private AStar _aStar;
	
	
	private static WaypointGrid instance;
	
	public static WaypointGrid Instance
	{
		get 
		{
			return instance;
		}
	}

	void Awake () {
		instance = this;
		/*actions = new List<AStarAction>();
		WaypointAction north = new WaypointAction();
		north.direction = Direction.North;
		actions.Add(north);
		WaypointAction northWest = new WaypointAction();
		northWest.direction = Direction.NorthWest;
		actions.Add(northWest);
		WaypointAction west = new WaypointAction();
		west.direction = Direction.West;
		actions.Add(west);
		WaypointAction southWest = new WaypointAction();
		southWest.direction = Direction.SouthWest;
		actions.Add(southWest);
		WaypointAction south = new WaypointAction();
		south.direction = Direction.South;
		actions.Add(south);
		WaypointAction southEast = new WaypointAction();
		southEast.direction = Direction.SouthEast;
		actions.Add(southEast);
		WaypointAction east = new WaypointAction();
		east.direction = Direction.East;
		actions.Add(east);
		WaypointAction northEast = new WaypointAction();
		northEast.direction = Direction.NorthEast;
		actions.Add(northEast);*/
		_lineToDraw = new List<Vector3>();
		_lineRenderer = gameObject.GetComponent<LineRenderer>();
		_aStar = new AStar(maxDepth);
	}

	// Use this for initialization
	void Start () {
		allNodes = new WaypointNode[width,height];
		Vector3 pt = new Vector3(-2.0f * (float)width/2.0f + 1.0f, 0.0f, -2.0f * (float)height/2.0f+1.0f);
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				WaypointNode newNode =  new WaypointNode();//Instantiate (node, pt, Quaternion.identity) as WaypointNode;
				newNode.i = i;
				newNode.j = j;
				allNodes[i,j] = newNode;
				if (i == 0 && j == 0) {
					start = newNode;
				}
				if (i == width - 1 && j == height -1) {
					goal = newNode;
				}
				pt.z += 2.0f;
			}
			pt.x += 2.0f;
			pt.z = -2.0f * (float)height/2.0f+1.0f;
		}
		/*for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				bool iMinusOne = (i - 1 >= 0);
				bool jMinusOne = (j - 1 >= 0);
				bool iPlusOne = (i + 1 < width);
				bool jPlusOne = (j + 1 < height);
				if (iMinusOne && jMinusOne) {
					allNodes[i,j].neighbors.Add(allNodes[i-1,j-1]);
				}
				if (iMinusOne) {
					allNodes[i,j].neighbors.Add(allNodes[i-1,j]);
				}
				if (iMinusOne && jPlusOne) {
					allNodes[i,j].neighbors.Add(allNodes[i-1,j+1]);
				}
				if (jPlusOne) {
					allNodes[i,j].neighbors.Add(allNodes[i,j+1]);
				}
				if (iPlusOne && jPlusOne) {
					allNodes[i,j].neighbors.Add(allNodes[i+1,j+1]);
				}
				if (iPlusOne) {
					allNodes[i,j].neighbors.Add(allNodes[i+1,j]);
				}
				if (iPlusOne && jMinusOne) {
					allNodes[i,j].neighbors.Add(allNodes[i+1,j-1]);
				}
				if (jMinusOne) {
					allNodes[i,j].neighbors.Add(allNodes[i,j-1]);
				}
			}
		}*/
	}

	public void UpdateNeighbors (){
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				allNodes[i,j].UpdateNeighbors();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		_lineRenderer.SetVertexCount(_lineToDraw.Count);
		while (i < _lineToDraw.Count) {
			_lineRenderer.SetPosition(i, _lineToDraw[i]);
			i++;
		}

		if (Input.GetKeyDown(KeyCode.W)) {
			foreach (AStarNode node in _aStar.FindPath (start, goal)) {
//				_lineToDraw.Add(node.gameObject.transform.position);
			}
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			clearPath ();
			UpdateNeighbors();
		}
	}

	
	public void clearPath () {
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				allNodes[i,j].clear();
			}
		}
		_lineToDraw = new List<Vector3>();
		_aStar.Reset();
		
	}
}
