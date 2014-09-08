using UnityEngine;
using System.Collections;

public class StoryBoard : MonoBehaviour {
	public TextMesh text;
	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = StateStory.Instance.storyBoardText;
	}
}
