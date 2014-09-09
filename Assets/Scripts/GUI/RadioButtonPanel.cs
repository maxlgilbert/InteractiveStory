using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadioButtonPanel : MonoBehaviour {
	public Material selected;
	public Material deselected;
	private List<RadioButton> _buttons;

	void Awake () {
		_buttons = new List<RadioButton> ();
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddButton (RadioButton button) {
		_buttons.Add (button);
	}

	public void SelectButton (RadioButton button) {
		for(int i = 0; i < _buttons.Count; i++) {
			_buttons[i].DeselectButton();
		}
		button.SelectButton ();
	}
}
