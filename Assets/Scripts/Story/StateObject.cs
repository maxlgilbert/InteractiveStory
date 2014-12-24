using UnityEngine;
using System.Collections;
public enum Role {
	Protagonist,
	Character
}

public class StateObject : MonoBehaviour {
	public Vector4 emotionalState;

	public Role role;

    public float speed;

    public ulong objectIndex;
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
}
