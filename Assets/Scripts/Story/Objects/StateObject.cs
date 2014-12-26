using UnityEngine;
using System.Collections;
public enum Role {
	Protagonist,
	Character
}

public class StateObject : StoryObject {
    public SmartState state;

	public Role role;

    public float speed;

    public bool moveable;

    public ulong objectIndex;

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
        StateStory.Instance.AddStateObject(this);
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
        int layerMask = 1 << 9;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            gameObject.transform.position = hit.point;
            StoryObject receiver = hit.collider.gameObject.GetComponent<StoryObject>();
            ChangeState("Room", receiver.roomNumber);
            OnStateChanged();
        }
        else
        {
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
}
