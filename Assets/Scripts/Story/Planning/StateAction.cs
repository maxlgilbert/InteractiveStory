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
        //StateStory.Instance.actions[this.actionIndex] = this;
        StateStory.Instance.AddAction(this);
    }

	public override string ToString () {
		return gameObject.name;
	}

    public delegate void ActionCompletedHandler();
    public event ActionCompletedHandler ActionCompleted;
    protected virtual void OnActionCompleted()
    {
        if (ActionCompleted != null)
        {
            ActionCompleted();
        }
    }
    public virtual void EnactAction(List<StateObject> Actors, List<StateObject> Objects)
    {
        OnActionCompleted();
    }
}
