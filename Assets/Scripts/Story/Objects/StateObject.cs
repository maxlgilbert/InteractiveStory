using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum Role {
	Protagonist,
	Character,
    Guard,
    Gun,
    Radio,
    Artifact,
    Door
}

public class StateObject : StoryObject {
    public SmartState state;

	public Role role;

    public float speed;

    public bool moveable;

    public ulong objectIndex;

    public bool InScene;

    public bool stuckAtZero;

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
    public virtual void OnNothingChanged()
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

    public void MoveToWithin(Vector3 newPosition, float stoppingRadius, StateAction.ActionCompletedHandler actionCompleted, List<int> roomsVisited = null)
    {
        int layerMask = 1 << 9;
       // Ray ray =  Camera.main.ScreenPointToRay(newPosition);
        RaycastHit hit;
        StoryObject receiver = null;
        Vector3 rayOrigin = newPosition;
        rayOrigin.y += 20;
        if (Physics.Raycast(rayOrigin, new Vector3(0,-1,0),out hit, 100, layerMask))
        {
            receiver = hit.collider.gameObject.GetComponent<StoryObject>();
        }
        if (receiver != null)
        {
            FixedRoom targetRoom = StateStory.Instance.fixedRooms[receiver.roomNumber];
            if (targetRoom.roomNumber != this.roomNumber)
            {
                FixedRoom currentRoom = StateStory.Instance.fixedRooms[this.roomNumber];
                List<Door> doors = currentRoom.FindPath(targetRoom.roomNumber);
                StateAction.ActionCompletedHandler finalAction = () =>
                {
                    StartCoroutine(MoveToAtSpeed(newPosition, stoppingRadius, this.speed, actionCompleted));
                    this.roomNumber = receiver.roomNumber;
                };
                MoveAlongDoors(doors, finalAction);


                    /*
                for (int i = 0; i < doors.Count; i++)
                {
                    Door door = doors[i];
                    int nextRoomNumber = door.roomOne;
                    if (nextRoomNumber == this.roomNumber) nextRoomNumber = door.roomTwo;
                    if (!roomsVisited.Contains(nextRoomNumber)) //nextRoomNumber has not been visited
                    {
                        Debug.LogError(4);
                        StateAction.ActionCompletedHandler tryMovingAgain = () =>
                        {
                            this.roomNumber = nextRoomNumber;
                            MoveToWithin(newPosition, stoppingRadius, actionCompleted,roomsVisited);
                        };
                        Vector3 intermediatePosition = door.transform.position;
                        intermediatePosition.y = 0;
                        StartCoroutine(MoveToAtSpeed(intermediatePosition, 1.0f, this.speed, tryMovingAgain));
                        break;
                    }
                }*/
            }
            else
            {
                StartCoroutine(MoveToAtSpeed(newPosition, stoppingRadius, this.speed, actionCompleted));
            }
        }
        else
        {
            Debug.LogError("oops did this happen");
            //StartCoroutine(MoveToAtSpeed(newPosition, stoppingRadius, this.speed, actionCompleted));
        }
    }

    public void MoveAlongDoors(List<Door> doorsToMoveTo, StateAction.ActionCompletedHandler actionCompleted)
    {
        if (doorsToMoveTo.Count > 0)
        {
            Door nextDoor = doorsToMoveTo[0];
            doorsToMoveTo.Remove(nextDoor);
            Vector3 nextPosition = nextDoor.gameObject.transform.position;
            nextPosition.y = 0;
            StateAction.ActionCompletedHandler nextAction = () =>
            {
                MoveAlongDoors(doorsToMoveTo, actionCompleted);
            };
            StartCoroutine(MoveToAtSpeed(nextPosition, 1.0f, this.speed, nextAction));
        }
        else
        {
            actionCompleted();
        }
    }

    private IEnumerator MoveToAtSpeed(Vector3 newPosition, float stoppingRadius, float speed, StateAction.ActionCompletedHandler actionCompleted)
    {
        if (stuckAtZero)
        {
            //newPosition.y = 0;
        }
        Vector3 direction = newPosition - gameObject.transform.position;
        while (Mathf.Abs(Vector3.Distance(gameObject.transform.position, newPosition)) > stoppingRadius)
        {
            Vector3 nextPosition = gameObject.transform.position + speed * direction / 30.0f;
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
