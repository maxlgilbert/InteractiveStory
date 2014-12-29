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
        BaseStart();
        Initialize delayedStart = () =>
        {
            StateStory.Instance.fixedRooms[roomOne].AddDoor(this);
            StateStory.Instance.fixedRooms[roomTwo].AddDoor(this);
        };
        StartCoroutine(DelayedStart(.1f, delayedStart));
    }
    public static bool IsGuarded(string doorName, Dictionary<string, SmartState> globalState)
    {
        List<StateObject> guards = StateStory.Instance.roles[Role.Guard];
        for (int i = 0; i < guards.Count; i++)
        {
            string guardName = guards[i].gameObject.name;
            int guardRoom = (int) globalState[guardName].GetValue("Room");
            Door door = StateStory.Instance.GetStateObject(doorName) as Door;
            if (guardRoom == door.roomOne || guardRoom == door.roomTwo)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsOpen(string doorName, Dictionary<string, SmartState> globalState)
    {
        if (globalState[doorName].GetValue("Open") == 1.0f)
        {
            return true;
        }
        return false;
    }
}
