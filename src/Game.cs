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
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the admin office");
		Room kitchen = new Room("in the pub's kitchen");
		Room cellar = new Room("in the pub's cellar");
		Room backyard = new Room("in the backyard");

		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);

		theatre.AddExit("west", outside);
		theatre.AddExit("south", backyard);

		pub.AddExit("east", outside);
		pub.AddExit("south", kitchen);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);

		office.AddExit("west", lab);

		kitchen.AddExit("down", cellar);
		kitchen.AddExit("north", pub);
		kitchen.AddExit("east", backyard);

		cellar.AddExit("up", kitchen);
		
		backyard.AddExit("west", kitchen);
		backyard.AddExit("north", theatre);

        // Create your Items here
		// Makes a item find more in Item.cs
        Item bandage = new Item(10, "bandage");
		Item medkit = new Item(40, "medkit");
		Item key = new Item(5, "key");
		Item metal = new Item(60, "metal");

		// And add them to the Rooms
		outside.Chest.Put("bandage",bandage);
		outside.Chest.Put("medkit", medkit);
		outside.Chest.Put("key", key);
		outside.Chest.Put("metal", metal);


		// Start game outside
		player.CurrentRoom = outside;

		winRoom = cellar;
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
				LowHp();
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				Look();
				break;
			case "health":
				SeeHealth();
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

	// Shows the players HP
	private void SeeHealth()
    {
        Console.WriteLine($"Your health is: {player.health}HP");
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

	// If the player is low show a message
	// If you are very low show a diffrent message
	private void LowHp()
	{
		if (player.health <= 40 && player.health >= 30)
		{
			Console.WriteLine($"U feel hurt.");
		}
		else if(player.health <= 20)
		{
			Console.WriteLine($"U feel miserable. U should heal!");
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

		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}
}
