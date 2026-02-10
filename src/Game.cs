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
		teeth.AddLock();

        // Create your Items here
		// Makes a item find more in Item.cs
        Item bandage = new Item(10, "bandage");
		Item medkit = new Item(40, "medkit");
		Item key = new Item(5, "key");
		Item nurgling = new Item(1000, "nurgling");
		Item metalrod = new Item (30, "metalrod");
		Item metalplate = new Item (50, "metalplate");

		// And add them to the Rooms
		carrion.Chest.Put("bandage",bandage);
		carrion.Chest.Put("key", key);
		carrion.Chest.Put("medkit", medkit);
		corpse.Chest.Put("medkit", medkit);
		bile.Chest.Put("key", key);
		nurgle.Chest.Put("nurgling", nurgling);
		rash.Chest.Put("metalrod", metalrod);
		corpse.Chest.Put("metalplate", metalplate);
	


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

			// Checks if health is == 0 then stops and makes color red
			if (player.health == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Game Over\n");
				finished = true;
			}
		}
		// End message and change color back to white if the game ended in a death
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

			case "look":
				Look();
				break;

			case "take":
				player.TakeFromChest(command.SecondWord);
				break;

			case "place":
				player.PutInChest(command.SecondWord);
				break;

			case "health":
				player.SeeHealth();
				break;

			case "backpack":
				checkInventory(command.SecondWord);
				player.checkWeight(command.SecondWord);
				break;

			case "checkall":
				player.SeeHealth();
				checkInventory(command.SecondWord);
				player.checkWeight(command.SecondWord);
				break;

			case "use":
				player.use(command);
				break;

			case "craft":
				player.Craft(command);
				break;

			case "heal":
				player.Heal(command.SecondWord);
				break;

			case "die":
				player.Sepuccu();
				break;

			case "quit":
				wantToQuit = true;
				break;

			case "damage":
				player.damageNum(command.SecondWord);
				break;
		}

		// Checks if the CurrentRoom is the same as the winroom
		// if so make color green an print win message.
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
			// The _ is used couse the weight of the item is not necesairy here
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
		Console.WriteLine("You wander around a wierd fleshy location.");
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
		
		if (nextRoom.GetLock() == true)
		{
			Console.WriteLine("This door is locked.");
			return;
		}

		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}
}