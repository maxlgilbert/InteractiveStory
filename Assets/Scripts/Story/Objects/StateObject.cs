using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum Role {
	Protagonist,
	Character,
    Guard,
    Gun
}

public class StateObject : StoryObject {
    public SmartState state;

	public Role role;

    public float speed;

    public bool moveable;

    public ulong objectIndex;

    public bool InScene;

    private Vector3 _previousPosition;
    void Awake()
    {
        state = new SmartState();

    }

    //public ulong indexOffset;
	// Use this for initialization
	void Start () {
        BaseStart();
	}

    protected void BaseStart()
    {
        state.AddState("Room", roomNumber); //TODO Standardizes these strings somehow (static constant somewhere)
        StateStory.Instance.AddPossibleObject(this);
        if (InScene)
        {
            StateStory.Instance.AddStateObject(this);
        }
    }

	void OnMouseDown () {
		StateStory.Instance.SetSelectedObject(this);
        _previousPosition = gameObject.transform.position;
	}

    void OnMouseDrag()
    {
        if (moveable)
        {
            int layerMask = 1 << 8;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100,layerMask))
            {
                gameObject.transform.position = hit.point;
            }
        }
    }

    void OnMouseUp()
    {
        if (moveable)
        {
            int layerMask = 1 << 9;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //bool returnToPrevious = true;
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                gameObject.transform.position = hit.point;
                StoryObject receiver = hit.collider.gameObject.GetComponent<StoryObject>();
                if (receiver.roomNumber == -1)
                {
                    if (this.roomNumber != -1 && !this.InScene)
                    {
                        ChangeState("Room", receiver.roomNumber);
                        this.roomNumber = -1;
                        StateStory.Instance.RemoveStatebject(this);
                        return;
                    }
                    else if (this.roomNumber == -1)
                    {
                        return;
                    }
                }
                else if (this.roomNumber == -1)
                {
                    if (StateStory.Instance.AddStateObject(this))
                    {
                        ChangeState("Room", receiver.roomNumber);
                        //Debug.LogError(
                        OnStateChanged();
                        this.roomNumber = receiver.roomNumber;
                        return;
                    }
                }
                else
                {
                    ChangeState("Room", receiver.roomNumber);
                    //Debug.LogError(
                    OnStateChanged();
                    this.roomNumber = receiver.roomNumber;
                    return;
                }
            }
            StateAction.ActionCompletedHandler nothingChanged = () => OnNothingChanged();
            MoveToWithin(_previousPosition, 0.2f, nothingChanged);
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public event StateAction.ActionCompletedHandler StateChanged;
    protected virtual void OnNothingChanged()
    {

    }

    protected virtual void OnStateChanged()
    {
        if (StateChanged != null)
        {
            StateChanged();
        }
    }
	public void ChangeState (string field, float value) {
		state.SetState(field,value);
	}

	public void Select () {
		gameObject.renderer.material = StateStory.Instance.green;
	}

	public void Deselect () {
		gameObject.renderer.material = StateStory.Instance.red;
	}
    
    public void MoveToWithin(Vector3 newPosition, float stoppingRadius, StateAction.ActionCompletedHandler actionCompleted)
    {
        
        StartCoroutine(MoveToAtSpeed(newPosition, stoppingRadius, this.speed, actionCompleted));
    }

    private IEnumerator MoveToAtSpeed(Vector3 newPosition, float stoppingRadius, float speed, StateAction.ActionCompletedHandler actionCompleted)
    {
        Vector3 direction = newPosition - gameObject.transform.position;
        while (Vector3.Distance(gameObject.transform.position, newPosition) > stoppingRadius)
        {
            Vector3 nextPosition = gameObject.transform.position + speed*direction/30.0f;
            gameObject.transform.position = nextPosition;
            yield return new WaitForFixedUpdate();
        }
        actionCompleted();
    }

    public void WaitFor(float duration, StateAction.ActionCompletedHandler actionCompleted)
    {

        StartCoroutine(WaitForThen(duration, actionCompleted));
    }

    private IEnumerator WaitForThen(float duration, StateAction.ActionCompletedHandler actionCompleted)
    {
        yield return new WaitForSeconds(duration);
        actionCompleted();
    }

    public static void SetGlobalState(string stateToChange, string field, float value, Dictionary<string,SmartState> globalState)
    {
        globalState[stateToChange].SetState(field, value);
    }
}
