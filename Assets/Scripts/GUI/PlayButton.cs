using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {

	void OnMouseDown () {
		StateStory.Instance.StartStory();
		audio.Play ();
	}
}
