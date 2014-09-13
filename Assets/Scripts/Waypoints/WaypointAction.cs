using UnityEngine;
using System.Collections;

public enum Direction {
	North,
	NorthEast,
	East,
	SouthEast,
	South,
	West,
	SouthWest,
	NorthWest
}
public class WaypointAction : AStarAction {
	void Start () {
		WaypointGrid.Instance.actions.Add(this);
	}

	public Direction direction;

	/*public override AStarNode TryAction (AStarNode curr)
	{
		WaypointNode node = curr as WaypointNode;
		int i = node.i;
		int j = node.j;
		bool iMinusOne = (i - 1 >= 0);
		bool jMinusOne = (j - 1 >= 0);
		bool iPlusOne = (i + 1 < WaypointGrid.Instance.width);
		bool jPlusOne = (j + 1 < WaypointGrid.Instance.height);
		if (direction == Direction.SouthWest && iMinusOne && jMinusOne) {
			return WaypointGrid.Instance.allNodes[i-1,j-1];
		}
		if (direction == Direction.West && iMinusOne) {
			return WaypointGrid.Instance.allNodes[i-1,j];
		}
		if (direction == Direction.NorthWest && iMinusOne && jPlusOne) {
			return WaypointGrid.Instance.allNodes[i-1,j+1];
		}
		if (direction == Direction.North && jPlusOne) {
			return WaypointGrid.Instance.allNodes[i,j+1];
		}
		if (direction == Direction.NorthEast && iPlusOne && jPlusOne) {
			return WaypointGrid.Instance.allNodes[i+1,j+1];
		}
		if (direction == Direction.East && iPlusOne) {
			return WaypointGrid.Instance.allNodes[i+1,j];
		}
		if (direction == Direction.SouthEast && iPlusOne && jMinusOne) {
			return WaypointGrid.Instance.allNodes[i+1,j-1];
		}
		if (direction == Direction.South && jMinusOne) {
			return WaypointGrid.Instance.allNodes[i,j-1];
		}
		return null;
		
	}*/
}
