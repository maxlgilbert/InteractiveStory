using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointNode : AStarNode {
	
	
	public Material green;
	public Material red;
	public int i;
	public int j;
	// Use this for initialization
	void Awake () {
		actions = WaypointGrid.Instance.actions;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// List of possible neighbor nodes.
	/// </summary>
	//public List<AStarNode> neighbors;


	/// <summary>
	/// Function to get neighbors.
	/// </summary>
	/// <returns>The neighbors.</returns>
	/*public override void UpdateNeighbors ()
	{
		base.UpdateNeighbors ();
	}

	public override List<AStarNode> GetNeighbors ()
	{
		return base.GetNeighbors ();
	}*/
	
	
	void OnMouseDown () {
		setTraversable();
	}
	
	public void setTraversable() {
		if (traversable) {
		//	gameObject.renderer.material = red;
		} else {
		//	gameObject.renderer.material = green;
		}
		traversable = !traversable;
	}
}
