using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : StateObject
{
    public bool open;
    public int roomOne;
    public int roomTwo;

    void Awake()
    {
        state = new SmartState();
        int openOrClosed = 0;
        if (open) openOrClosed = 1;
        state.AddState("Open", openOrClosed);
    }

    void Start()
    {
    }

    void OnMouseDown()
    {
        BaseStart();
        StateStory.Instance.fixedRooms[roomOne].AddDoor(this);
        StateStory.Instance.fixedRooms[roomTwo].AddDoor(this);

    }

    public static bool CanGetThrough(string doorName, Dictionary<string, SmartState> globalState)
    {
        if (globalState[doorName].GetValue("Open") == 1.0f)
        {
            return true;
        }
        return false;
    }
}
