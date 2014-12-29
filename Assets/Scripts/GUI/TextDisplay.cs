using UnityEngine;
using System.Collections;

public enum DisplayType {
	Story,
	States,
	Actions,
    Objects
}
public class TextDisplay : MonoBehaviour {
	public TextMesh text;
	public DisplayType displayType;
	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        if (displayType == DisplayType.Story)
        {
            text.text = StateStory.Instance.storyBoardText;
        }
        else if (displayType == DisplayType.States)
        {
            text.text = StateStory.Instance.initialStateText;
        }
        else if (displayType == DisplayType.Actions)
        {
            text.text = StateStory.Instance.actionListText;
        }
        else if (displayType == DisplayType.Objects)
        {
            text.text = StateStory.Instance.objectText;
        }
	}
}
