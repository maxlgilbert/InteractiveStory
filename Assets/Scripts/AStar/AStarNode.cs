using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base class for a node that AStar can search paths along.
/// Inherit and overrid functions and fields to implement
/// </summary>
public class AStarNode : MonoBehaviour {

	/// <summary>
	/// Whether or not this node is free.
	/// </summary>
	public bool traversable = true;

	/// <summary>
	/// Minimum currently projected distance travelled at this node.
	/// </summary>
	public float distanceTraveled = .0f;

	/// <summary>
	/// Total AStar score.
	/// </summary>
	public float fValue = 0.0f;
	
	/// <summary>
	/// The parent node.
	/// </summary>
	public AStarNode parent;

	/// <summary>
	/// Whether AStar has seen this node or not.
	/// </summary>
	public bool visited = false;

	public List<AStarAction> actions;

	protected List<AStarNode> _neighbors;

	public void UpdateNeighbors () {
		_neighbors = new List<AStarNode>();
		if (actions != null) {
			foreach (AStarAction action in actions) {
				AStarNode possilbeNeighbor = action.TryAction(this);
				if (possilbeNeighbor != null) {
					_neighbors.Add(possilbeNeighbor);
				}
			}
		}
	}
	
	/// <summary>
	/// Function to get neighbors.
	/// </summary>
	/// <returns>The neighbors.</returns>
	public List<AStarNode> GetNeighbors (){
		return _neighbors;
	}

	/// <summary>
	/// Determines whether the specified <see cref="AStarNode"/> is equal to the current <see cref="AStarNode"/>.
	/// </summary>
	/// <param name="other">The <see cref="AStarNode"/> to compare with the current <see cref="AStarNode"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="AStarNode"/> is equal to the current <see cref="AStarNode"/>; otherwise, <c>false</c>.</returns>
	public virtual bool Equals (AStarNode other) {
		return (this.gameObject.transform.position == other.gameObject.transform.position);
	}

	/// <summary>
	/// Distance from this node to another.
	/// </summary>
	/// <param name="other">Other.</param>
	public virtual float distance (AStarNode other) {
		return Vector3.Distance(this.gameObject.transform.position,other.gameObject.transform.position);
	}

	/// <summary>
	/// Estimated distance from this node to another.
	/// </summary>
	/// <param name="other">Other.</param>
	public virtual float estimate (AStarNode other) {
		return Vector3.Distance(this.gameObject.transform.position,other.gameObject.transform.position);
	}

	/// <summary>
	/// Resets this node.
	/// </summary>
	public virtual void clear() {
		distanceTraveled = 0.0f;
		fValue = 0.0f;
		parent = null;
		visited = false;
	}
}
