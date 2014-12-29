using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AncientArtifact : StateObject
{
    public bool activated;
    void Awake()
    {
        state = new SmartState();
        int active = 0;
        if (activated) active = 1;
        state.AddState(AncientArtifact.StateName, active);
    }
    public static string StateName = "Activated";
}
