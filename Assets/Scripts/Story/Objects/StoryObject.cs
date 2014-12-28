using UnityEngine;
using System.Collections;

public class StoryObject : MonoBehaviour {
    public int roomNumber;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    protected delegate void Initialize();
    protected IEnumerator DelayedStart(float duration, Initialize delayedStart)
    {
        yield return new WaitForSeconds(duration);
        delayedStart();
    }
}
