using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateAction : AStarAction {
	void Start () {
		StateStory.Instance.actions.Add(this);
		identification = gameObject.name;
	}
	public override List<AStarNode> TryAction (AStarNode curr)
	{
		return null;
	}

	public virtual string ToString () {
		return gameObject.name;
	}
}
