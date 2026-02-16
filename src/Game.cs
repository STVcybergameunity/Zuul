using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;
	private Room winRoom;
	private Room nurgleRoom;
	private List<string> ItemLib = new List<string>();
	private List<string> RoomLib = new List<string>();
	Random rndnum = new Random();

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
		Room secret = new Room("in the nurgles mouth. Why did I decide to do this...");

		RoomLib.AddRange(carrion.GetShortDescription(), narrow.GetShortDescription(), corpse.GetShortDescription(), nurgle.GetShortDescription(),
		silence.GetShortDescription(), bile.GetShortDescription(), rash.GetShortDescription(), teeth.GetShortDescription(),
		secret.GetShortDescription());

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
		nurgle.AddExit("inside", secret);

		secret.AddNurgleLock();

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
		Item piston = new Item (50, "piston");
		Item ducttape = new Item (5, "ducttape");
		Item hydraulics = new Item (70, "hydraulics");
		Item meatpack = new Item (0, "meatpack");
		Item bookofmeat = new Item (0, "bookofmeat");

		Spell fireball = new Spell ("fireball", player.PlayerAttack(45,61));
		player.PlayerAddSpell(fireball.desc,fireball);

		ItemLib.AddRange(bandage.Description, medkit.Description, key.Description, nurgling.Description, metalrod.Description, piston.Description, ducttape.Description, hydraulics.Description);

		// And add them to the Rooms
		carrion.Chest.Put("bandage",bandage);
		carrion.Chest.Put("meatpack", meatpack);
		carrion.Chest.Put("nurgling", nurgling);
		carrion.Chest.Put("bookofmeat", bookofmeat);
		// carrion.Chest.Put("key", key);
		// carrion.Chest.Put("medkit", medkit);
		// carrion.Chest.Put("metalrod", metalrod);
		// carrion.Chest.Put("piston", piston);
		// carrion.Chest.Put("ducttape", ducttape);
		// carrion.Chest.Put("hydraulics", hydraulics);
		corpse.Chest.Put("medkit", medkit);
		bile.Chest.Put("key", key);
		nurgle.Chest.Put("nurgling", nurgling);
		rash.Chest.Put("metalrod", metalrod);
		corpse.Chest.Put("piston", piston);
		narrow.Chest.Put("ducttape", ducttape);

		// Create enemies
		Enemy mountofflesh = new Enemy(100, "mountofflesh", player.enemyAttack);

		// Add enemies to rooms
		narrow.addEnemy(mountofflesh);

		// Start game carrion
		player.CurrentRoom = carrion;

		winRoom = teeth;

		nurgleRoom = secret;
	}

	//  Main play routine. Loops until end of play.

	public void Play()
	{
		PrintWelcome();
		PrintHelp();
		Console.WriteLine($"\n", player.CurrentRoom.GetLongDescription());

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);

			// Checks if health is <= 0 then stops and makes color red
			if (player.health <= 0)
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

	public void Devoured()
	{
		if (player.CurrentRoom == nurgleRoom)
		{
			Console.WriteLine("Why did I do this. I should have known it would kill me.\n");
			player.health = 0;
		}
	}

	public Player GetPlayer()
    {
        return player;
    }

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
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
				checkInventory();
				player.checkWeight();
				break;

			case "checkall":
				player.SeeHealth();
				checkInventory();
				player.checkWeight();
				break;

			case "use":
				player.Use(command);
				break;

			case "craft":
				CraftHelp(command);
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
			case "cast":
				player.Spells(command);
				break;

			case "attack":
				Fight();
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

		if (player.CurrentRoom.enemy != null)
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.WriteLine($"There is a {player.CurrentRoom.enemy.GetEnemyDesc()} here.\n");
			Console.BackgroundColor = ConsoleColor.White;
		}
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	private void CraftHelp(Command command)
	{
		string craftable = player.Craft(command);
		if (craftable == "hydraulics")
		{
			player.PutInChest(command.SecondWord);player.PutInChest(command.ThirdWord);player.PutInChest(command.FourthWord);
			player.CurrentRoom.Chest.Get(command.SecondWord);player.CurrentRoom.Chest.Get(command.ThirdWord);player.CurrentRoom.Chest.Get(command.FourthWord);
			
			Console.WriteLine("You crafted hydraulics!\n");
			player.getBackpack().Put("hydraulics", new Item(70, "hydraulics"));
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("You can't craft that.\n");
			Console.ForegroundColor = ConsoleColor.White;
		}
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
	private void checkInventory()
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
			// if there is no secohydraulicsnd word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;
		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"There is no door to {direction}!\n{Console.ForegroundColor = ConsoleColor.White}See -help- for more info.");
			return;
		}
		
		if (nextRoom.GetLock() == true)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("This door is locked.");
			Console.ForegroundColor = ConsoleColor.White;
			return;
		}

		if (nextRoom.GetNurgleLock() == true)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("It's mouth is closed maybe I can open it.");
			Console.ForegroundColor = ConsoleColor.White;
			return;
		}

		if (player.CurrentRoom.enemy != null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"There is a {player.CurrentRoom.enemy.GetEnemyDesc()} in this room. Use attack or cast to kill it.\n");
			Console.ForegroundColor = ConsoleColor.White;
			return;
		}

		player.CurrentRoom = nextRoom;

		if (nextRoom.enemy != null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"There is a {player.CurrentRoom.enemy.GetEnemyDesc()} in this room. Use attack or cast to kill it.\n");
			Console.ForegroundColor = ConsoleColor.White;
		}

		player.Damage(5);
		player.LowHp();
		Devoured();
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}
	private void Fight()
	{	
		if (player.tel > 0)
		{
			player.tel -= 1;
		}

		if (player.CurrentRoom.enemy == null)
		{
			Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("There is nothing here to attack.\n"); Console.ForegroundColor = ConsoleColor.White;
			return;
		}

		int playerAttack;

		playerAttack = player.PlayerAttack(20,25);
		player.enemyAttack = player.EnemyAttack(15, 25);
		
		Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"You got hit for {player.enemyAttack} damage. You got {player.health}HP left\n"); Console.ForegroundColor = ConsoleColor.Green;
		if (player.CurrentRoom.enemy.GetEnemyCurrentHealth() > 0)
		{
			Console.WriteLine($"You hit the enemy for {playerAttack} damage. It still has {player.CurrentRoom.enemy.GetEnemyCurrentHealth()}HP remaining\n");
			Console.ForegroundColor = ConsoleColor.White;
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("You slayed the thing.");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(player.CurrentRoom.GetLongDescription());
		}

		if (!player.isAlive())
		{
			Console.WriteLine($"You died in combat due to {player.CurrentRoom.enemy.GetEnemyDesc()}");
		}

		if (!player.CurrentRoom.enemy.EnemyIsAlive())
		{
			player.CurrentRoom.enemy = null;
		}
	}
}
