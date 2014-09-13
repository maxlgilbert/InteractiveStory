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
		/*StateNode currState = curr as StateNode;
		if (currState.happy) {
			return StateStory.Instance.sad;
		} else {
			return StateStory.Instance.happy;
		}*/
		return null;
	}
}
