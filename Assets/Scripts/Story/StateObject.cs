using UnityEngine;
using System.Collections;
public enum Role {
	Protagonist,
	Character
}

public class StateObject : MonoBehaviour {
	public Vector4 emotionalState;

    public SmartState state;

	public Role role;

    public float speed;

    public ulong objectIndex;
    void Awake()
    {
        state = new SmartState();
		
		state.AddState("Joy", emotionalState.x);
		state.AddState("Anger", emotionalState.y);
		state.AddState("Fear", emotionalState.z);
		state.AddState("Trust", emotionalState.w);

    }

    //public ulong indexOffset;
	// Use this for initialization
	void Start () {
		StateStory.Instance.AddStateObject (this);
	}

	void OnMouseDown () {
		StateStory.Instance.SetSelectedObject(this);
	}
	
	// Update is called once per frame
	void Update () {
	
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
