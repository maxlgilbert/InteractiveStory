using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class StateAction : AStarAction {
	void Start () {
		//StateStory.Instance.actions.Add(this);
        identification = gameObject.name;
        StateStory.Instance.AddPossibleAction(this);
	}
	public override List<AStarNode> TryAction (AStarNode curr)
	{
		return null;
	}
    void OnMouseDown()
    {
        //StateStory.Instance.actions[this.actionIndex] = this;
        if (!StateStory.Instance.actions.ContainsValue(this))
        {
            if (StateStory.Instance.AddAction(this))
            {
                Select();
            }
        }
        else
        {
            if (StateStory.Instance.RemoveAction(this))
            {
                Deselect();
            }
        }
        StateStory.Instance.selectedAction = this;

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
    public virtual void EnactAction(string actionText)
    {
        OnActionCompleted();
    }

    public void Select()
    {
        gameObject.renderer.material = StateStory.Instance.green;
    }

    public void Deselect()
    {
        gameObject.renderer.material = StateStory.Instance.red;
    }
    
}
