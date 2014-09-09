using UnityEngine;
using System.Collections;

public class RadioButton : MonoBehaviour {
	
	private TextMesh _text;
	private RadioButtonPanel _panel;
	public string text;
	public int index;
	// Use this for initialization
	void Start () {
		_text = gameObject.GetComponentInChildren<TextMesh>();
		_panel = gameObject.GetComponentInParent<RadioButtonPanel> ();
		_text.text = text;
		gameObject.renderer.material = _panel.deselected;
		_panel.AddButton (this);
		if (index == 0) {
			_panel.SelectButton(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {
		_panel.SelectButton (this);
	}

	public void SelectButton () {
		StateStory.Instance.selectedState = index;
		gameObject.renderer.material = _panel.selected;

	}

	public void DeselectButton () {
		gameObject.renderer.material = _panel.deselected;

	}
}
