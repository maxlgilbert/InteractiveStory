using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FixedRoom : FixedObject {

	public List<int> adjacentRooms;
    private List<Door> _doors;

    void Awake()
    {
        _doors = new List<Door>();
    }
	// Use this for initialization
	void Start () {
		StateStory.Instance.AddFixedRoom(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool IsAdjacentTo (int otherRoom) {
		if (otherRoom == this.roomNumber) {
			return true;
		} else {
			return adjacentRooms.Contains(otherRoom);
		}
	}

    public void AddDoor(Door door)
    {
        _doors.Add(door);
    }

    public bool RoomsConnected(int otherRoom, Dictionary<string, SmartState> globalState, List<int> visited = null)
    {
        //FixedRoom fixedRoom2 = StateStory.Instance.fixedRooms[room2];
        if (visited == null)
        {
            visited = new List<int>();
            visited.Add(this.roomNumber);
        }
        else
        {
            visited = new List<int>(visited);
        }
        for (int i = 0; i < _doors.Count; i++)
        {
            if (Door.CanGetThrough(_doors[i].gameObject.name, globalState))
            {
               // Debug.LogError("Got through a door");
                int adjacentRoomNumber = _doors[i].roomOne;
                if (adjacentRoomNumber == this.roomNumber)
                {
                    adjacentRoomNumber = _doors[i].roomTwo;
                }
                if (!visited.Contains(adjacentRoomNumber))
                {
                    if (adjacentRoomNumber == otherRoom)
                    {
                        return true;
                    }
                    else
                    {
                        visited.Add(adjacentRoomNumber);
                        bool recursion = RoomsConnected(otherRoom, globalState, visited);
                        if (recursion)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}
