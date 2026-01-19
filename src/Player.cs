using System.Runtime.InteropServices;

class Player
{
    //fields
    private Inventory backpack;
    public int health;
    // auto property
    public Room CurrentRoom { get; set; }
    // constructor

    // Makes a player with HP and a Inventory
    public Player()
    {
        CurrentRoom = null;
        health = 100;
        backpack = new Inventory(100);
    }
    // Methods

    public Inventory getBackpack()
    {
        return backpack;
    }

    // Allows u to take a item from a room
    public bool TakeFromChest(string itemName)
    {
        if (CurrentRoom.Chest != null)
        {
            // Remove itemName from chest and save it
            Item item = CurrentRoom.Chest.Get(itemName);

            // Add the item we took to the backpack
            backpack.Put(itemName, item);

            // Return true
            return true;
        }

        // If empty return false
        return false;
    }

    // A method that allows you to take damage
    // Can later be inplemented for on a hit taken /Maybe with random int
    public void Damage()
	{
		health -=10;
	}

    // Heals the player but only if u can heal 20 hp or more
    // Else print feedback
    public void Heal(int hptot)
	{
		if (health <= 100-hptot)
		{
			health += hptot;
			Console.WriteLine($"You healed! Your health is now: {health}HP");
		}
		else
		{
			Console.WriteLine("You aren't all that injured are you?");
		}
	}

    // Kill urself usefull to test on death triggers
    public void Sepuccu()
	{
		health = 0;
	}
}
