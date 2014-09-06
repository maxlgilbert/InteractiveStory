using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarAction : MonoBehaviour {
	public virtual string GetActionText() {
		return gameObject.name;
	}

	public virtual AStarNode TryAction(AStarNode curr) {
		return null;
	}
}
