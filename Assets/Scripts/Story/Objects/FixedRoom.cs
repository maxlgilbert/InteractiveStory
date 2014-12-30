using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FixedRoom : FixedObject {

	//public List<int> adjacentRooms;
    public List<Door> doors;

    void Awake()
    {
        doors = new List<Door>();
    }
	// Use this for initialization
	void Start () {
		StateStory.Instance.AddFixedRoom(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
/*
	public bool IsAdjacentTo (int otherRoom) {
		if (otherRoom == this.roomNumber) {
			return true;
		} else {
			return adjacentRooms.Contains(otherRoom);
		}
	}
    */
    public void AddDoor(Door door)
    {
        doors.Add(door);
    }
    public static List<FixedRoom> GetConnectedRooms(int room1, Dictionary<string, SmartState> globalState, List<FixedRoom> visited = null)
    {
        return visited;
    }
    public static bool RoomsConnected(int room1, int room2, Dictionary<string, SmartState> globalState, bool isGuard, List<int> visited = null)
    {
        //FixedRoom fixedRoom2 = StateStory.Instance.fixedRooms[room2];
        if (visited == null)
        {
            if (room1 == room2)
            {
                return true;
            }
            visited = new List<int>();
            visited.Add(room1);
        }
        else
        {
            visited = new List<int>(visited);
        }
        FixedRoom currentRoom = StateStory.Instance.fixedRooms[room1];
        for (int i = 0; i < currentRoom.doors.Count; i++)
        {
            if (isGuard || ( Door.IsOpen(currentRoom.doors[i].gameObject.name, globalState) && !Door.IsGuarded(currentRoom.doors[i].gameObject.name, globalState)))
            {
               // Debug.LogError("Got through a door");
                int adjacentRoomNumber = currentRoom.doors[i].roomOne;
                if (adjacentRoomNumber == visited[visited.Count-1])
                {
                    adjacentRoomNumber = currentRoom.doors[i].roomTwo;
                }
                if (!visited.Contains(adjacentRoomNumber))
                {
                    if (adjacentRoomNumber == room2)
                    {
                        return true;
                    }
                    else
                    {
                        visited.Add(adjacentRoomNumber);
                        bool recursion = RoomsConnected(adjacentRoomNumber, room2, globalState, isGuard, visited);
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

    public List<Door> FindPath(int otherRoom)
    {
        List<Door> doorPath = new List<Door>();
        Queue<RoomNode> roomQueue = new Queue<RoomNode>();
        List<int> visitedRooms = new List<int>();
        roomQueue.Enqueue(new RoomNode(this.roomNumber));
        visitedRooms.Add(this.roomNumber);
        RoomNode curr = null;
        while (roomQueue.Count > 0)
        {
            curr = roomQueue.Dequeue();
            if (curr.roomNumber == otherRoom)
            {
                break;
            }
            foreach (Door door in StateStory.Instance.fixedRooms[curr.roomNumber].doors)
            {
                int nextRoomNumber = door.roomOne;
                if (nextRoomNumber == curr.roomNumber) nextRoomNumber = door.roomTwo;
                RoomNode nextRoomNode = new RoomNode(nextRoomNumber);
                nextRoomNode.parentDoor = door;
                nextRoomNode.parentNode = curr;
                if (!visitedRooms.Contains(nextRoomNumber))
                {
                    visitedRooms.Add(nextRoomNumber);
                    roomQueue.Enqueue(nextRoomNode);
                }
            }


        }
        while (curr != null && curr.parentDoor != null)
        {
            doorPath.Add(curr.parentDoor);
            curr = curr.parentNode;
        }
        doorPath.Reverse();
        return doorPath;
    }
}

public class RoomNode
{
    public int roomNumber;
    public Door parentDoor;
    public RoomNode parentNode;
    public RoomNode(int number)
    {
        roomNumber = number;
    }


}
