using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;
	private Room winRoom;


    // Constructor
	// Makes the player object, parser object and room objects
    public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room carrion = new Room("in a fleshy place");
		Room narrow = new Room("in a hallway filled with flesh, it's so narrow where does it lead");
		Room corpse = new Room("in a open area, there is a corpse here. It smells");
		Room nurgle = new Room("in a room with a horrible smell. There is something here, it's green and slimey..");
		Room silence = new Room("in a room that is completely silent, this is unusual....");
		Room bile = new Room("in a pool of bile, u feel your skin crawl");
		Room rash = new Room("in a room filled with rashes, I feel like i'm gonna vomit");
		Room teeth = new Room("in a room filled with teeth, it makes me uneasy");

		// Initialise room exits
		carrion.AddExit("east", narrow);
		carrion.AddExit("south", nurgle);
		carrion.AddExit("west", corpse);

		narrow.AddExit("west", carrion);
		narrow.AddExit("up", teeth);

		corpse.AddExit("east", carrion);
		corpse.AddExit("south", bile);

		nurgle.AddExit("north", carrion);
		nurgle.AddExit("east", silence);

		silence.AddExit("west", nurgle);

		bile.AddExit("down", rash);
		bile.AddExit("north", corpse);
		bile.AddExit("east", narrow);

		rash.AddExit("up", bile);
		
		teeth.AddExit("down", narrow);

        // Create your Items here
		// Makes a item find more in Item.cs
        Item bandage = new Item(10, "bandage");
		Item medkit = new Item(40, "medkit");
		Item key = new Item(5, "key");
		Item nurgling = new Item(1000, "nurgling");

		// And add them to the Rooms
		carrion.Chest.Put("bandage",bandage);
		corpse.Chest.Put("medkit", medkit);
		bile.Chest.Put("key", key);
		nurgle.Chest.Put("nurgling", nurgling);


		// Start game carrion
		player.CurrentRoom = carrion;

		winRoom = teeth;
	}

	//  Main play routine. Loops until end of play.

	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);

			//checks if health is == 0 then stops
			if (player.health == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Game Over\n");
				finished = true;
			}



		}
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
    }	
	

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		// Description info at Room.cs
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if(command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...\nSee -help- for more info.");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				player.Damage();
				player.LowHp();
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				Look();
				break;
			case "health":
				player.SeeHealth();
				break;
			case "heal":
				player.Heal(command.SecondWord);
				break;
			case "die":
				player.Sepuccu();
				break;
			case "take":
				TakeFromChest(command.SecondWord);
				break;
			case "place":
				PutInChest(command.SecondWord);
				break;
			case "backpack":
				checkInventory(command.SecondWord);
				checkWeight(command.SecondWord);
				break;
		}

		if (player.CurrentRoom == winRoom)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("\nYou Won\n");
			wantToQuit = true;
		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################
	
	// Print out some help information.
	// Here we print the mission and a list of the command words.
	
	//See what is in the room and where you are
	private void Look()
	{
		if (player.CurrentRoom.Chest != null)
        {
			foreach ( var (_, item) in player.CurrentRoom.Chest.getItems())
			{
				Console.WriteLine("You found :");
				Console.Write(item.Description);
				Console.Write(" - ");
				Console.WriteLine(item.Weight);
				Console.WriteLine($"Type take {item.Description} to grab the item.\n");
			}
		}
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	// Shows the commands again
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	//See what you have in your inventory
	private void checkInventory(string items)
	{
		foreach( var (_, item) in player.getBackpack().getItems())
		{
			Console.Write(item.Description);
			Console.Write(" - ");
			Console.WriteLine(item.Weight);
		}
	}

	// Allows the player to see how much they are carrying and the space left
	private void checkWeight(string weight)
	{
		Console.Write("Total used weight is: ");
		Console.WriteLine(player.getBackpack().TotalWeight());
		Console.Write("U have: ");
		Console.WriteLine($"{player.getBackpack().FreeWeight()} weight left\n");
	}

	// Allows you to add items to a room
	private bool PutInChest(string itemName)
    {
        if (player.getBackpack() != null)
        {
            // Remove itemName from backpack and save it
            Item item = player.Place(itemName);

            // Add the item we took to the inventory
            player.CurrentRoom.Chest.Put(itemName, item);

            // Return true
            return true;
        }

        // If empty return false
        return false;
    }

	    // Allows u to take a item from a room
    private bool TakeFromChest(string itemName)
    {
        if (player.CurrentRoom.Chest != null)
        {
            // Remove itemName from chest and save it
            Item item = player.CurrentRoom.Chest.Get(itemName);

            // Add the item we took to the backpack
            player.getBackpack().Put(itemName, item);

            // Return true
            return true;
        }

        // If empty return false
        return false;
    }

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if(!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine($"There is no door to {direction}!\nSee -help- for more info.");
			return;
		}

		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}
}
