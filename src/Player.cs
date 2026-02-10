using System.Runtime.InteropServices;

class Player
{
    //fields
    private Inventory backpack;
    public int health;
    // auto property
    private List<string> craftItem = new List<string>();
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

    public Item Place(string itemName)
    {
        // A temp item object
        Item temp = null;

        if(backpack.getItems().ContainsKey(itemName))
        {
            // Item exists in Inventory?
            // Secure item
            temp = backpack.getItems()[itemName];

            // Remove from dictionary
            backpack.getItems().Remove(itemName);
        }

        // Return saved item
        return temp;
    }

    // A method that allows you to take damage
    // Can later be inplemented for on a hit taken /Maybe with random int
    public void Damage()
	{
		health -=10;
	}

    // Heals the player but only if u can heal with a amount of hp u choose
    // Else print feedback
    public void Heal(string hptotstr)
	{
        if (hptotstr != null)
        {
            int hptot=Int32.Parse(hptotstr);
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
        else
        {
            Console.WriteLine("Please add a valid number...");
        }
	}

    public void damageNum(string hptotstr)
	{
        if (hptotstr != null)
        {
            int hptot=Int32.Parse(hptotstr);
            if (health >= hptot)
            {
                health -= hptot;
                Console.WriteLine($"You took {hptot} damage! Your health is now: {health}HP");
            }
            else
            {
                Console.WriteLine("That would kill u.");
            }
        }
        else
        {
            Console.WriteLine("Please add a valid number...");
        }
	}

    // If the player is low show a message
	// If you are very low show a diffrent message
	public void LowHp()
	{
		if (health <= 40 && health >= 30)
		{
			Console.WriteLine($"U feel hurt.");
		}
		else if(health <= 20)
		{
			Console.WriteLine($"U feel miserable. U should heal!");
		}
	}

    // Shows the players HP
	public void SeeHealth()
    {
        Console.WriteLine($"Your health is: {health}HP\n");
    }
    
    // Allows the player to see how much they are carrying and the space left
	public void checkWeight(string weight)
	{
		Console.Write("Total used weight is: ");
		Console.WriteLine(getBackpack().TotalWeight());
		Console.Write("U have: ");
		Console.WriteLine($"{getBackpack().FreeWeight()} weight left\n");
	}

    // Kill urself usefull to test on death triggers
    public void Sepuccu()
	{
		health = 0;
	}

    	    // Allows u to take a item from a room
    public bool TakeFromChest(string itemName)
    {
		Item testtemp = CurrentRoom.Chest.Peek(itemName);
        if (testtemp != null)
        {
            // Remove itemName from chest and save it
            Item item = CurrentRoom.Chest.Get(itemName);

            // Add the item we took to the backpack
            getBackpack().Put(itemName, item);

            // Return true
            return true;
        }

        // If empty return false
        return false;
    }

    	// Allows you to add items to a room
	public bool PutInChest(string itemName)
    {
        if (getBackpack() != null)
        {
            // Remove itemName from backpack and save it
            Item item = Place(itemName);

            // Add the item we took to the inventory
            CurrentRoom.Chest.Put(itemName, item);

            // Return true
            return true;
        }

        // If empty return false
        return false;
    }


    // Uses a item
    public void use(Command command)
    {
        if(!command.HasSecondWord())
        {
            Console.WriteLine("What do u want to use.\n");
        }

        string itemName = command.SecondWord;
        Item item = backpack.Peek(itemName);
        backpack.Get(itemName);
        if (item == null)
        {
            Console.WriteLine("U don't have that as a item.\n");
            return;
        }
        
        // Checks what item it is then uses it
        if (itemName == "bandage")
        {
            if (health <= 100-20)
            {
                health += 20;
                Console.WriteLine("U used the bandage +20HP");
                Console.WriteLine($"You healed! Your health is now: {health}HP\n");
            }
            else
            {
                Console.WriteLine("You aren't all that injured are you? \n");
            }
            Console.WriteLine(CurrentRoom.GetLongDescription());
        }
        
        if (itemName == "medkit")
        {
            if (health <= 100-50)
            {
                Console.WriteLine($"U used the medkit +{100-health}HP");
                health = 100;
                Console.WriteLine($"You healed! Your health is now: {health}HP\n");
            }
            else
            {
                Console.WriteLine("You aren't all that injured are you? \n");
            }
            Console.WriteLine(CurrentRoom.GetLongDescription());
        }
    }

    public void Craft(Command command)
    {
        craftItem.Add(command.SecondWord);
        craftItem.Add(command.ThirdWord);
        
        if(!command.HasSecondWord() || !command.HasThirdWord())
        {
            Console.WriteLine("What do u want to use in crafting.\n");
        }
    }
}