using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {
    public int level;
    public TextMesh text;
    public bool start;
    // Use this for initialization
    void Start()
    {
        if (text == null)
        {
            text = gameObject.GetComponentInChildren<TextMesh>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!start)
        {
            if (StateStory.Instance.achievedGoal == 2)
            {
                renderer.enabled = true;
                text.text = "Advance to chapter " + (level);
            }
            else if (StateStory.Instance.achievedGoal == 1)
            {
                renderer.enabled = true;
                text.text = "Try chapter " + level + " again";
            }
            else
            {
                text.text = "";
            }
        }
    }
    void OnMouseDown()
    {
        if (start || StateStory.Instance.achievedGoal == 2)
        {
            audio.Play();
            Application.LoadLevel(level);
        }
        else
        {
            audio.Play();
            Application.LoadLevel(level-1);

        }
	}
}
