using System.Collections.Generic;

class Room
{
	// Private fields

	private Inventory chest;
	private string description;
	private bool isLocked;
	private bool isNurgleLocked;
	private Dictionary<string, Room> exits; // stores exits of this room.

	public Enemy enemy;

	// Create a room described "description". Initially, it has no exits.
	// "description" is something like "in a kitchen" or "in a court yard".
	public Inventory Chest
	{
		get { return chest; }
	}

	public Room(string desc)
	{
		description = desc;
		exits = new Dictionary<string, Room>();
		chest = new Inventory(10000);
		isLocked = false;
	}

	public void AddLock()
	{
		isLocked = true;
	}

	public void RemoveLock()
	{
		isLocked = false;
	}

	public void AddNurgleLock()
	{
		isNurgleLocked = true;
	}

	public void RemoveNurgleLock()
	{
		isNurgleLocked = false;
	}
	
	public bool GetLock()
	{
		return isLocked;
	}

	public bool GetNurgleLock()
	{
		return isNurgleLocked;
	}

	// Define an exit for this room.
	public void AddExit(string direction, Room neighbor)
	{
		exits.Add(direction, neighbor);
	}

	// public void AddItem(string description, Item heft)
	// {
	// 	Item.Add(description, heft);
	// }

	// Return the description of the room.
	public string GetShortDescription()
	{
		return description;
	}

	// Return a long description of this room, in the form:
	//     You are in the kitchen.edkit = new Item(40, "Medkit");
		// And add them to the Rooms
	//     Exits: north, west
	public string GetLongDescription()
	{
		string str = "You are ";
		str += description;
		str += ".\n";
		str += GetExitString();
		str += "\n";
		return str;
	}

	// Return the room that is reached if we go from this room in direction
	// "direction". If there is no room in that direction, return null.
	public Room GetExit(string direction)
	{
		if (exits.ContainsKey(direction))
		{
			return exits[direction];
		}
		return null;
	}

	// Return a string describing the room's exits, for example
	// "Exits: north, west".
	private string GetExitString()
	{
		string str = "Exits: ";
		str += String.Join(", ", exits.Keys);

		return str;
	}

    public static implicit operator Room(string v)
    {
        throw new NotImplementedException();
    }
	public void addEnemy(Enemy enemy)
	{
		this.enemy = enemy;
	}
}
