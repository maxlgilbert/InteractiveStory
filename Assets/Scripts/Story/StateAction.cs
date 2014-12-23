using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateAction : AStarAction {
	void Start () {
		//StateStory.Instance.actions.Add(this);
		identification = gameObject.name;
	}
	public override List<AStarNode> TryAction (AStarNode curr)
	{
		return null;
	}
    void OnMouseDown()
    {
        StateStory.Instance.actions.Add(this);
    }

	public virtual string ToString () {
		return gameObject.name;
	}

    public virtual void EnactAction(List<StateObject> Actors, List<StateObject> Objects)
    {

    }
}
